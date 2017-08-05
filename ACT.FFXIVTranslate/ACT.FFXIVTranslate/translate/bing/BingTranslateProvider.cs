using ACT.FFXIVTranslate.localization;
using System;
using System.Collections.Generic;
using System.Linq;
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
                        foreach (var line in chattingLines)
                        {
                            textWriter.WriteElementString("string", NsArrays,
                                TextProcessor.NaiveCleanText(line.RawContent));
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

                var authToken = _token.GetAccessToken();

                var task = TaskEx.Run(async () =>
                {
                    using (var client = new HttpClient())
                    using (var request = new HttpRequestMessage())
                    {
                        request.Method = HttpMethod.Post;
                        request.RequestUri = new Uri(ServiceUri);
                        request.Content = new StringContent(requestBody, Encoding.UTF8, "text/xml");
                        request.Headers.Add("Authorization", authToken);
                        var response = await client.SendAsync(request);
                        var responseBody = await response.Content.ReadAsStringAsync();
                        switch (response.StatusCode)
                        {
                            case HttpStatusCode.OK:
                                Console.WriteLine("Request status is OK. Response body is:");
                                Console.WriteLine(responseBody);
                                Console.WriteLine("Result of translate array method is:");
                                var doc = XDocument.Parse(responseBody);
                                var ns = XNamespace.Get(
                                    "http://schemas.datacontract.org/2004/07/Microsoft.MT.Web.Service.V2");
                                var sourceTextCounter = 0;
                                foreach (XElement xe in doc.Descendants(ns + "TranslateArrayResponse"))
                                {
                                    foreach (var node in xe.Elements(ns + "TranslatedText"))
                                    {
                                        Console.WriteLine("\n\nSource text: {0}\nTranslated Text: {1}",
                                            chattingLines[sourceTextCounter].RawContent, node.Value);
                                        chattingLines[sourceTextCounter].TranslatedContent = node.Value;
                                    }
                                    sourceTextCounter++;
                                }
                                break;
                            default:
                                Console.WriteLine("Request status code is: {0}.", response.StatusCode);
                                Console.WriteLine("Request error message: {0}.", responseBody);
                                break;
                        }
                    }
                });

                while (!task.IsCompleted)
                {
                    System.Threading.Thread.Yield();
                }
                if (task.IsFaulted)
                {
                    throw task.Exception;
                }
                if (task.IsCanceled)
                {
                    throw new Exception("Timeout obtaining access token.");
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
