using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ACT.FFXIVTranslate
{
    public partial class FFXIVTranslateTabControl : UserControl
    {
        private MainController _controller;
        private Dictionary<CheckBox, EventCode> _channelFilterMapper = new Dictionary<CheckBox, EventCode>();

        public FFXIVTranslateTabControl()
        {
            InitializeComponent();

            _channelFilterMapper[checkBoxChannelFilterSay] = EventCode.Say;
            _channelFilterMapper[checkBoxChannelFilterShout] = EventCode.Shout;
            _channelFilterMapper[checkBoxChannelFilterYell] = EventCode.Yell;
            _channelFilterMapper[checkBoxChannelFilterTell] = EventCode.TellFrom;
            _channelFilterMapper[checkBoxChannelFilterParty] = EventCode.Party;
            _channelFilterMapper[checkBoxChannelFilterAlliance] = EventCode.Alliance;
            _channelFilterMapper[checkBoxChannelFilterFC] = EventCode.FreeCompany;
            _channelFilterMapper[checkBoxChannelFilterNovice] = EventCode.Novice;
            _channelFilterMapper[checkBoxChannelFilterLS1] = EventCode.LS1;
            _channelFilterMapper[checkBoxChannelFilterLS2] = EventCode.LS2;
            _channelFilterMapper[checkBoxChannelFilterLS3] = EventCode.LS3;
            _channelFilterMapper[checkBoxChannelFilterLS4] = EventCode.LS4;
            _channelFilterMapper[checkBoxChannelFilterLS5] = EventCode.LS5;
            _channelFilterMapper[checkBoxChannelFilterLS6] = EventCode.LS6;
            _channelFilterMapper[checkBoxChannelFilterLS7] = EventCode.LS7;
            _channelFilterMapper[checkBoxChannelFilterLS8] = EventCode.LS8;

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
            settings.AddControlSetting(textBoxFont);
            settings.AddControlSetting(checkBoxClickthrough);
            settings.AddStringSetting(nameof(plugin.Language));

            foreach (var cb in _channelFilterMapper.Keys)
            {
                settings.AddControlSetting(cb);
                cb.CheckedChanged += CheckBoxChannelFilterOnCheckedChanged;
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

            trackBarOpacity_ValueChanged(this, EventArgs.Empty);
            NumericUpDownPositionOnValueChanged(this, EventArgs.Empty);
            NumericUpDownSizeOnValueChanged(this, EventArgs.Empty);
            CheckBoxClickthroughOnCheckedChanged(this, EventArgs.Empty);

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
            var eventCode = _channelFilterMapper[cb];
            _controller.NotifyChannelFilterChanged(true, eventCode, cb.Checked);
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

        private void ComboBoxLanguageSelectedIndexChanged(object sender, EventArgs e)
        {
            _controller.NoitfyLanguageChanged(true, (string) comboBoxLanguage.SelectedValue);
        }
    }
}
