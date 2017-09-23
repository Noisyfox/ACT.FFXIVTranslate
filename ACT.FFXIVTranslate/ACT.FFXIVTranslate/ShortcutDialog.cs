using System;
using System.Windows.Forms;
using ACT.FFXIVTranslate.localization;

namespace ACT.FFXIVTranslate
{
    public partial class ShortcutDialog : Form
    {
        public Keys CurrentKey { get; set; } = Keys.None;

        public ShortcutDialog()
        {
            InitializeComponent();
            Text = strings.actPanelTitle;
            Localization.TranslateControls(this);
        }

        private void ShortcutDialog_Load(object sender, EventArgs e)
        {
            FixKey();
            ShowKey();
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            FixKey();
            DialogResult = DialogResult.OK;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            FixKey();
            DialogResult = DialogResult.Cancel;
        }

        private void buttonNone_Click(object sender, EventArgs e)
        {
            CurrentKey = Keys.None;
            DialogResult = DialogResult.OK;
        }

        private void ShortcutDialog_KeyDown(object sender, KeyEventArgs e)
        {
            //Combination key only
            if (e.Modifiers != 0)
            {
                CurrentKey = e.KeyData;
                if (!HasValueKey(CurrentKey))
                {
                    CurrentKey &= Keys.Modifiers;
                }
            }
            else
            {
                CurrentKey = Keys.None;
            }
            ShowKey();
        }

        private void ShortcutDialog_KeyUp(object sender, KeyEventArgs e)
        {
            FixKey();
            ShowKey();
        }

        private void FixKey()
        {
            if (!HasValueKey(CurrentKey))
            {
                CurrentKey = Keys.None;
            }
        }

        private void ShowKey()
        {
            if (CurrentKey == Keys.None)
            {
                textBoxShortcut.Text = strings.messageWaitKey;
            }
            else
            {
                textBoxShortcut.Text = ShortkeyManager.KeyToString(CurrentKey);
            }
        }

        private bool HasValueKey(Keys keyvalue)
        {
            keyvalue = keyvalue & Keys.KeyCode;
            if ((keyvalue >= Keys.PageUp && keyvalue <= Keys.Down) ||
                (keyvalue >= Keys.A && keyvalue <= Keys.Z) ||
                (keyvalue >= Keys.F1 && keyvalue <= Keys.F12))
            {
                return true;
            }
            else if (keyvalue >= Keys.D0 && keyvalue <= Keys.D9)
            {
                return true;
            }
            else if (keyvalue >= Keys.NumPad0 && keyvalue <= Keys.NumPad9)
            {
                return true;
            }

            return false;
        }
    }
}
