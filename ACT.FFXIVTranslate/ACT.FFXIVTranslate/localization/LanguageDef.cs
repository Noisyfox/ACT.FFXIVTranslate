using System.Globalization;

namespace ACT.FFXIVTranslate.localization
{

    public class LanguageDef
    {
        public const string CodeAuto = "auto";

        public string LangCode { get; }

        public string EnglishName { get; }

        public string LocalizedName { get; }

        public string DisplayName
        {
            get
            {
                if (string.IsNullOrEmpty(LocalizedName))
                {
                    return EnglishName;
                }

                return $"{LocalizedName}({EnglishName})";
            }
        }

        internal LanguageDef(string code, string eName, string lName)
        {
            LangCode = code;
            EnglishName = eName;
            LocalizedName = lName;
        }

        public static LanguageDef BuildLangFromCulture(string name)
        {
            return BuildLangFromCulture(name, name);
        }

        public static LanguageDef BuildLangFromCulture(string name, string code)
        {
            var ci = CultureInfo.GetCultureInfo(name);
            var eName = ci.EnglishName;
            var nName = ci.NativeName;
            if (nName == eName)
            {
                nName = string.Empty;
            }
            return new LanguageDef(code, eName, nName);
        }
    }


}
