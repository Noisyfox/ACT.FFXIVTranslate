using System.Linq;
using ACT.FFXIVTranslate.localization;
using ACT.FFXIVTranslate.translate.google_unofficial;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ACT.FFXIVTranslate.Test
{
    [TestClass]
    public class UnofficialGoogleTranslateTest
    {
        [TestMethod]
        public void TestUnofficialGoogleTranslate()
        {
            var factory = new GoogleTranslateProviderFactory();
            var to = LanguageDef.BuildLangFromCulture("zh");
            var provider = factory.CreateProvider(null, null, to);

            var lines = new[]
            {
                new ChattingLine{RawContent = "This is a testing string."},
                new ChattingLine{RawContent = "This is another testing string."},
                new ChattingLine{RawContent = "Wow lots of strings!"},
                new ChattingLine{RawContent = "I like the game called 'Final Fantasy XIV'!"},
                new ChattingLine{RawContent = "This is a string <aaa> contains html tag."},
                new ChattingLine{RawContent = "This is a string &lt; contains html tag."},
                new ChattingLine{RawContent = "&lt;"},
            }.ToList();

            provider.Translate(lines);
        }
    }
}
