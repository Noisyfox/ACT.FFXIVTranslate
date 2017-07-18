using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ACT.FFXIVTranslate.translate.yandax
{
    class YandaxTranslateProviderFactory : ITranslaterProviderFactory
    {
        private readonly List<LanguageDef> _allSupportedLanguages = new[]
        {
            new LanguageDef("zh", "Chinese", ""),
            new LanguageDef("en", "English", ""),
            new LanguageDef("ja", "Japanese", ""),
            new LanguageDef("de", "German", ""),
            new LanguageDef("fr", "French", ""),
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
