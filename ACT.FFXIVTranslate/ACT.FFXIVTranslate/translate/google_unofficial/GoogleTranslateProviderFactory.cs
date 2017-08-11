using System.Collections.Generic;
using System.Linq;
using ACT.FFXIVTranslate.localization;

namespace ACT.FFXIVTranslate.translate.google_unofficial
{
    internal class GoogleTranslateProviderFactory : ITranslaterProviderFactory
    {
        private readonly List<LanguageDef> _allSupportedLanguages = new[]
        {
            LanguageDef.BuildLangFromCulture("zh"),
            LanguageDef.BuildLangFromCulture("en"),
            LanguageDef.BuildLangFromCulture("ja"),
            LanguageDef.BuildLangFromCulture("de"),
            LanguageDef.BuildLangFromCulture("fr"),
        }.ToList();

        public string ProviderName => "Google Translate (unofficial)";
        public bool SupportAutoDetect => true;
        public List<LanguageDef> SupportedSrcLanguages => _allSupportedLanguages;
        public List<LanguageDef> SupportedDestLanguages => _allSupportedLanguages;
        public ProviderLegalInfo LegalInfo => null;
        public string DefaultPublicKey => null;

        public ITranslateProvider CreateProvider(string apiKey, LanguageDef src, LanguageDef dst)
        {
            return new GoogleTranslateProvider(src, dst);
        }
    }
}
