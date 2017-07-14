using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    }
}
