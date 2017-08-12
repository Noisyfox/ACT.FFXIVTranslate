using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
    }
}
