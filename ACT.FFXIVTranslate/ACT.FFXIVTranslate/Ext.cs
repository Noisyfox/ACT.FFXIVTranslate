using System;
using System.Collections.Generic;
using System.Drawing;
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

        public static string ToHexString(this Color c)
        {
            return "#" + c.R.ToString("X2") + c.G.ToString("X2") + c.B.ToString("X2");
        }
    }

    public static class ThreadInvokesExt
    {
        private delegate void RichTextBoxAppendRtfCallback(Form parent, RichTextBox target, string rtf);

        public static void RichTextBoxAppendRtf(Form parent, RichTextBox target, string rtf)
        {
            if (target.InvokeRequired)
            {
                RichTextBoxAppendRtfCallback appendTextCallback = RichTextBoxAppendRtf;
                parent.Invoke(appendTextCallback, parent, target, rtf);
            }
            else
            {
                target.SelectionStart = target.TextLength;
                target.SelectionLength = 0;

                target.SelectedRtf = rtf;
//                target.SelectionFont = font;
//                target.AppendText(text);
//                target.SelectionColor = target.ForeColor;
                target.ScrollToCaret();
            }
        }
    }
}
