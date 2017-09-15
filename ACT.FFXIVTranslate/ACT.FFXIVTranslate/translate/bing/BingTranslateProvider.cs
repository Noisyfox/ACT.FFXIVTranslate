using ACT.FFXIVTranslate.localization;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace ACT.FFXIVTranslate.translate.bing
{
    internal class BingTranslateProvider : ITranslateProvider
    {
        private readonly AzureAuthToken _token;
        private readonly string _langFrom;
        private readonly string _langTo;


        public BingTranslateProvider(string apiKey, LanguageDef src, LanguageDef dst)
        {
            _token = new AzureAuthToken(apiKey);
            _langFrom = src?.LangCode;
            _langTo = dst.LangCode;
        }

        private const string ServiceUri = "https://api.microsofttranslator.com/v2/Http.svc/TranslateArray";

        private const string NsService = "http://schemas.datacontract.org/2004/07/Microsoft.MT.Web.Service.V2";
        private const string NsArrays = "http://schemas.microsoft.com/2003/10/Serialization/Arrays";


        public void Translate(List<ChattingLine> chattingLines)
        {
            try
            {
                // Build text
                var availableLines = new List<ChattingLine>();
                chattingLines.ForEach(it =>
                {
                    it.CleanedContent = TextProcessor.NaiveCleanText(it.RawContent);
                    if (!string.IsNullOrEmpty(it.CleanedContent))
                    {
                        availableLines.Add(it);
                    }
                    else
                    {
                        it.TranslatedContent = string.Empty;
                    }
                });

                // Make sure we do have something to translate
                if (availableLines.Count == 0)
                {
                    return;
                }

                var textBuilder = new StringBuilder();
                var settings = new XmlWriterSettings {OmitXmlDeclaration = true};
                var textWriter = XmlWriter.Create(textBuilder, settings);
                textWriter.WriteStartElement("TranslateArrayRequest");
                {
                    textWriter.WriteElementString("AppId", null);
                    textWriter.WriteElementString("From", _langFrom);

                    textWriter.WriteStartElement("Options");
                    {
                        textWriter.WriteElementString("Category", NsService, "general");
                        textWriter.WriteElementString("ContentType", NsService, "text/html");
                        textWriter.WriteElementString("ReservedFlags", NsService, null);
                        textWriter.WriteElementString("State", NsService, "0");
                        textWriter.WriteElementString("Uri", NsService, "all");
                        textWriter.WriteElementString("User", NsService, "all");

                        textWriter.WriteEndElement();
                    }

                    textWriter.WriteStartElement("Texts");
                    {
                        foreach (var line in availableLines)
                        {
                            textWriter.WriteElementString("string", NsArrays,
                                WebUtility.HtmlEncode(line.CleanedContent));
                        }

                        textWriter.WriteEndElement();
                    }

                    textWriter.WriteElementString("To", _langTo);

                    textWriter.WriteEndElement();
                }
                textWriter.Flush();
                textWriter.Close();
                var requestBody = textBuilder.ToString();
                Console.WriteLine("Request body is: {0}.", requestBody);

                // Call Microsoft translate API.
                DoRequest(GetAuthToken(), requestBody, out string responseBody, out HttpStatusCode statusCode);

                // Parse result
                switch (statusCode)
                {
                    case HttpStatusCode.OK:
                        Console.WriteLine("Request status is OK. Response body is:");
                        Console.WriteLine(responseBody);
                        Console.WriteLine("Result of translate array method is:");
                        var doc = XDocument.Parse(responseBody);
                        var ns = XNamespace.Get(NsService);
                        var sourceTextCounter = 0;
                        foreach (var xe in doc.Descendants(ns + "TranslateArrayResponse"))
                        {
                            foreach (var node in xe.Elements(ns + "TranslatedText"))
                            {
                                var result = WebUtility.HtmlDecode(node.Value);
                                Console.WriteLine("\n\nSource text: {0}\nTranslated Text: {1}",
                                    availableLines[sourceTextCounter].RawContent, result);
                                availableLines[sourceTextCounter].TranslatedContent = result;
                            }
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

        private void DoRequest(string authToken, string requestBody,
            out string responseBody, out HttpStatusCode statusCode)
        {
            string _responseBody;
            HttpStatusCode _statusCode;

            using (var client = ProxyFactory.Instance.NewClient())
            using (var request = new HttpRequestMessage())
            {
                request.Method = HttpMethod.Post;
                request.RequestUri = new Uri(ServiceUri);
                request.Content = new StringContent(requestBody, Encoding.UTF8, "text/xml");
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
