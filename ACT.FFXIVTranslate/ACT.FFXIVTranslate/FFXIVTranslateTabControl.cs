using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Advanced_Combat_Tracker;

namespace ACT.FFXIVTranslate
{
    public partial class FFXIVTranslateTabControl : UserControl
    {
        private MainController _controller;
        private readonly List<ChannelSettingsMapper> _channelSettings;
        private Font _currentFont;

        private class ChannelSettingsMapper
        {
            public EventCode Code { get; }
            public CheckBox CheckBoxFilter { get; }
            public Button ButtonColor { get; }
            public CheckBox CheckBoxLabel { get; }

            public ChannelSettingsMapper(EventCode code, CheckBox cbFilter, Button btnColor, CheckBox cbLabel)
            {
                Code = code;
                CheckBoxFilter = cbFilter;
                ButtonColor = btnColor;
                CheckBoxLabel = cbLabel;
            }

            public ChannelSettingsMapper(EventCode code, ControlCollection panel)
            {
                Code = code;

                var name = code.ToString();
                switch (name)
                {
                    case "FreeCompany":
                        name = "FC";
                        break;
                    case "TellFrom":
                        name = "Tell";
                        break;
                }
                CheckBoxFilter = (CheckBox) panel.Find($"checkBoxChannelFilter{name}", true)[0];
                ButtonColor = (Button)panel.Find($"buttonChannelColor{name}", true)[0];
                CheckBoxLabel = (CheckBox)panel.Find($"checkBoxChannelLabel{name}", true)[0];
            }
        }

        public FFXIVTranslateTabControl()
        {
            InitializeComponent();

            _channelSettings = Enum.GetValues(typeof(EventCode)).OfType<EventCode>().Where(it => it != EventCode.TellTo)
                .Select(it => new ChannelSettingsMapper(it, Controls)).ToList();

            comboBoxLanguage.DisplayMember = "DisplayName";
            comboBoxLanguage.ValueMember = "LangCode";
            comboBoxLanguage.DataSource = localization.Localization.SupportedLanguages;
        }

        public void AttachToAct(FFXIVTranslatePlugin plugin)
        {

            var parentTabPage = plugin.ParentTabPage;

            parentTabPage.Controls.Add(this);
            parentTabPage.Resize += ParentTabPageOnResize;
            ParentTabPageOnResize(parentTabPage, null);

            var settings = plugin.Settings;
            // add settings
            settings.AddControlSetting(numericUpDownX);
            settings.AddControlSetting(numericUpDownY);
            settings.AddControlSetting(numericUpDownWidth);
            settings.AddControlSetting(numericUpDownHeight);
            settings.AddControlSetting(trackBarOpacity);
            settings.AddControlSetting(checkBoxClickthrough);
            settings.AddStringSetting(nameof(plugin.Language));
            settings.AddStringSetting(nameof(plugin.OverlayFont));

            foreach (var cs in _channelSettings)
            {
                settings.AddControlSetting(cs.CheckBoxFilter);
                cs.CheckBoxFilter.CheckedChanged += CheckBoxChannelFilterOnCheckedChanged;

                settings.AddControlSetting(cs.CheckBoxLabel);
                cs.CheckBoxLabel.CheckedChanged += CheckBoxChannelLabelOnCheckedChanged;

                settings.AddControlSetting(cs.ButtonColor);
                cs.ButtonColor.Click += ButtonChannelColorClick;
                cs.ButtonColor.TextChanged += ButtonColorOnTextChanged;
            }

            _controller = plugin.Controller;

            numericUpDownX.ValueChanged+= NumericUpDownPositionOnValueChanged;
            numericUpDownY.ValueChanged += NumericUpDownPositionOnValueChanged;
            numericUpDownWidth.ValueChanged += NumericUpDownSizeOnValueChanged;
            numericUpDownHeight.ValueChanged += NumericUpDownSizeOnValueChanged;
            checkBoxClickthrough.CheckedChanged += CheckBoxClickthroughOnCheckedChanged;
            comboBoxLanguage.SelectedIndexChanged += ComboBoxLanguageSelectedIndexChanged;

            _controller.OverlayMoved += ControllerOnOverlayMoved;
            _controller.OverlayResized += ControllerOnOverlayResized;
            _controller.LanguageChanged += ControllerOnLanguageChanged;
            _controller.LogMessageAppend += ControllerOnLogMessageAppend;
            _controller.OverlayFontChanged += ControllerOnOverlayFontChanged;
            _controller.ChannelColorChanged += ControllerOnChannelColorChanged;

            trackBarOpacity_ValueChanged(this, EventArgs.Empty);
            NumericUpDownPositionOnValueChanged(this, EventArgs.Empty);
            NumericUpDownSizeOnValueChanged(this, EventArgs.Empty);
            CheckBoxClickthroughOnCheckedChanged(this, EventArgs.Empty);
            foreach (var cs in _channelSettings)
            {
                cs.ButtonColor.Text = "#FFFFFF";
            }

            translateProviderPanel.AttachToAct(plugin);
        }

        public void DoLocalization()
        {
            localization.Localization.TranslateControls(this);
            translateProviderPanel.DoLocalization();
        }

        private void CheckBoxChannelFilterOnCheckedChanged(object sender, EventArgs eventArgs)
        {
            var cb = (CheckBox)sender;
            var eventCode = _channelSettings.Find(it => it.CheckBoxFilter == sender).Code;
            _controller.NotifyChannelFilterChanged(true, eventCode, cb.Checked);
        }

        private void CheckBoxChannelLabelOnCheckedChanged(object sender, EventArgs eventArgs)
        {
            var cb = (CheckBox)sender;
            var eventCode = _channelSettings.Find(it => it.CheckBoxLabel == sender).Code;
            _controller.NotifyChannelLabelChanged(true, eventCode, cb.Checked);
        }

        private void ButtonChannelColorClick(object sender, EventArgs e)
        {
            var btn = (Button) sender;
            var eventCode = _channelSettings.Find(it => it.ButtonColor == sender).Code;
            var colorDialog = new ColorDialog();
            colorDialog.Color = btn.BackColor;
            if (colorDialog.ShowDialog() != DialogResult.Cancel)
            {
                _controller.NotifyChannelColorChanged(true, eventCode, colorDialog.Color);
            }
        }

        private void ButtonColorOnTextChanged(object sender, EventArgs eventArgs)
        {
            var btn = (Button)sender;
            var eventCode = _channelSettings.Find(it => it.ButtonColor == sender).Code;
            var color = ColorTranslator.FromHtml(btn.Text);

            _controller.NotifyChannelColorChanged(true, eventCode, color);
        }

        private void ParentTabPageOnResize(object sender, EventArgs eventArgs)
        {
            Location = new Point(0, 0);
            Size = ((TabPage)sender).Size;
        }

        private void trackBarOpacity_ValueChanged(object sender, EventArgs e)
        {
            labelOpacityValue.Text = $"{trackBarOpacity.Value}%";
            _controller.NotifyOpacityChanged(false, trackBarOpacity.Value / 100D);
        }

        private void buttonFont_Click(object sender, EventArgs e)
        {
            var fontdialog = new FontDialog();
            fontdialog.Font = _currentFont;

            if (fontdialog.ShowDialog() != DialogResult.Cancel)
            {
                _controller.NotifyOverlayFontChanged(true, fontdialog.Font);
            }
        }

        private void NumericUpDownPositionOnValueChanged(object sender, EventArgs eventArgs)
        {
            _controller.NotifyOverlayMoved(false, (int) numericUpDownX.Value, (int) numericUpDownY.Value);
        }

        private void NumericUpDownSizeOnValueChanged(object sender, EventArgs eventArgs)
        {
            _controller.NotifyOverlayResized(false, (int)numericUpDownWidth.Value, (int)numericUpDownHeight.Value);
        }

        private void CheckBoxClickthroughOnCheckedChanged(object sender, EventArgs eventArgs)
        {
            _controller.NotifyClickthroughChanged(false, checkBoxClickthrough.Checked);
        }

        private void ComboBoxLanguageSelectedIndexChanged(object sender, EventArgs e)
        {
            _controller.NoitfyLanguageChanged(true, (string)comboBoxLanguage.SelectedValue);
        }

        private void ControllerOnOverlayMoved(bool fromView, int x, int y)
        {
            if (!fromView)
            {
                return;
            }
            numericUpDownX.ValueChanged -= NumericUpDownPositionOnValueChanged;
            numericUpDownY.ValueChanged -= NumericUpDownPositionOnValueChanged;

            numericUpDownX.Value = x;
            numericUpDownY.Value = y;

            numericUpDownX.ValueChanged += NumericUpDownPositionOnValueChanged;
            numericUpDownY.ValueChanged += NumericUpDownPositionOnValueChanged;
        }

        private void ControllerOnOverlayResized(bool fromView, int w, int h)
        {
            if (!fromView)
            {
                return;
            }

            numericUpDownWidth.ValueChanged -= NumericUpDownSizeOnValueChanged;
            numericUpDownHeight.ValueChanged -= NumericUpDownSizeOnValueChanged;

            numericUpDownWidth.Value = w;
            numericUpDownHeight.Value = h;

            numericUpDownWidth.ValueChanged += NumericUpDownSizeOnValueChanged;
            numericUpDownHeight.ValueChanged += NumericUpDownSizeOnValueChanged;
        }

        private void ControllerOnLanguageChanged(bool fromView, string lang)
        {
            if (fromView)
            {
                return;
            }
            var ld = localization.Localization.GetLanguage(lang);
            _controller.NoitfyLanguageChanged(true, ld.LangCode);
            comboBoxLanguage.SelectedValue = ld.LangCode;
        }

        private void ControllerOnLogMessageAppend(bool fromView, string log)
        {
            ThreadInvokes.RichTextBoxAppendDateTimeLine(ActGlobals.oFormActMain, richTextBoxLog, log);
        }

        private void ControllerOnOverlayFontChanged(bool fromView, Font font)
        {
            _currentFont = font;
            textBoxFont.Text = TypeDescriptor.GetConverter(typeof(Font)).ConvertToString(font);
        }

        private void ControllerOnChannelColorChanged(bool fromView, EventCode code, Color color)
        {
            var btn = _channelSettings.Find(it => it.Code == code).ButtonColor;
            btn.BackColor = color;
            btn.Text = color.ToHexString();
        }
    }
}
