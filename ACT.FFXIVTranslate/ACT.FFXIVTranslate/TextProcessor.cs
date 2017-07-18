using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ACT.FFXIVTranslate
{
    public class TextProcessor
    {
        /// <summary>
        /// Remove all control content in chatting text.
        /// </summary>
        public static string NaiveCleanText(string input)
        {
            var builder = new StringBuilder();
            var is02 = false;

            foreach (var chr in input)
            {
                switch (chr)
                {
                    case '\u0002':
                        is02 = true;
                        continue;
                    case '\u0003':
                        is02 = false;
                        continue;
                    case '\uFFFD': // EF BF BD
                        is02 = true;
                        continue;
                }

                if (is02)
                {
                    continue;
                }

                switch (chr)
                {
                    case '\uE03C': // EE 80 BC (HQ)
                        builder.Append("[HQ]");
                        break;
                    case '\uE0BB': // EE 82 BB (The icon before item name)
                        break;
                    default:
                        builder.Append(chr);
                        break;
                }

            }
            return builder.ToString();
        }
    }
}
