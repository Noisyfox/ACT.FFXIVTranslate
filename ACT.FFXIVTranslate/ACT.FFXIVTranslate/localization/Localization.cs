using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ACT.FFXIVTranslate.localization
{
    public static class Localization
    {
        public static readonly LanguageDef[] SupportedLanguages = {
            BuildLangFromCulture("zh-CN"),
            BuildLangFromCulture("en-US"),
        };

        private const string DefaultLanguage = "zh-CN";

        private static LanguageDef BuildLangFromCulture(string name)
        {
            var ci = CultureInfo.GetCultureInfo(name);
            var eName = ci.EnglishName;
            var nName = ci.NativeName;
            if (nName == eName)
            {
                nName = string.Empty;
            }
            return new LanguageDef(name, eName, nName);
        }

        public static LanguageDef GetLanguage(string code)
        {
            return SupportedLanguages.FirstOrDefault(it => it.LangCode == code) ?? GetLanguage(DefaultLanguage);
        }

        public static void ConfigLocalization(string code)
        {
            strings.Culture = CultureInfo.GetCultureInfo(code);
        }

        private static void UpdateTextBasedOnName(Control control)
        {
            var t = strings.ResourceManager.GetString(control.Name, strings.Culture);
            if (t != null)
            {
                control.Text = t;
            }
        }

        public static void TranslateControls(Control control)
        {
            var setterList = new List<Action>();

            setterList.Add(()=>UpdateTextBasedOnName(control));

            foreach (Control child in control.Controls.AsParallel())
            {
                TranslateControls(child);
            }

            foreach (var action in setterList.AsParallel())
            {
                if (control.InvokeRequired)
                {
                    control.Invoke((MethodInvoker)delegate { action(); });
                }
                else
                {
                    action();
                }
            }
        }
    }
}
