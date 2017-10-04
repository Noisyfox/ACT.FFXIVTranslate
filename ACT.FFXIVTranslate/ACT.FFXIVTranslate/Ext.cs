using System;
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

        public static T Clamp<T>(this T val, T min, T max) where T : IComparable<T>
        {
            if (val.CompareTo(min) < 0) return min;
            else if (val.CompareTo(max) > 0) return max;
            else return val;
        }
    }
}
