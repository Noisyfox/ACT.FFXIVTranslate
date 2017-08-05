using System;
using System.Collections.Generic;
using ACT.FFXIVTranslate.localization;

namespace ACT.FFXIVTranslate.translate
{
    internal interface ITranslateProvider
    {
        void Translate(List<ChattingLine> chattingLines);
    }

    internal interface ITranslaterProviderFactory
    {
        string ProviderName { get; }

        bool SupportAutoDetect { get; }

        List<LanguageDef> SupportedSrcLanguages { get; }

        List<LanguageDef> SupportedDestLanguages { get; }

        ProviderLegalInfo LegalInfo { get; }

        ITranslateProvider CreateProvider(string apiKey, LanguageDef src, LanguageDef dst);
    }

    internal class TranslateException : Exception
    {
        public enum ExceptionReason
        {
            InvalidApiKey,
            ApiLimitExceed,
            DirectionNotSupported,
            GeneralServiceError,
            NetworkError,
            InternalError,
            UnknownError
        }

        public ExceptionReason Reason { get; }

        public TranslateException(ExceptionReason reason, string message, Exception innerException)
            : base(message, innerException)
        {
            Reason = reason;
        }
    }

    public class ProviderLegalInfo
    {
        public string LabelMain { get; set; }
        public string LabelMainLink { get; set; }
        public string LabelResult { get; set; }
        public string LabelResultLink { get; set; }
    }
}
