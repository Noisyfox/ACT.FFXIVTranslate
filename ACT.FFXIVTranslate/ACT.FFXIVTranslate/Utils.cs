﻿using System;
using System.Diagnostics;
using System.IO;

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
    }
}
