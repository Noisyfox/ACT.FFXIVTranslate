using System.Linq;
using ACT.FFXIVTranslate.translate.bing;
using ACT.FoxCommon.localization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ACT.FFXIVTranslate.Test
{
    [TestClass]
    public class BingTranslateTest
    {
        [TestMethod]
        public void TestBingTranslate()
        {
            var factory = new BingTranslateProviderFactory();
            var key = "90ebd9b5e7544500a5c1cf3ff7996314";
            var to = LanguageDef.BuildLangFromCulture("zh-CHS");
            var provider = factory.CreateProvider(key, null, to);

            var lines = new[]
            {
                new ChattingLine{RawContent = "This is a testing string."},
                new ChattingLine{RawContent = "This is another testing string."},
                new ChattingLine{RawContent = "Wow lots of strings!"},
                new ChattingLine{RawContent = "I like the game called 'Final Fantasy XIV'!"},
                new ChattingLine{RawContent = "This is a string <aaa> contains html tag."},
                new ChattingLine{RawContent = "This is a string &lt; contains html tag."},
                new ChattingLine{RawContent = "&lt;"},
            }.Select(it =>
            {
                provider.PreprocessLine(it);
                return it;
            }).ToList();

            provider.Translate(lines);
        }
    }
}
