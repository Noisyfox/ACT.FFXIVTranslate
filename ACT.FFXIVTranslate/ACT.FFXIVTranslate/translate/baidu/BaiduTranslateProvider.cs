using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using ACT.FFXIVTranslate.localization;
using ACT.FoxCommon;
using ACT.FoxCommon.localization;
using Newtonsoft.Json.Linq;

namespace ACT.FFXIVTranslate.translate.baidu
{
    internal class BaiduTranslateProvider : DefaultTranslateProvider
    {
        private readonly string _appId;
        private readonly string _secret;
        private readonly string _langFrom;
        private readonly string _langTo;


        public BaiduTranslateProvider(string apiKey, LanguageDef src, LanguageDef dst)
        {
            var kp = apiKey.Split(':');
            if (kp.Length < 2)
            {
                _appId = "";
                _secret = "";
            }
            else
            {
                _appId = kp[0];
                _secret = kp[1];
            }
            _langFrom = src?.LangCode ?? "auto";
            _langTo = dst.LangCode;
        }

        public override void Translate(List<ChattingLine> chattingLines)
        {
            try
            {
                // Build text
                var query = string.Join("\n", chattingLines.Select(it => it.CleanedContent));

                // 1. Generate a salt
                var salt = Utils.TimestampMillisFromDateTime(DateTime.Now).ToString();
                // 2. Calculate sign
                var md5Input = Encoding.UTF8.GetBytes($"{_appId}{query}{salt}{_secret}");
                var md5 = System.Security.Cryptography.MD5.Create();
                var hash = md5.ComputeHash(md5Input);
                var sign = string.Join("", hash.Select(it => it.ToString("x2")));

                string textResponse;
                using (var client = ProxyFactory.Instance.NewClient())
                using (var request = new HttpRequestMessage(HttpMethod.Post, "/api/trans/vip/translate"))
                {
                    client.BaseAddress = new Uri("https://fanyi-api.baidu.com");

                    var keyValues = new List<KeyValuePair<string, string>>();
                    keyValues.Add(new KeyValuePair<string, string>("q", query));
                    keyValues.Add(new KeyValuePair<string, string>("from", _langFrom));
                    keyValues.Add(new KeyValuePair<string, string>("to", _langTo));
                    keyValues.Add(new KeyValuePair<string, string>("appid", _appId));
                    keyValues.Add(new KeyValuePair<string, string>("salt", salt));
                    keyValues.Add(new KeyValuePair<string, string>("sign", sign));

                    request.Content = new FormUrlEncodedContent(keyValues);
                    var response = client.SendAsync(request).Result;
                    response.EnsureSuccessStatusCode();
                    textResponse = response.Content.ReadAsStringAsync().Result;
                    Debug.WriteLine(textResponse);
                }

                // read json
                var result = JObject.Parse(textResponse);
                if (((IDictionary<string, JToken>) result).ContainsKey("error_code"))
                {
                    // Something wrong
                    var errorCode = (string)result["error_code"];
                    switch (errorCode)
                    {
                        case "52000": // This one is good
                            break;
                        case "52001":
                        case "52002":
                            throw new TranslateException(TranslateException.ExceptionReason.GeneralServiceError,
                                (string) result["error_msg"], null);
                        case "52003":
                            throw new TranslateException(TranslateException.ExceptionReason.InvalidApiKey,
                                "AppId Error", null);
                        case "54000":
                            throw new TranslateException(TranslateException.ExceptionReason.InternalError,
                                "API Params Missing", null);
                        case "54001":
                            throw new TranslateException(TranslateException.ExceptionReason.InvalidApiKey,
                                "Secret Error", null);
                        case "54003":
                        case "54005":
                            throw new TranslateException(TranslateException.ExceptionReason.ApiLimitExceed,
                                "Too Frequently", null);
                        case "54004":
                            throw new TranslateException(TranslateException.ExceptionReason.ApiLimitExceed,
                                "Insufficient Account Balance", null);
                        case "58000":
                            throw new TranslateException(TranslateException.ExceptionReason.GeneralServiceError,
                                "Limited IP",
                                null);
                        case "58001":
                            throw new TranslateException(TranslateException.ExceptionReason.DirectionNotSupported,
                                (string) result["error_msg"], null);
                        default:
                            throw new TranslateException(TranslateException.ExceptionReason.GeneralServiceError,
                                (string) result["error_msg"], null);
                    }
                }

                var transResult = (JArray) result["trans_result"];
                if (transResult.Count >= chattingLines.Count)
                {
                    for (var i = 0; i < chattingLines.Count; i++)
                    {
                        chattingLines[i].TranslatedContent = (string) transResult[i]["dst"];
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
    }
}
