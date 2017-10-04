using System;
using System.Collections.Generic;
using ACT.FFXIVTranslate.localization;

namespace ACT.FFXIVTranslate.translate
{
    internal interface ITranslateProvider
    {
        void Translate(List<ChattingLine> chattingLines);
        
        /// <param name="chattingLine"></param>
        /// <returns>False if this line can be ignored (usually is empty), True otherwise.</returns>
        bool PreprocessLine(ChattingLine chattingLine);
    }

    /// <summary>
    /// A provider that use TextProcessor.NaiveCleanText() as the econtent preprossor
    /// and ignores all empty lines after that.
    /// </summary>
    internal abstract class DefaultTranslateProvider : ITranslateProvider
    {
        public abstract void Translate(List<ChattingLine> chattingLines);

        public virtual bool PreprocessLine(ChattingLine chattingLine)
        {
            chattingLine.CleanedContent = TextProcessor.NaiveCleanText(chattingLine.RawContent);

            return !string.IsNullOrEmpty(chattingLine.CleanedContent);
        }
    }

    internal interface ITranslaterProviderFactory
    {
        string ProviderName { get; }

        bool SupportAutoDetect { get; }

        List<LanguageDef> SupportedSrcLanguages { get; }

        List<LanguageDef> SupportedDestLanguages { get; }

        ProviderLegalInfo LegalInfo { get; }

        /// <summary>
        /// The default API key for free public use. A gift from me :)
        /// </summary>
        string DefaultPublicKey { get; }

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
