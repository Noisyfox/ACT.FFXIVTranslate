using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Advanced_Combat_Tracker;

namespace ACT.FFXIVTranslate
{
    public class FFXIVTranslatePlugin : IActPluginV1
    {
        private readonly string _settingsFile = Path.Combine(ActGlobals.oFormActMain.AppDataFolder.FullName, "Config\\ACT.FFXIVTranslate.config.xml");

        private TabPage parentTabPage;
        private Label statusLabel;

        public void InitPlugin(TabPage pluginScreenSpace, Label pluginStatusText)
        {
            parentTabPage = pluginScreenSpace;
            statusLabel = pluginStatusText;

            // Load FFXIV plugin's assembly if needed
            AppDomain.CurrentDomain.AssemblyResolve += (o, e) =>
            {
                var simpleName = new string(e.Name.TakeWhile(x => x != ',').ToArray());
                if (simpleName == "FFXIV_ACT_Plugin")
                {
                    var query = ActGlobals.oFormActMain.ActPlugins.Where(x => x.lblPluginTitle.Text == "FFXIV_ACT_Plugin.dll");
                    var plugin = query.FirstOrDefault();

                    if (plugin != null)
                    {
                        return System.Reflection.Assembly.LoadFrom(plugin.pluginFile.FullName);
                    }
                }

                return null;
            };

            parentTabPage.Text = "Talk Translate";

            var mainControl = new FFXIVTranslateTabControl();
            mainControl.AttachToACT(parentTabPage);
        }

        public void DeInitPlugin()
        {
        }
    }
}
