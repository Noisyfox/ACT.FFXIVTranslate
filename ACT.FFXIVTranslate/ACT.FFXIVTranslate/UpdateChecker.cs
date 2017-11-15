using System.Text.RegularExpressions;
using ACT.FoxCommon.update;

namespace ACT.FFXIVTranslate
{
    internal class UpdateChecker : UpdateCheckerBase<MainController, FFXIVTranslatePlugin>
    {
        public const string ReleasePage = "https://github.com/Noisyfox/ACT.FFXIVTranslate/releases";


        protected override string UpdateUrl { get; } = "https://api.github.com/repos/Noisyfox/ACT.FFXIVTranslate/releases";

        private const string NameMatcher =
            @"^ACT\.FFXIVTranslate(?:-|\.)(?<version>\d+(?:\.\d+)*)(?:|-Release)\.7z$";

        protected override string ParseVersion(string fileName)
        {
            var match = Regex.Match(fileName, NameMatcher);
            if (match.Success)
            {
                return match.Groups["version"].Value;
            }

            return null;
        }
    }
}
