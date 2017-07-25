using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ACT.FFXIVTranslate.localization;

namespace ACT.FFXIVTranslate.translate.yandax
{
    class YandaxTranslateProviderFactory : ITranslaterProviderFactory
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

        public ITranslateProvider CreateProvider(string apiKey, LanguageDef src, LanguageDef dst)
        {
            return new YandaxTranslateProvider(apiKey, src, dst);
        }
    }
}
