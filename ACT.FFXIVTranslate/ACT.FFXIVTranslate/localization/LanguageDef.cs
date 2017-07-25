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
    }
}
