using System.Drawing;
using System.Windows.Forms;
using Advanced_Combat_Tracker;

namespace ACT.FFXIVTranslate
{
    public static class Ext
    {
        public static void AddControlSetting(this SettingsSerializer serializer, Control controlToSerialize)
        {
            serializer.AddControlSetting(controlToSerialize.Name, controlToSerialize);
        }

        public static string ToHexString(this Color c)
        {
            return "#" + c.R.ToString("X2") + c.G.ToString("X2") + c.B.ToString("X2");
        }
    }
}
