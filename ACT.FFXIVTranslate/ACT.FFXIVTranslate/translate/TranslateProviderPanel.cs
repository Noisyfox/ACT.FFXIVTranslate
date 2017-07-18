using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

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

            comboBoxProvider.DataSource = _service.AllProviders;

            var controller = plugin.Controller;
            controller.TranslateProviderChanged += ControllerOnTranslateProviderChanged;
        }

        private void comboBoxProvider_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedProvider = (ITranslaterProviderFactory) comboBoxProvider.SelectedItem;

            if (selectedProvider != null)
            {
                // Update language selector
                comboBoxLangFrom.DataSource = selectedProvider.SupportAutoDetect
                    ? new[] {LanguageDef.Auto}.Concat(selectedProvider.SupportedSrcLanguages).ToList()
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

            comboBoxProvider.SelectedValue = provider;
            if (comboBoxProvider.SelectedIndex == -1)
            {
                comboBoxProvider.SelectedIndex = 0;
            }

            textBoxApiKey.Text = apiKey;

            comboBoxLangFrom.SelectedValue = langFrom;
            if (comboBoxLangFrom.SelectedIndex == -1)
            {
                comboBoxLangFrom.SelectedIndex = 0;
            }
            comboBoxLangTo.SelectedValue = langTo;
            if (comboBoxLangTo.SelectedIndex == -1)
            {
                comboBoxLangTo.SelectedIndex = 0;
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
