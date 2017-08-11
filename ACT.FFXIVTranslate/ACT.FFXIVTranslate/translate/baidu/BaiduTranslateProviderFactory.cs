using System.Collections.Generic;
using System.Linq;
using ACT.FFXIVTranslate.localization;

namespace ACT.FFXIVTranslate.translate.baidu
{
    internal class BaiduTranslateProviderFactory : ITranslaterProviderFactory
    {
        private readonly List<LanguageDef> _allSupportedLanguages = new[]
        {
            LanguageDef.BuildLangFromCulture("zh"),
            LanguageDef.BuildLangFromCulture("en"),
            LanguageDef.BuildLangFromCulture("ja", "jp"),
            LanguageDef.BuildLangFromCulture("de"),
            LanguageDef.BuildLangFromCulture("fr", "fra"),
        }.ToList();

        public string ProviderName => "百度翻译";
        public bool SupportAutoDetect => true;
        public List<LanguageDef> SupportedSrcLanguages => _allSupportedLanguages;
        public List<LanguageDef> SupportedDestLanguages => _allSupportedLanguages;
        public ProviderLegalInfo LegalInfo => null;
        public string DefaultPublicKey => null;

        public ITranslateProvider CreateProvider(string apiKey, LanguageDef src, LanguageDef dst)
        {
            return new BaiduTranslateProvider(apiKey, src, dst);
        }
    }
}
