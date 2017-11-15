using System.Linq;
using ACT.FFXIVTranslate.translate.baidu;
using ACT.FoxCommon.localization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ACT.FFXIVTranslate.Test
{
    [TestClass]
    public class BaiduTranslateTest
    {
        [TestMethod]
        public void TestBaiduTranslate()
        {
            var factory = new BaiduTranslateProviderFactory();
            var to = LanguageDef.BuildLangFromCulture("zh");
            var appId = "YOUR-APP-ID";
            var secret = "YOUR-APP-SECRET";
            var provider = factory.CreateProvider($"{appId}:{secret}", null, to);

            var lines = new[]
            {
                new ChattingLine{RawContent = "This is a testing string."},
                new ChattingLine{RawContent = "This is another testing string."},
                new ChattingLine{RawContent = "Wow lots of strings!"},
                new ChattingLine{RawContent = "I like the game called 'Final Fantasy XIV'!"},
                new ChattingLine{RawContent = "This is a string <aaa> contains html tag."},
                new ChattingLine{RawContent = "This is a string &lt; contains html tag."},
                new ChattingLine{RawContent = "&lt;"},
                new ChattingLine{RawContent = "你在逗我呢？"},
            }.Select(it =>
            {
                provider.PreprocessLine(it);
                return it;
            }).ToList();

            provider.Translate(lines);
        }
    }
}
