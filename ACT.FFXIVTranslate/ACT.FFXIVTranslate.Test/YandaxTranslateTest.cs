using System;
using System.Linq;
using ACT.FFXIVTranslate.localization;
using ACT.FFXIVTranslate.translate.yandax;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ACT.FFXIVTranslate.Test
{
    [TestClass]
    public class YandaxTranslateTest
    {
        [TestMethod]
        public void TestYandaxTranslate()
        {
            var factory = new YandaxTranslateProviderFactory();
            var key = factory.DefaultPublicKey;
            var to = LanguageDef.BuildLangFromCulture("zh");
            var provider = factory.CreateProvider(key, null, to);

            var lines = new[]
            {
                new ChattingLine {RawContent = "This is a testing string."},
                new ChattingLine {RawContent = "This is another testing string."},
                new ChattingLine {RawContent = "Wow lots of strings!"},
                new ChattingLine {RawContent = "I like the game called 'Final Fantasy XIV'!"},
                new ChattingLine {RawContent = "This is a string <aaa> contains html tag."},
                new ChattingLine {RawContent = "This is a string &lt; contains html tag."},
                new ChattingLine {RawContent = "&lt;"},
            }.Select(it =>
            {
                provider.PreprocessLine(it);
                return it;
            }).ToList();

            provider.Translate(lines);
        }
    }
}
