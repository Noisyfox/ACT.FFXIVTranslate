using System.Collections.Generic;
using System.Linq;
using ACT.FFXIVTranslate.localization;

namespace ACT.FFXIVTranslate.translate.yandax
{
    internal class YandaxTranslateProviderFactory : ITranslaterProviderFactory
    {
        private readonly List<LanguageDef> _allSupportedLanguages = new[]
        {
            LanguageDef.BuildLangFromCulture("zh"),
            LanguageDef.BuildLangFromCulture("en"),
            LanguageDef.BuildLangFromCulture("ja"),
            LanguageDef.BuildLangFromCulture("de"),
            LanguageDef.BuildLangFromCulture("fr"),
        }.ToList();

        public string ProviderName { get; } = "Yandex Translate";

        public bool SupportAutoDetect { get; } = true;

        public List<LanguageDef> SupportedSrcLanguages => _allSupportedLanguages;

        public List<LanguageDef> SupportedDestLanguages => _allSupportedLanguages;
        public ProviderLegalInfo LegalInfo { get; } = new ProviderLegalInfo
        {
            LabelMain = "Powered by Yandex.Translate",
            LabelResult = "Powered by Yandex.Translate",
            LabelMainLink = "http://translate.yandex.com",
            LabelResultLink = "http://translate.yandex.com",
        };

        public ITranslateProvider CreateProvider(string apiKey, LanguageDef src, LanguageDef dst)
        {
            return new YandaxTranslateProvider(apiKey, src, dst);
        }
    }
}
