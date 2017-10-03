using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Media;
using FontFamily = System.Windows.Media.FontFamily;

namespace ACT.FFXIVTranslate
{
    internal static class Utils
    {
        public static long TimestampMillisFromDateTime(DateTime date)
        {
            var unixTimestamp = date.Ticks - new DateTime(1970, 1, 1).Ticks;
            unixTimestamp /= TimeSpan.TicksPerMillisecond;
            return unixTimestamp;
        }

        public static bool IsGameExePath(string path)
        {
            var exe = Path.GetFileName(path);
            return exe == "ffxiv.exe" || exe == "ffxiv_dx11.exe";
        }

        public static bool IsActExePath(string path)
        {
            return path == Process.GetCurrentProcess().MainModule.FileName;
        }

        public static Typeface NewTypeFaceFromFont(Font f)
        {
            var ff = new FontFamily(f.Name);

            var typeface = new Typeface(ff,
                f.Style.HasFlag(System.Drawing.FontStyle.Italic) ? FontStyles.Italic : FontStyles.Normal,
                f.Style.HasFlag(System.Drawing.FontStyle.Bold) ? FontWeights.Bold : FontWeights.Normal,
                FontStretches.Normal);

            return typeface;
        }
    }
}
