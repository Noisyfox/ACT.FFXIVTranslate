using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using ACT.FFXIVTranslate.localization;
using RTF;

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

            for (var i = 0; i < input.Length; i++)
            {
                var chr = input[i];

                switch (chr)
                {
                    case '\u0002':
                        is02 = true;
                        if (i + 3 < input.Length)
                        {
                            int len = input[i + 2];
                            var last = i + 2 + len;
                            if (last < input.Length)
                            {
                                if (input[last] == '\u0003')
                                {
                                    i = last;
                                    is02 = false;
                                    continue;
                                }
                                i = last - 1;
                                continue;
                            }
                        }
                        i += 2;
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
                case '\uE06F': // EE 81 AF
                    return "🡆"; // 🡆
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
                    return "\u2326"; // ⌦
            }

            return null;
        }

        private static readonly string WorldSp = Encoding.UTF8.GetString(new byte []{ 0x03, 0x02, 0x12, 0x02, 0x59, 0x03 });

        public static Tuple<string, string> ExtractName(string rawSender)
        {
            var i = rawSender.IndexOf(WorldSp, StringComparison.Ordinal);
            if (i == -1)
            {
                return new Tuple<string, string>(NaiveCleanText(rawSender), null);
            }
            return new Tuple<string, string>(NaiveCleanText(rawSender.Substring(0, i)),
                NaiveCleanText(rawSender.Substring(i + 1)));
        }


        public static string BuildQuote(ChattingLine chatting, bool showLabel)
        {
            var extractedName = ExtractName(chatting.RawSender);

            var eventCode = chatting.RawEventCode;
            var knownCode = Enum.IsDefined(typeof(EventCode), (byte) (eventCode & byte.MaxValue));

            if (knownCode)
            {
                var codeEnum = (EventCode) eventCode;

                if (codeEnum == EventCode.TellFrom)
                {
                    return $"{extractedName.Item1} >> ";
                }
                else if (codeEnum == EventCode.TellTo)
                {
                    return $">>{extractedName.Item1}：";
                }
                else
                {
                    if (showLabel)
                    {
                        var nameWithWorld = extractedName.Item2 == null
                            ? extractedName.Item1
                            : $"{extractedName.Item1}@{extractedName.Item2}";
                        switch (codeEnum)
                        {
                            case EventCode.LS1:
                                return $"[1]<{nameWithWorld}> ";
                            case EventCode.LS2:
                                return $"[2]<{nameWithWorld}> ";
                            case EventCode.LS3:
                                return $"[3]<{nameWithWorld}> ";
                            case EventCode.LS4:
                                return $"[4]<{nameWithWorld}> ";
                            case EventCode.LS5:
                                return $"[5]<{nameWithWorld}> ";
                            case EventCode.LS6:
                                return $"[6]<{nameWithWorld}> ";
                            case EventCode.LS7:
                                return $"[7]<{nameWithWorld}> ";
                            case EventCode.LS8:
                                return $"[8]<{nameWithWorld}> ";
                            case EventCode.CWLS1:
                                return $"[{GetCWLSLabel(1)}]<{nameWithWorld}> ";
                            case EventCode.CWLS2:
                                return $"[{GetCWLSLabel(2)}]<{nameWithWorld}> ";
                            case EventCode.CWLS3:
                                return $"[{GetCWLSLabel(3)}]<{nameWithWorld}> ";
                            case EventCode.CWLS4:
                                return $"[{GetCWLSLabel(4)}]<{nameWithWorld}> ";
                            case EventCode.CWLS5:
                                return $"[{GetCWLSLabel(5)}]<{nameWithWorld}> ";
                            case EventCode.CWLS6:
                                return $"[{GetCWLSLabel(6)}]<{nameWithWorld}> ";
                            case EventCode.CWLS7:
                                return $"[{GetCWLSLabel(7)}]<{nameWithWorld}> ";
                            case EventCode.CWLS8:
                                return $"[{GetCWLSLabel(8)}]<{nameWithWorld}> ";
                            case EventCode.FreeCompany:
                                return $"[FC]<{nameWithWorld}> ";
                            case EventCode.Party:
                                return $"({nameWithWorld}) ";
                            case EventCode.Alliance:
                                return $"(({nameWithWorld})) ";
                        }
                    }
                }
            }

            return $"{extractedName.Item1}: ";
        }

        private static string GetCWLSLabel(int n)
        {
            return string.Format(strings.channelLabelCWLS, n);
        }

        public static string BuildRTF(
            FFXIVTranslatePlugin plugin,
            bool addTimestamp,
            bool timestamp24Hour,
            IEnumerable<ChattingLine> chattingLines)
        {
            var finalResultBuilder = new RTFBuilder();
            foreach (var line in chattingLines)
            {
                if (addTimestamp)
                {
                    finalResultBuilder.ForeColor(Color.White);
                    string formattedTime;
                    if (timestamp24Hour)
                    {
                        formattedTime = $"[{line.Timestamp:HH:mm}]";
                    }
                    else
                    {
                        formattedTime =
                            string.Format("[{0}]",
                                line.Timestamp.ToString(
                                    line.Timestamp.Hour > 11
                                        ? strings.timeFormat12HourPM
                                        : strings.timeFormat12HourAM,
                                    strings.Culture));
                    }
                    finalResultBuilder.Append(formattedTime);
                }
                var settings = plugin.GetChannelSettings(line.EventCode);
                finalResultBuilder.ForeColor(settings.DisplayColor);
                finalResultBuilder.AppendLine(
                    $"{TextProcessor.BuildQuote(line, settings.ShowLabel)}{line.TranslatedContent}");
            }

            return finalResultBuilder.ToString();
        }
    }
}
