﻿using System.Collections.Generic;
using System.Linq;
using ACT.FoxCommon.localization;

namespace ACT.FFXIVTranslate.translate.baidu
{
    internal class BaiduTranslateProviderFactory : ITranslateProviderFactory
    {
        private readonly List<LanguageDef> _allSupportedLanguages = new[]
        {
            LanguageDef.BuildLangFromCulture("zh"),
            LanguageDef.BuildLangFromCulture("en"),
            LanguageDef.BuildLangFromCulture("ja", "jp"),
            LanguageDef.BuildLangFromCulture("de"),
            LanguageDef.BuildLangFromCulture("fr", "fra"),
            LanguageDef.BuildLangFromCulture("ru"),
            LanguageDef.BuildLangFromCulture("ko", "kor"),
        }.ToList();

        public string ProviderId => "百度翻译";
        public string ProviderName => LocalizationBase.GetString("translateProviderNameBaidu", ProviderId);

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
            return new BaiduTranslateProvider(apiKey, src, dst);
        }
    }
}
