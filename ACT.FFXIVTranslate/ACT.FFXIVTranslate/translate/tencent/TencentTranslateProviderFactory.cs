using System.Collections.Generic;
using System.Linq;
using ACT.FoxCommon.localization;

namespace ACT.FFXIVTranslate.translate.tencent
{
    internal class TencentTranslateProviderFactory : ITranslateProviderFactory
    {
        private readonly List<LanguageDef> _allSupportedLanguages = new[]
        {
            LanguageDef.BuildLangFromCulture("zh-CN", "zh"),
            LanguageDef.BuildLangFromCulture("zh-TW"),
            LanguageDef.BuildLangFromCulture("en"),
            LanguageDef.BuildLangFromCulture("ja", "jp"),
            LanguageDef.BuildLangFromCulture("de"),
            LanguageDef.BuildLangFromCulture("fr"),
            LanguageDef.BuildLangFromCulture("ru"),
            LanguageDef.BuildLangFromCulture("ko", "kr"),
        }.ToList();

        public string ProviderId => "Tencent Translate";
        public string ProviderName => LocalizationBase.GetString("translateProviderNameTencent", ProviderId);
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
            return new TencentTranslateProvider(apiKey, src, dst);
        }
    }
}
