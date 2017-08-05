using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Xml;
using ACT.FFXIVTranslate.localization;

namespace ACT.FFXIVTranslate.translate.yandax
{
    internal class YandaxTranslateProvider : ITranslateProvider
    {
        private readonly string _apiKey;
        private readonly string _lang;

        public YandaxTranslateProvider(string apiKey, LanguageDef src, LanguageDef dst)
        {
            _apiKey = apiKey;
            _lang = src == null ? dst.LangCode : $"{src.LangCode}-{dst.LangCode}";
        }

        public void Translate(List<ChattingLine> chattingLines)
        {
            try
            {
                // Build text
                var textBuilder = new StringBuilder();
                var settings = new XmlWriterSettings {OmitXmlDeclaration = true};
                var textWriter = XmlWriter.Create(textBuilder, settings);
                textWriter.WriteStartElement("lines");
                foreach (var line in chattingLines)
                {
                    textWriter.WriteElementString("line", TextProcessor.NaiveCleanText(line.RawContent));
                }
                textWriter.WriteEndElement();
                textWriter.Flush();
                textWriter.Close();

                var text = textBuilder.ToString();
                string textResponse;
                using (var client = new HttpClient())
                using (var request = new HttpRequestMessage(HttpMethod.Post, "/api/v1.5/tr.json/translate"))
                {
                    client.BaseAddress = new Uri("https://translate.yandex.net");

                    var keyValues = new List<KeyValuePair<string, string>>();
                    keyValues.Add(new KeyValuePair<string, string>("key", _apiKey));
                    keyValues.Add(new KeyValuePair<string, string>("text", text));
                    keyValues.Add(new KeyValuePair<string, string>("lang", _lang));
                    keyValues.Add(new KeyValuePair<string, string>("options", "1"));
                    keyValues.Add(new KeyValuePair<string, string>("format", "html"));

                    request.Content = new FormUrlEncodedContent(keyValues);
                    var response = client.SendAsync(request).Result;
                    textResponse = response.Content.ReadAsStringAsync().Result;
                }

                // read json
                var ser = new DataContractJsonSerializer(typeof(YandexTranslateResult));
                var result =
                    (YandexTranslateResult)
                    ser.ReadObject(new MemoryStream(Encoding.UTF8.GetBytes(textResponse)));

                switch (result.Code)
                {
                    case 200:
                        break;

                    // Faild
                    case 401:
                        throw new TranslateException(TranslateException.ExceptionReason.InvalidApiKey,
                            "Invalid API key",
                            null);
                    case 402:
                        throw new TranslateException(TranslateException.ExceptionReason.InvalidApiKey,
                            "Blocked API key",
                            null);
                    case 404:
                        throw new TranslateException(TranslateException.ExceptionReason.ApiLimitExceed,
                            "Exceeded the daily limit on the amount of translated text", null);
                    case 413:
                        throw new TranslateException(TranslateException.ExceptionReason.ApiLimitExceed,
                            "Exceeded the maximum text size", null);
                    case 422:
                        throw new TranslateException(TranslateException.ExceptionReason.GeneralServiceError,
                            "The text cannot be translated", null);
                    case 501:
                        throw new TranslateException(TranslateException.ExceptionReason.DirectionNotSupported,
                            "The specified translation direction is not supported", null);
                    default:
                        throw new TranslateException(TranslateException.ExceptionReason.GeneralServiceError,
                            $"Code = {result.Code}, Msg = {result.Message}", null);
                }


                var translatedLinesXml = result.Text[0];
                // Parse xml
                var doc = new XmlDocument();
                doc.LoadXml(translatedLinesXml);
                var nodes = doc.SelectNodes("lines/line");
                if (nodes == null || nodes.Count != chattingLines.Count)
                {
                    // Error
                    throw new TranslateException(TranslateException.ExceptionReason.InternalError,
                        "Translate result count mismatch.", null);
                }

                foreach (var p in chattingLines.Zip(nodes.Cast<XmlNode>(),
                    (a, b) => new KeyValuePair<ChattingLine, XmlNode>(a, b)))
                {
                    p.Key.TranslatedContent = p.Value.InnerText;
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

        [DataContract]
        internal class YandexTranslateResult
        {
            [DataContract]
            internal class DetectedLang
            {
                [DataMember(Name = "lang", IsRequired = true)] internal string Lang;
            }

            [DataMember(Name = "code", IsRequired = true)] internal int Code;

            [DataMember(Name = "message", IsRequired = false)] internal string Message;

            [DataMember(Name = "detected", IsRequired = false)] internal DetectedLang Detected;

            [DataMember(Name = "lang", IsRequired = false)] internal string Lang;

            [DataMember(Name = "text", IsRequired = false)] internal string[] Text;
        }
    }
}
