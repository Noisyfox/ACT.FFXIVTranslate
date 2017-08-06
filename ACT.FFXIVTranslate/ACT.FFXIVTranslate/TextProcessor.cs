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

                var sp = MappingSpecialCharacter(chr);
                if (sp != null)
                {
                    builder.Append(sp);
                }
                else
                {
                    builder.Append(chr);
                }

            }
            return builder.ToString();
        }

        public static string RemoveControlCharacters(string input)
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

                builder.Append(chr);
            }
            return builder.ToString();
        }

        public static string MappingSpecialCharacter(char c)
        {
            if (c < '\uE000' || c > '\uE0FF')
            {
                return null;
            }


            switch (c)
            {
                case '\uE03C': // EE 80 BC (HQ)
                    return "[HQ]";
                case '\uE070':
                    return "[?]";
            }

            if (c >= '\uE071' && c <= '\uE08A') // alphabets with square
            {
                return $"[{(char) (c - '\uE071' + 'A')}]";
            }
            if (c >= '\uE08F' && c <= '\uE0AE') // [0] ~ [31]
            {
                return $"[{c - 0xE08F}]";
            }

            switch (c)
            {
                case '\uE0AF':
                    return "[+]";
                case '\uE0B0':
                    return "[E]";
            }
            if (c >= '\uE0B1' && c <= '\uE0B9') // (1) ~ (9)
            {
                return $"({c - 0xE0B1 + 1})";
            }
            switch (c)
            {
                case '\uE0BB': // EE 82 BB (The icon before item name)
                    return null;
            }

            return null;
        }


        public static string BuildQuote(ChattingLine chatting, bool showLabel)
        {
            var cleanedName = NaiveCleanText(chatting.RawSender);

            var eventCode = chatting.RawEventCode;
            var knownCode = Enum.IsDefined(typeof(EventCode), (byte) (eventCode & byte.MaxValue));

            if (knownCode)
            {
                var codeEnum = (EventCode) eventCode;

                if (codeEnum == EventCode.TellFrom)
                {
                    return $"{cleanedName} >> ";
                }
                else if (codeEnum == EventCode.TellTo)
                {
                    return $">>{cleanedName}：";
                }
                else
                {
                    if (showLabel)
                    {
                        switch (codeEnum)
                        {
                            case EventCode.LS1:
                                return $"[1]<{cleanedName}> ";
                            case EventCode.LS2:
                                return $"[2]<{cleanedName}> ";
                            case EventCode.LS3:
                                return $"[3]<{cleanedName}> ";
                            case EventCode.LS4:
                                return $"[4]<{cleanedName}> ";
                            case EventCode.LS5:
                                return $"[5]<{cleanedName}> ";
                            case EventCode.LS6:
                                return $"[6]<{cleanedName}> ";
                            case EventCode.LS7:
                                return $"[7]<{cleanedName}> ";
                            case EventCode.LS8:
                                return $"[8]<{cleanedName}> ";
                            case EventCode.FreeCompany:
                                return $"[FC]<{cleanedName}> ";
                            case EventCode.Party:
                                return $"({cleanedName}) ";
                        }
                    }
                }
            }

            return $"{cleanedName}: ";
        }
    }
}
