using System.Collections.Generic;
using System.Linq;
using ACT.FoxCommon.localization;

namespace ACT.FFXIVTranslate.translate.google_unofficial
{
    internal class GoogleTranslateProviderFactory : ITranslateProviderFactory
    {
        private readonly List<LanguageDef> _allSupportedLanguages = new[]
        {
            LanguageDef.BuildLangFromCulture("zh-CN"),
            LanguageDef.BuildLangFromCulture("zh-TW"),
            LanguageDef.BuildLangFromCulture("en"),
            LanguageDef.BuildLangFromCulture("ja"),
            LanguageDef.BuildLangFromCulture("de"),
            LanguageDef.BuildLangFromCulture("fr"),
            LanguageDef.BuildLangFromCulture("ru"),
            LanguageDef.BuildLangFromCulture("ko"),
        }.ToList();

        public string ProviderId => "Google Translate (unofficial)";
        public string ProviderName => LocalizationBase.GetString("translateProviderNameGoogleUnofficial", ProviderId);
        public bool SupportAutoDetect => true;
        public List<LanguageDef> SupportedSrcLanguages => _allSupportedLanguages;
        public List<LanguageDef> GetSupportedDestLanguages(LanguageDef srcLanguage)
        {
            return _allSupportedLanguages.Where(it => it != srcLanguage).ToList();
        }
        public ProviderLegalInfo LegalInfo => null;
        public string DefaultPublicKey => null;

        public ITranslateProvider CreateProvider(string apiKey, LanguageDef src, LanguageDef dst)
        {
            return new GoogleTranslateProvider(src, dst);
        }
    }
}
