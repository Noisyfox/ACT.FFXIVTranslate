using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using ACT.FoxCommon.localization;
using Newtonsoft.Json;

namespace ACT.FFXIVTranslate.translate.microsoft
{
    internal class RequestItem
    {
        public readonly string Text;

        public RequestItem(string text)
        {
            Text = text;
        }
    }

    internal class ResponseItem
    {
        internal class TranslationItem
        {
            public string text;
        }

        public List<TranslationItem> translations;
    }

    internal class MicrosoftTranslateProvider : DefaultTranslateProvider
    {
        private readonly AzureAuthToken _token;
        private readonly string _langFrom;
        private readonly string _langTo;


        public MicrosoftTranslateProvider(string apiKey, LanguageDef src, LanguageDef dst)
        {
            _token = new AzureAuthToken(apiKey);
            _langFrom = src?.LangCode;
            _langTo = dst.LangCode;
        }

        private const string ServiceUri = "https://api.cognitive.microsofttranslator.com/translate?api-version=3.0";

        public override void Translate(List<ChattingLine> chattingLines)
        {
            try
            {
                // Build text
                var uri = $"{ServiceUri}&from={_langFrom}&to={_langTo}";
                Console.WriteLine("Uri is: {0}.", uri);

                var bodyContent = new List<RequestItem>();
                foreach (var line in chattingLines)
                {
                    bodyContent.Add(new RequestItem(line.CleanedContent));
                }
                var requestBody = JsonConvert.SerializeObject(bodyContent, Formatting.None);
                Console.WriteLine("Request body is: {0}.", requestBody);

                // Call Microsoft translate API.
                DoRequest(GetAuthToken(), uri, requestBody, out string responseBody, out HttpStatusCode statusCode);

                // Parse result
                switch (statusCode)
                {
                    case HttpStatusCode.OK:
                        Console.WriteLine("Request status is OK. Response body is:");
                        Console.WriteLine(responseBody);
                        Console.WriteLine("Result of translate array method is:");

                        var response = JsonConvert.DeserializeObject<List<ResponseItem>>(responseBody);
                        var sourceTextCounter = 0;
                        foreach (var line in response)
                        {
                            var result = line.translations[0].text;
                            Console.WriteLine("\n\nSource text: {0}\nTranslated Text: {1}", chattingLines[sourceTextCounter].RawContent, result);
                            chattingLines[sourceTextCounter].TranslatedContent = result;
                            sourceTextCounter++;
                        }
                        break;
                    case HttpStatusCode.Unauthorized:
                        throw new TranslateException(TranslateException.ExceptionReason.InvalidApiKey,
                            "Invalid API key",
                            null);
                    default:
                        Console.WriteLine("Request status code is: {0}.", statusCode);
                        Console.WriteLine("Request error message: {0}.", responseBody);
                        throw new TranslateException(TranslateException.ExceptionReason.GeneralServiceError,
                            responseBody, null);
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

        private string GetAuthToken()
        {
            string authToken;
            try
            {
                authToken = _token.GetAccessToken();
            }
            catch (HttpRequestException ex)
            {
                switch (_token.RequestStatusCode)
                {
                    case HttpStatusCode.Unauthorized:
                        throw new TranslateException(TranslateException.ExceptionReason.InvalidApiKey,
                            "Invalid API key",
                            null);
                    case HttpStatusCode.Forbidden:
                        throw new TranslateException(TranslateException.ExceptionReason.ApiLimitExceed,
                            "Account quota has been exceeded.",
                            null);
                    default:
                        throw new TranslateException(TranslateException.ExceptionReason.GeneralServiceError,
                            null, ex);
                }
            }

            return authToken;
        }

        private void DoRequest(string authToken, string uri, string requestBody,
            out string responseBody, out HttpStatusCode statusCode)
        {
            string _responseBody;
            HttpStatusCode _statusCode;

            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

            using (var client = ProxyFactory.Instance.NewClient())
            using (var request = new HttpRequestMessage())
            {
                request.Method = HttpMethod.Post;
                request.RequestUri = new Uri(uri);
                request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
                request.Headers.Add("Authorization", authToken);
                var response = client.SendAsync(request).Result;
                _responseBody = response.Content.ReadAsStringAsync().Result;
                _statusCode = response.StatusCode;
            }

            responseBody = _responseBody;
            statusCode = _statusCode;
        }
    }
}
