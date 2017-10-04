using System.Collections.Generic;
using System.Linq;
using ACT.FFXIVTranslate.localization;

namespace ACT.FFXIVTranslate.translate.youdao
{
    internal class YoudaoTranslateProviderFactory : ITranslaterProviderFactory
    {
        private readonly List<LanguageDef> _allSupportedLanguages = new[]
        {
            LanguageDef.BuildLangFromCulture("zh-CHS"),
            LanguageDef.BuildLangFromCulture("en", "EN"),
            LanguageDef.BuildLangFromCulture("ja"),
            LanguageDef.BuildLangFromCulture("fr"),
            LanguageDef.BuildLangFromCulture("ru"),
            LanguageDef.BuildLangFromCulture("ko"),
        }.ToList();

        public string ProviderName => "有道翻译";
        public bool SupportAutoDetect => true;
        public List<LanguageDef> SupportedSrcLanguages => _allSupportedLanguages;
        public List<LanguageDef> SupportedDestLanguages => _allSupportedLanguages;
        public ProviderLegalInfo LegalInfo => null;
        public string DefaultPublicKey => null;

        public ITranslateProvider CreateProvider(string apiKey, LanguageDef src, LanguageDef dst)
        {
            return new YoudaoTranslateProvider(apiKey, src, dst);
        }
    }
}
