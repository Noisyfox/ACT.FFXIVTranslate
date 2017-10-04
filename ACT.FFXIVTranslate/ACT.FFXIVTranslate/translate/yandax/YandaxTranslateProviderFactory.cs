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
            LanguageDef.BuildLangFromCulture("ru"),
            LanguageDef.BuildLangFromCulture("ko"),
        }.ToList();

        public string ProviderName => "Yandex Translate";

        public bool SupportAutoDetect => true;

        public List<LanguageDef> SupportedSrcLanguages => _allSupportedLanguages;

        public List<LanguageDef> SupportedDestLanguages => _allSupportedLanguages;
        public ProviderLegalInfo LegalInfo { get; } = new ProviderLegalInfo
        {
            LabelMain = "Powered by Yandex.Translate",
            LabelResult = "Powered by Yandex.Translate",
            LabelMainLink = "http://translate.yandex.com",
            LabelResultLink = "http://translate.yandex.com",
        };

        public string DefaultPublicKey => "trnsl.1.1.20170716T025951Z.13c73247084b012d.3404189299f91adf7792235bc7cf7fb7f3bd26a2";

        public ITranslateProvider CreateProvider(string apiKey, LanguageDef src, LanguageDef dst)
        {
            return new YandaxTranslateProvider(apiKey, src, dst);
        }
    }
}
