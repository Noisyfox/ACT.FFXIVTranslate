namespace ACT.FFXIVTranslate.translate
{
    partial class TranslateProviderPanel
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBoxProvider = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.labelProvider = new System.Windows.Forms.Label();
            this.comboBoxProvider = new System.Windows.Forms.ComboBox();
            this.labelApiKey = new System.Windows.Forms.Label();
            this.textBoxApiKey = new System.Windows.Forms.TextBox();
            this.labelLang = new System.Windows.Forms.Label();
            this.labelLangFrom = new System.Windows.Forms.Label();
            this.labelLangTo = new System.Windows.Forms.Label();
            this.comboBoxLangFrom = new System.Windows.Forms.ComboBox();
            this.comboBoxLangTo = new System.Windows.Forms.ComboBox();
            this.buttonProviderApply = new System.Windows.Forms.Button();
            this.linkLabelPowered = new System.Windows.Forms.LinkLabel();
            this.groupBoxProvider.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxProvider
            // 
            this.groupBoxProvider.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxProvider.Controls.Add(this.tableLayoutPanel2);
            this.groupBoxProvider.Location = new System.Drawing.Point(3, 3);
            this.groupBoxProvider.Name = "groupBoxProvider";
            this.groupBoxProvider.Size = new System.Drawing.Size(433, 153);
            this.groupBoxProvider.TabIndex = 2;
            this.groupBoxProvider.TabStop = false;
            this.groupBoxProvider.Text = "Provider Settings";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel2.ColumnCount = 5;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.labelProvider, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.comboBoxProvider, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.labelApiKey, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.textBoxApiKey, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.labelLang, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.labelLangFrom, 1, 2);
            this.tableLayoutPanel2.Controls.Add(this.labelLangTo, 3, 2);
            this.tableLayoutPanel2.Controls.Add(this.comboBoxLangFrom, 2, 2);
            this.tableLayoutPanel2.Controls.Add(this.comboBoxLangTo, 4, 2);
            this.tableLayoutPanel2.Controls.Add(this.buttonProviderApply, 4, 3);
            this.tableLayoutPanel2.Controls.Add(this.linkLabelPowered, 0, 3);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(6, 20);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 4;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(421, 127);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // labelProvider
            // 
            this.labelProvider.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labelProvider.AutoSize = true;
            this.labelProvider.Location = new System.Drawing.Point(3, 7);
            this.labelProvider.Name = "labelProvider";
            this.labelProvider.Size = new System.Drawing.Size(59, 12);
            this.labelProvider.TabIndex = 0;
            this.labelProvider.Text = "Provider:";
            // 
            // comboBoxProvider
            // 
            this.comboBoxProvider.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel2.SetColumnSpan(this.comboBoxProvider, 4);
            this.comboBoxProvider.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxProvider.FormattingEnabled = true;
            this.comboBoxProvider.Location = new System.Drawing.Point(68, 3);
            this.comboBoxProvider.Name = "comboBoxProvider";
            this.comboBoxProvider.Size = new System.Drawing.Size(350, 20);
            this.comboBoxProvider.TabIndex = 1;
            this.comboBoxProvider.SelectedIndexChanged += new System.EventHandler(this.comboBoxProvider_SelectedIndexChanged);
            // 
            // labelApiKey
            // 
            this.labelApiKey.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labelApiKey.AutoSize = true;
            this.labelApiKey.Location = new System.Drawing.Point(3, 33);
            this.labelApiKey.Name = "labelApiKey";
            this.labelApiKey.Size = new System.Drawing.Size(53, 12);
            this.labelApiKey.TabIndex = 2;
            this.labelApiKey.Text = "API Key:";
            // 
            // textBoxApiKey
            // 
            this.textBoxApiKey.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel2.SetColumnSpan(this.textBoxApiKey, 4);
            this.textBoxApiKey.Location = new System.Drawing.Point(68, 29);
            this.textBoxApiKey.Name = "textBoxApiKey";
            this.textBoxApiKey.Size = new System.Drawing.Size(350, 21);
            this.textBoxApiKey.TabIndex = 3;
            this.textBoxApiKey.Text = "trnsl.1.1.20170716T025951Z.13c73247084b012d.3404189299f91adf7792235bc7cf7fb7f3bd2" +
    "6a2";
            // 
            // labelLang
            // 
            this.labelLang.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labelLang.AutoSize = true;
            this.labelLang.Location = new System.Drawing.Point(3, 60);
            this.labelLang.Name = "labelLang";
            this.labelLang.Size = new System.Drawing.Size(53, 12);
            this.labelLang.TabIndex = 4;
            this.labelLang.Text = "Language";
            // 
            // labelLangFrom
            // 
            this.labelLangFrom.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.labelLangFrom.AutoSize = true;
            this.labelLangFrom.Location = new System.Drawing.Point(68, 60);
            this.labelLangFrom.Name = "labelLangFrom";
            this.labelLangFrom.Size = new System.Drawing.Size(35, 12);
            this.labelLangFrom.TabIndex = 5;
            this.labelLangFrom.Text = "From:";
            // 
            // labelLangTo
            // 
            this.labelLangTo.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.labelLangTo.AutoSize = true;
            this.labelLangTo.Location = new System.Drawing.Point(252, 60);
            this.labelLangTo.Name = "labelLangTo";
            this.labelLangTo.Size = new System.Drawing.Size(23, 12);
            this.labelLangTo.TabIndex = 6;
            this.labelLangTo.Text = "To:";
            // 
            // comboBoxLangFrom
            // 
            this.comboBoxLangFrom.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxLangFrom.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxLangFrom.FormattingEnabled = true;
            this.comboBoxLangFrom.Location = new System.Drawing.Point(109, 56);
            this.comboBoxLangFrom.Name = "comboBoxLangFrom";
            this.comboBoxLangFrom.Size = new System.Drawing.Size(137, 20);
            this.comboBoxLangFrom.TabIndex = 7;
            // 
            // comboBoxLangTo
            // 
            this.comboBoxLangTo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxLangTo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxLangTo.FormattingEnabled = true;
            this.comboBoxLangTo.Location = new System.Drawing.Point(281, 56);
            this.comboBoxLangTo.Name = "comboBoxLangTo";
            this.comboBoxLangTo.Size = new System.Drawing.Size(137, 20);
            this.comboBoxLangTo.TabIndex = 8;
            // 
            // buttonProviderApply
            // 
            this.buttonProviderApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonProviderApply.Location = new System.Drawing.Point(343, 101);
            this.buttonProviderApply.Name = "buttonProviderApply";
            this.buttonProviderApply.Size = new System.Drawing.Size(75, 23);
            this.buttonProviderApply.TabIndex = 9;
            this.buttonProviderApply.Text = "Apply";
            this.buttonProviderApply.UseVisualStyleBackColor = true;
            this.buttonProviderApply.Click += new System.EventHandler(this.buttonProviderApply_Click);
            // 
            // linkLabelPowered
            // 
            this.linkLabelPowered.AutoSize = true;
            this.tableLayoutPanel2.SetColumnSpan(this.linkLabelPowered, 4);
            this.linkLabelPowered.Location = new System.Drawing.Point(3, 79);
            this.linkLabelPowered.Name = "linkLabelPowered";
            this.linkLabelPowered.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.linkLabelPowered.Size = new System.Drawing.Size(167, 17);
            this.linkLabelPowered.TabIndex = 10;
            this.linkLabelPowered.TabStop = true;
            this.linkLabelPowered.Text = "Powered by Yandex.Translate";
            // 
            // TranslateProviderPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBoxProvider);
            this.Name = "TranslateProviderPanel";
            this.Size = new System.Drawing.Size(436, 159);
            this.groupBoxProvider.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxProvider;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label labelProvider;
        private System.Windows.Forms.ComboBox comboBoxProvider;
        private System.Windows.Forms.Label labelApiKey;
        private System.Windows.Forms.TextBox textBoxApiKey;
        private System.Windows.Forms.Label labelLang;
        private System.Windows.Forms.Label labelLangFrom;
        private System.Windows.Forms.Label labelLangTo;
        private System.Windows.Forms.ComboBox comboBoxLangFrom;
        private System.Windows.Forms.ComboBox comboBoxLangTo;
        private System.Windows.Forms.Button buttonProviderApply;
        private System.Windows.Forms.LinkLabel linkLabelPowered;
    }
}
