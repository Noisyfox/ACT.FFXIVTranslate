using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ACT.FFXIVTranslate.localization;

namespace ACT.FFXIVTranslate.translate
{
    public partial class TranslateProviderPanel : UserControl
    {
        private FFXIVTranslatePlugin _plugin;
        private TranslateService _service;

        public TranslateProviderPanel()
        {
            InitializeComponent();
            comboBoxProvider.DisplayMember = "ProviderName";
            comboBoxProvider.ValueMember = "ProviderName";
            comboBoxLangFrom.DisplayMember = "DisplayName";
            comboBoxLangFrom.ValueMember = "LangCode";
            comboBoxLangTo.DisplayMember = "DisplayName";
            comboBoxLangTo.ValueMember = "LangCode";
        }

        internal void AttachToAct(FFXIVTranslatePlugin plugin)
        {
            _service = plugin.TranslateService;
            _plugin = plugin;

            var settings = plugin.Settings;
            settings.AddStringSetting(nameof(plugin.TranslateProvider));
            settings.AddStringSetting(nameof(plugin.TranslateApiKey));
            settings.AddStringSetting(nameof(plugin.TranslateLangFrom));
            settings.AddStringSetting(nameof(plugin.TranslateLangTo));

            var controller = plugin.Controller;
            controller.TranslateProviderChanged += ControllerOnTranslateProviderChanged;
        }
        public void DoLocalization()
        {
            comboBoxProvider.DataSource = _service.AllProviders;
        }

        private void comboBoxProvider_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedProvider = (ITranslaterProviderFactory) comboBoxProvider.SelectedItem;

            if (selectedProvider != null)
            {
                // Update language selector
                comboBoxLangFrom.DataSource = selectedProvider.SupportAutoDetect
                    ? new[] { new LanguageDef(LanguageDef.CodeAuto, strings.LangAutoDetect, string.Empty) }.Concat(selectedProvider.SupportedSrcLanguages).ToList()
                    : selectedProvider.SupportedSrcLanguages;
                comboBoxLangTo.DataSource = selectedProvider.SupportedDestLanguages;
            }
        }

        private void ControllerOnTranslateProviderChanged(bool fromView, string provider, string apiKey, string langFrom, string langTo)
        {
            if (fromView)
            {
                return;
            }

            if (string.IsNullOrEmpty(provider))
            {
                comboBoxProvider.SelectedIndex = 0;
            }
            else
            {
                comboBoxProvider.SelectedValue = provider;
                if (comboBoxProvider.SelectedIndex == -1)
                {
                    comboBoxProvider.SelectedIndex = 0;
                }
            }

            if (!string.IsNullOrEmpty(apiKey))
            {
                textBoxApiKey.Text = apiKey;
            }

            if (string.IsNullOrEmpty(langFrom))
            {
                comboBoxLangFrom.SelectedIndex = 0;
            }
            else
            {
                comboBoxLangFrom.SelectedValue = langFrom;
                if (comboBoxLangFrom.SelectedIndex == -1)
                {
                    comboBoxLangFrom.SelectedIndex = 0;
                }
            }

            if (string.IsNullOrEmpty(langTo))
            {
                comboBoxLangTo.SelectedIndex = 0;
            }
            else
            {
                comboBoxLangTo.SelectedValue = langTo;
                if (comboBoxLangTo.SelectedIndex == -1)
                {
                    comboBoxLangTo.SelectedIndex = 0;
                }
            }
            _plugin.Controller.NotifyTranslateProviderChanged(true, (string)comboBoxProvider.SelectedValue,
                textBoxApiKey.Text, (string)comboBoxLangFrom.SelectedValue, (string)comboBoxLangTo.SelectedValue);
        }

        private void buttonProviderApply_Click(object sender, EventArgs e)
        {
            _plugin.Controller.NotifyTranslateProviderChanged(true, (string) comboBoxProvider.SelectedValue,
                textBoxApiKey.Text, (string) comboBoxLangFrom.SelectedValue, (string) comboBoxLangTo.SelectedValue);
        }
    }
}
