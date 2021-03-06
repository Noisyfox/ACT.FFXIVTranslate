﻿using System;
using System.Linq;
using System.Windows.Forms;
using ACT.FFXIVTranslate.localization;
using ACT.FoxCommon.dpi;
using ACT.FoxCommon.localization;

namespace ACT.FFXIVTranslate.translate
{
    public partial class TranslateProviderPanel : UserControl, IPluginComponent
    {
        private FFXIVTranslatePlugin _plugin;
        private TranslateService _service;

        public TranslateProviderPanel()
        {
            InitializeComponent();
            comboBoxProvider.DisplayMember = nameof(ITranslateProviderFactory.ProviderName);
            comboBoxProvider.ValueMember = nameof(ITranslateProviderFactory.ProviderId);
            comboBoxLangFrom.DisplayMember = nameof(LanguageDef.DisplayName);
            comboBoxLangFrom.ValueMember = nameof(LanguageDef.LangCode);
            comboBoxLangTo.DisplayMember = nameof(LanguageDef.DisplayName);
            comboBoxLangTo.ValueMember = nameof(LanguageDef.LangCode);

            this.AdjustForDpiScaling();
        }

        public void AttachToAct(FFXIVTranslatePlugin plugin)
        {
            _service = plugin.TranslateService;
            _plugin = plugin;

            var controller = plugin.Controller;
            controller.TranslateProviderChanged += ControllerOnTranslateProviderChanged;
            controller.LegalInfoChanged += ControllerOnLegalInfoChanged;
        }

        public void PostAttachToAct(FFXIVTranslatePlugin plugin)
        {
        }

        public void DoLocalization()
        {
            comboBoxProvider.DataSource = _service.AllProviders;
        }

        private void comboBoxProvider_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedProvider = (ITranslateProviderFactory) comboBoxProvider.SelectedItem;

            if (selectedProvider != null)
            {
                // Update language selector
                comboBoxLangFrom.DataSource = selectedProvider.SupportAutoDetect
                    ? new[] { new LanguageDef(LanguageDef.CodeAuto, strings.LangAutoDetect, string.Empty) }.Concat(selectedProvider.SupportedSrcLanguages).ToList()
                    : selectedProvider.SupportedSrcLanguages;

                UpdateSupportedDestLanguage();
            }
        }

        private void UpdateSupportedDestLanguage()
        {
            var selectedProvider = (ITranslateProviderFactory) comboBoxProvider.SelectedItem;
            var selectedFromLang = (LanguageDef) comboBoxLangFrom.SelectedItem;

            if (selectedProvider != null && selectedFromLang != null)
            {
                comboBoxLangTo.DataSource = selectedProvider.GetSupportedDestLanguages(selectedFromLang);
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

        private void ControllerOnLegalInfoChanged(bool fromView, ProviderLegalInfo info)
        {
            var label = info?.LabelResult;
            if (label != null)
            {
                linkLabelPowered.Visible = true;
                linkLabelPowered.Text = label;
                linkLabelPowered.Tag = info.LabelResultLink;
            }
            else
            {
                linkLabelPowered.Visible = false;
                linkLabelPowered.Tag = null;
            }
        }

        private void buttonProviderApply_Click(object sender, EventArgs e)
        {
            _plugin.Controller.NotifyTranslateProviderChanged(true, (string) comboBoxProvider.SelectedValue,
                textBoxApiKey.Text, (string) comboBoxLangFrom.SelectedValue, (string) comboBoxLangTo.SelectedValue);
        }

        private void linkLabelPowered_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (linkLabelPowered.Tag is string link)
            {
                System.Diagnostics.Process.Start(link);
            }
        }

        private void buttonFreeKey_Click(object sender, EventArgs e)
        {

            var selectedProvider = (ITranslateProviderFactory) comboBoxProvider.SelectedItem;

            if (selectedProvider != null)
            {
                var key = selectedProvider.DefaultPublicKey;
                if (string.IsNullOrEmpty(key))
                {
                    key = string.Empty;
                    MessageBox.Show(strings.messageNoFreeKey, strings.actPanelTitle,
                        MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
                textBoxApiKey.Text = key;
            }
        }

        private void ComboBoxLangFrom_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateSupportedDestLanguage();
        }
    }
}
