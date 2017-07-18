namespace ACT.FFXIVTranslate
{
    partial class FFXIVTranslateTabControl
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
            this.groupBox_overlay = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.numericUpDownY = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.numericUpDownX = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.numericUpDownWidth = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownHeight = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.trackBarOpacity = new System.Windows.Forms.TrackBar();
            this.labelOpacityValue = new System.Windows.Forms.Label();
            this.checkBoxClickthrough = new System.Windows.Forms.CheckBox();
            this.buttonFont = new System.Windows.Forms.Button();
            this.textBoxFont = new System.Windows.Forms.TextBox();
            this.translateProviderPanel = new ACT.FFXIVTranslate.translate.TranslateProviderPanel();
            this.groupBox_overlay.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarOpacity)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox_overlay
            // 
            this.groupBox_overlay.Controls.Add(this.tableLayoutPanel1);
            this.groupBox_overlay.Location = new System.Drawing.Point(3, 3);
            this.groupBox_overlay.Name = "groupBox_overlay";
            this.groupBox_overlay.Size = new System.Drawing.Size(377, 182);
            this.groupBox_overlay.TabIndex = 0;
            this.groupBox_overlay.TabStop = false;
            this.groupBox_overlay.Text = "Overlay Settings";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 5;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.numericUpDownY, 4, 0);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label2, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label3, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.numericUpDownX, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label5, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.label6, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this.numericUpDownWidth, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.numericUpDownHeight, 4, 1);
            this.tableLayoutPanel1.Controls.Add(this.label7, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.trackBarOpacity, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.labelOpacityValue, 4, 2);
            this.tableLayoutPanel1.Controls.Add(this.checkBoxClickthrough, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.buttonFont, 4, 3);
            this.tableLayoutPanel1.Controls.Add(this.textBoxFont, 0, 3);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(6, 20);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(365, 156);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // numericUpDownY
            // 
            this.numericUpDownY.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.numericUpDownY.Location = new System.Drawing.Point(265, 3);
            this.numericUpDownY.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numericUpDownY.Minimum = new decimal(new int[] {
            65535,
            0,
            0,
            -2147483648});
            this.numericUpDownY.Name = "numericUpDownY";
            this.numericUpDownY.Size = new System.Drawing.Size(97, 21);
            this.numericUpDownY.TabIndex = 4;
            this.numericUpDownY.Value = new decimal(new int[] {
            200,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "Position";
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(86, 7);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(17, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "X:";
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(242, 7);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(17, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "Y:";
            // 
            // numericUpDownX
            // 
            this.numericUpDownX.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.numericUpDownX.Location = new System.Drawing.Point(109, 3);
            this.numericUpDownX.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numericUpDownX.Minimum = new decimal(new int[] {
            65535,
            0,
            0,
            -2147483648});
            this.numericUpDownX.Name = "numericUpDownX";
            this.numericUpDownX.Size = new System.Drawing.Size(97, 21);
            this.numericUpDownX.TabIndex = 3;
            this.numericUpDownX.Value = new decimal(new int[] {
            200,
            0,
            0,
            0});
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 34);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 12);
            this.label4.TabIndex = 5;
            this.label4.Text = "Size";
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(62, 34);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 12);
            this.label5.TabIndex = 6;
            this.label5.Text = "Width:";
            // 
            // label6
            // 
            this.label6.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(212, 34);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(47, 12);
            this.label6.TabIndex = 7;
            this.label6.Text = "Height:";
            // 
            // numericUpDownWidth
            // 
            this.numericUpDownWidth.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.numericUpDownWidth.Location = new System.Drawing.Point(109, 30);
            this.numericUpDownWidth.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numericUpDownWidth.Minimum = new decimal(new int[] {
            300,
            0,
            0,
            0});
            this.numericUpDownWidth.Name = "numericUpDownWidth";
            this.numericUpDownWidth.Size = new System.Drawing.Size(97, 21);
            this.numericUpDownWidth.TabIndex = 8;
            this.numericUpDownWidth.Value = new decimal(new int[] {
            300,
            0,
            0,
            0});
            // 
            // numericUpDownHeight
            // 
            this.numericUpDownHeight.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.numericUpDownHeight.Location = new System.Drawing.Point(265, 30);
            this.numericUpDownHeight.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numericUpDownHeight.Minimum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.numericUpDownHeight.Name = "numericUpDownHeight";
            this.numericUpDownHeight.Size = new System.Drawing.Size(97, 21);
            this.numericUpDownHeight.TabIndex = 9;
            this.numericUpDownHeight.Value = new decimal(new int[] {
            200,
            0,
            0,
            0});
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(3, 54);
            this.label7.Name = "label7";
            this.label7.Padding = new System.Windows.Forms.Padding(0, 10, 0, 0);
            this.label7.Size = new System.Drawing.Size(53, 22);
            this.label7.TabIndex = 10;
            this.label7.Text = "Opacity:";
            // 
            // trackBarOpacity
            // 
            this.trackBarOpacity.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.trackBarOpacity, 2);
            this.trackBarOpacity.Location = new System.Drawing.Point(109, 57);
            this.trackBarOpacity.Maximum = 100;
            this.trackBarOpacity.Name = "trackBarOpacity";
            this.trackBarOpacity.Size = new System.Drawing.Size(150, 45);
            this.trackBarOpacity.TabIndex = 11;
            this.trackBarOpacity.TickFrequency = 10;
            this.trackBarOpacity.Value = 100;
            this.trackBarOpacity.ValueChanged += new System.EventHandler(this.trackBarOpacity_ValueChanged);
            // 
            // labelOpacityValue
            // 
            this.labelOpacityValue.AutoSize = true;
            this.labelOpacityValue.Location = new System.Drawing.Point(265, 54);
            this.labelOpacityValue.Name = "labelOpacityValue";
            this.labelOpacityValue.Padding = new System.Windows.Forms.Padding(0, 10, 0, 0);
            this.labelOpacityValue.Size = new System.Drawing.Size(29, 22);
            this.labelOpacityValue.TabIndex = 12;
            this.labelOpacityValue.Text = "100%";
            // 
            // checkBoxClickthrough
            // 
            this.checkBoxClickthrough.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.checkBoxClickthrough.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.checkBoxClickthrough, 5);
            this.checkBoxClickthrough.Location = new System.Drawing.Point(3, 137);
            this.checkBoxClickthrough.Name = "checkBoxClickthrough";
            this.checkBoxClickthrough.Size = new System.Drawing.Size(96, 16);
            this.checkBoxClickthrough.TabIndex = 16;
            this.checkBoxClickthrough.Text = "Clickthrough";
            this.checkBoxClickthrough.UseVisualStyleBackColor = true;
            // 
            // buttonFont
            // 
            this.buttonFont.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonFont.Location = new System.Drawing.Point(265, 108);
            this.buttonFont.Name = "buttonFont";
            this.buttonFont.Size = new System.Drawing.Size(97, 23);
            this.buttonFont.TabIndex = 14;
            this.buttonFont.Text = "Change Font";
            this.buttonFont.UseVisualStyleBackColor = true;
            this.buttonFont.Click += new System.EventHandler(this.buttonFont_Click);
            // 
            // textBoxFont
            // 
            this.textBoxFont.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.textBoxFont, 4);
            this.textBoxFont.Location = new System.Drawing.Point(3, 109);
            this.textBoxFont.Name = "textBoxFont";
            this.textBoxFont.ReadOnly = true;
            this.textBoxFont.Size = new System.Drawing.Size(256, 21);
            this.textBoxFont.TabIndex = 17;
            // 
            // translateProviderPanel
            // 
            this.translateProviderPanel.Location = new System.Drawing.Point(3, 191);
            this.translateProviderPanel.Name = "translateProviderPanel";
            this.translateProviderPanel.Size = new System.Drawing.Size(377, 159);
            this.translateProviderPanel.TabIndex = 1;
            // 
            // FFXIVTranslateTabControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.Controls.Add(this.translateProviderPanel);
            this.Controls.Add(this.groupBox_overlay);
            this.Name = "FFXIVTranslateTabControl";
            this.Size = new System.Drawing.Size(757, 433);
            this.groupBox_overlay.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarOpacity)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox_overlay;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.NumericUpDown numericUpDownY;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numericUpDownX;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown numericUpDownWidth;
        private System.Windows.Forms.NumericUpDown numericUpDownHeight;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TrackBar trackBarOpacity;
        private System.Windows.Forms.Label labelOpacityValue;
        private System.Windows.Forms.Button buttonFont;
        private System.Windows.Forms.CheckBox checkBoxClickthrough;
        private System.Windows.Forms.TextBox textBoxFont;
        private translate.TranslateProviderPanel translateProviderPanel;
    }
}
