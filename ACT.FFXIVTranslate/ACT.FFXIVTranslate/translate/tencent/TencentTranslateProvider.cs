using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using ACT.FoxCommon.localization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ACT.FFXIVTranslate.translate.tencent
{
    internal class TencentTranslateProvider : DefaultTranslateProvider
    {
        private const string SERVICE = "tmt";
        private const string HOST = SERVICE + ".tencentcloudapi.com";
        private const string ALGORITHM = "TC3-HMAC-SHA256";

        private readonly string _secretId;
        private readonly string _secretKey;
        private readonly string _langFrom;
        private readonly string _langTo;

        public TencentTranslateProvider(string apiKey, LanguageDef src, LanguageDef dst)
        {
            var kp = apiKey.Split(':');
            if (kp.Length < 2)
            {
                _secretId = "";
                _secretKey = "";
            }
            else
            {
                _secretId = kp[0];
                _secretKey = kp[1];
            }

            _langFrom = src?.LangCode ?? "auto";
            _langTo = dst.LangCode;
        }

        public override void Translate(List<ChattingLine> chattingLines)
        {
            try
            {
                // Build request json
                var query = new Dictionary<string, object>();
                query["Source"] = _langFrom;
                query["Target"] = _langTo;
                query["ProjectId"] = 0;
                query["SourceTextList"] = chattingLines.Select(it => it.CleanedContent).ToList();

                var queryBody = JsonConvert.SerializeObject(query, Formatting.None);

                // Calculate signature, based on https://cloud.tencent.com/document/api/551/30636
                // and https://github.com/TencentCloud/tencentcloud-sdk-dotnet/blob/376182fa16beefb19d09cd6bceae536689b62978/TencentCloud/Common/AbstractClient.cs#L234

                var timestamp = ToTimestamp() / 1000;
                var date = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(timestamp)
                    .ToString("yyyy-MM-dd");
                var credentialScope = $"{date}/{SERVICE}/tc3_request";

                // 1. Build CanonicalRequest
                var canonicalRequest =
                    $"POST\n" +
                    $"/\n" +
                    $"\n" +
                    $"content-type:application/json; charset=utf-8\n" +
                    $"host:{HOST}\n" +
                    $"\n" +
                    $"content-type;host\n" +
                    $"{SHA256Hex(queryBody)}";

                // 2. Build StringToSign
                var stringToSign =
                    $"{ALGORITHM}\n" +
                    $"{timestamp}\n" +
                    $"{credentialScope}\n" +
                    $"{SHA256Hex(canonicalRequest)}";

                // 3. Calculate Signature
                var tc3SecretKey = Encoding.UTF8.GetBytes("TC3" + _secretKey);
                var secretDate = HmacSHA256(tc3SecretKey, Encoding.UTF8.GetBytes(date));
                var secretService = HmacSHA256(secretDate, Encoding.UTF8.GetBytes(SERVICE));
                var secretSigning = HmacSHA256(secretService, Encoding.UTF8.GetBytes("tc3_request"));
                var signatureBytes = HmacSHA256(secretSigning, Encoding.UTF8.GetBytes(stringToSign));
                var signature = BitConverter.ToString(signatureBytes).Replace("-", "").ToLower();

                // 4. Build Authorization
                var authorization =
                    $"{ALGORITHM} Credential={_secretId}/{credentialScope}, SignedHeaders=content-type;host, Signature={signature}";

                // Build headers
                var headers = new Dictionary<string, string>();
                headers["Authorization"] = authorization;
                headers["Host"] = HOST;
                headers["Content-Type"] = "application/json; charset=utf-8";
                headers["X-TC-Action"] = "TextTranslateBatch";
                headers["X-TC-Timestamp"] = timestamp.ToString();
                headers["X-TC-Version"] = "2018-03-21";
                headers["X-TC-Region"] = "ap-shanghai";

                // Make http request
                string textResponse;
                using (var client = ProxyFactory.Instance.NewClient())
                using (var request = new HttpRequestMessage(HttpMethod.Post, "/"))
                {
                    client.BaseAddress = new Uri($"https://{HOST}");

                    request.Content = new StringContent(queryBody, Encoding.UTF8, "application/json");
                    foreach (var header in headers)
                    {
                        request.Headers.TryAddWithoutValidation(header.Key, header.Value);
                    }

                    var response = client.SendAsync(request).Result;
                    response.EnsureSuccessStatusCode();
                    textResponse = response.Content.ReadAsStringAsync().Result;
                    Debug.WriteLine(textResponse);
                }

                // read json
                var result = (JObject) JObject.Parse(textResponse)["Response"];
                var error = (JObject) result["Error"];
                if (error != null)
                {
                    // handle error
                    var errorCode = (string) error["Code"];
                    var errorMessage = (string) error["Message"];

                    if (errorCode.StartsWith("AuthFailure."))
                    {
                        throw new TranslateException(TranslateException.ExceptionReason.InvalidApiKey,
                            errorCode + ": " + errorMessage,
                            null);
                    }

                    switch (errorCode)
                    {
                        case "LimitExceeded":
                        case "RequestLimitExceeded":
                            throw new TranslateException(TranslateException.ExceptionReason.ApiLimitExceed,
                                errorCode + ": " + errorMessage, null);

                        case "UnsupportedOperation.UnSupportedTargetLanguage":
                        case "UnsupportedOperation.UnsupportedLanguage":
                        case "UnsupportedOperation.UnsupportedSourceLanguage":
                            throw new TranslateException(TranslateException.ExceptionReason.DirectionNotSupported,
                                errorCode + ": " + errorMessage, null);

                        case "UnsupportedOperation.TextTooLong":
                            throw new TranslateException(TranslateException.ExceptionReason.InternalError,
                                errorCode + ": " + errorMessage, null);

                        default:
                            throw new TranslateException(TranslateException.ExceptionReason.GeneralServiceError,
                                textResponse, null);
                    }
                }

                var translation = (JArray) result["TargetTextList"];
                if (translation.Count >= chattingLines.Count)
                {
                    for (var i = 0; i < chattingLines.Count; i++)
                    {
                        chattingLines[i].TranslatedContent = (string) translation[i];
                    }
                }
                else
                {
                    // TODO: Oops!
                }
            }
            catch (TranslateException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new TranslateException(TranslateException.ExceptionReason.UnknownError, null, ex);
            }
        }

        #region Helper functions

        // https://github.com/TencentCloud/tencentcloud-sdk-dotnet/blob/376182fa16beefb19d09cd6bceae536689b62978/TencentCloud/Common/Sign.cs

        private static string SHA256Hex(string s)
        {
            using (SHA256 algo = SHA256.Create())
            {
                byte[] hashbytes = algo.ComputeHash(Encoding.UTF8.GetBytes(s));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < hashbytes.Length; ++i)
                {
                    builder.Append(hashbytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        private static byte[] HmacSHA256(byte[] key, byte[] msg)
        {
            using (HMACSHA256 mac = new HMACSHA256(key))
            {
                return mac.ComputeHash(msg);
            }
        }

        // https://github.com/TencentCloud/tencentcloud-sdk-dotnet/blob/376182fa16beefb19d09cd6bceae536689b62978/TencentCloud/Common/AbstractClient.cs#L472
        private static long ToTimestamp()
        {
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0, 0));
            DateTime nowTime = DateTime.Now;
            long unixTime = (long)Math.Round((nowTime - startTime).TotalMilliseconds, MidpointRounding.AwayFromZero);
            return unixTime;
        }

        #endregion

    }
}
