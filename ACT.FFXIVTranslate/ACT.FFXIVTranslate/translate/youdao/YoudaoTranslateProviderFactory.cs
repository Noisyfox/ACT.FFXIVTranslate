using System.Collections.Generic;
using System.Linq;
using ACT.FoxCommon.localization;

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

        public string ProviderId => "有道翻译";
        public string ProviderName => LocalizationBase.GetString("translateProviderNameYoudao", ProviderId);
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
            return new YoudaoTranslateProvider(apiKey, src, dst);
        }
    }
}
