using System;
using System.Collections.Generic;

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

        ITranslateProvider CreateProvider(string apiKey, LanguageDef src, LanguageDef dst);
    }

    internal class LanguageDef
    {
        public static readonly LanguageDef Auto = new LanguageDef("auto", "Auto Detect", string.Empty);

        public string LangCode { get; }

        public string EnglishName { get; }

        public string LocalizedName { get; }

        public string DisplayName
        {
            get
            {
                if (string.IsNullOrEmpty(LocalizedName))
                {
                    return EnglishName;
                }

                return $"{LocalizedName}({EnglishName})";
            }
        }

        internal LanguageDef(string code, string eName, string lName)
        {
            LangCode = code;
            EnglishName = eName;
            LocalizedName = lName;
        }
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
}
