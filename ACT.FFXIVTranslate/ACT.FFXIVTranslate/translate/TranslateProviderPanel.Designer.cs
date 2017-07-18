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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.label8 = new System.Windows.Forms.Label();
            this.comboBoxProvider = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.textBoxApiKey = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.comboBoxLangFrom = new System.Windows.Forms.ComboBox();
            this.comboBoxLangTo = new System.Windows.Forms.ComboBox();
            this.buttonProviderApply = new System.Windows.Forms.Button();
            this.linkLabelPowered = new System.Windows.Forms.LinkLabel();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.tableLayoutPanel2);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(433, 153);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Provider Settings";
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
            this.tableLayoutPanel2.Controls.Add(this.label8, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.comboBoxProvider, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.label9, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.textBoxApiKey, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.label10, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.label11, 1, 2);
            this.tableLayoutPanel2.Controls.Add(this.label12, 3, 2);
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
            // label8
            // 
            this.label8.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(3, 7);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(59, 12);
            this.label8.TabIndex = 0;
            this.label8.Text = "Provider:";
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
            // label9
            // 
            this.label9.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(3, 33);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(53, 12);
            this.label9.TabIndex = 2;
            this.label9.Text = "API Key:";
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
            // label10
            // 
            this.label10.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(3, 60);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(53, 12);
            this.label10.TabIndex = 4;
            this.label10.Text = "Language";
            // 
            // label11
            // 
            this.label11.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(68, 60);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(35, 12);
            this.label11.TabIndex = 5;
            this.label11.Text = "From:";
            // 
            // label12
            // 
            this.label12.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(252, 60);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(23, 12);
            this.label12.TabIndex = 6;
            this.label12.Text = "To:";
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
            this.Controls.Add(this.groupBox1);
            this.Name = "TranslateProviderPanel";
            this.Size = new System.Drawing.Size(436, 159);
            this.groupBox1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox comboBoxProvider;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox textBoxApiKey;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.ComboBox comboBoxLangFrom;
        private System.Windows.Forms.ComboBox comboBoxLangTo;
        private System.Windows.Forms.Button buttonProviderApply;
        private System.Windows.Forms.LinkLabel linkLabelPowered;
    }
}
