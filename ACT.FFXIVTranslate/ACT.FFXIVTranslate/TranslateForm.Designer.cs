namespace ACT.FFXIVTranslate
{
    partial class TranslateForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.richTextBoxContent = new System.Windows.Forms.RichTextBox();
            this.tableLayoutPanelContent = new System.Windows.Forms.TableLayoutPanel();
            this.linkLabelLegalInfo = new System.Windows.Forms.LinkLabel();
            this.timerAutoHide = new System.Windows.Forms.Timer(this.components);
            this.tableLayoutPanelContent.SuspendLayout();
            this.SuspendLayout();
            // 
            // richTextBoxContent
            // 
            this.richTextBoxContent.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBoxContent.BackColor = System.Drawing.Color.Black;
            this.richTextBoxContent.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tableLayoutPanelContent.SetColumnSpan(this.richTextBoxContent, 2);
            this.richTextBoxContent.ForeColor = System.Drawing.Color.White;
            this.richTextBoxContent.Location = new System.Drawing.Point(3, 23);
            this.richTextBoxContent.Name = "richTextBoxContent";
            this.richTextBoxContent.ReadOnly = true;
            this.richTextBoxContent.Size = new System.Drawing.Size(234, 145);
            this.richTextBoxContent.TabIndex = 0;
            this.richTextBoxContent.Text = "";
            // 
            // tableLayoutPanelContent
            // 
            this.tableLayoutPanelContent.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanelContent.ColumnCount = 2;
            this.tableLayoutPanelContent.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelContent.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelContent.Controls.Add(this.linkLabelLegalInfo, 1, 0);
            this.tableLayoutPanelContent.Controls.Add(this.richTextBoxContent, 0, 1);
            this.tableLayoutPanelContent.Cursor = System.Windows.Forms.Cursors.SizeAll;
            this.tableLayoutPanelContent.Location = new System.Drawing.Point(12, 12);
            this.tableLayoutPanelContent.Name = "tableLayoutPanelContent";
            this.tableLayoutPanelContent.RowCount = 2;
            this.tableLayoutPanelContent.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelContent.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelContent.Size = new System.Drawing.Size(240, 171);
            this.tableLayoutPanelContent.TabIndex = 1;
            this.tableLayoutPanelContent.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TranslateForm_MouseDown);
            // 
            // linkLabelLegalInfo
            // 
            this.linkLabelLegalInfo.AutoSize = true;
            this.linkLabelLegalInfo.Location = new System.Drawing.Point(118, 0);
            this.linkLabelLegalInfo.Name = "linkLabelLegalInfo";
            this.linkLabelLegalInfo.Size = new System.Drawing.Size(119, 12);
            this.linkLabelLegalInfo.TabIndex = 0;
            this.linkLabelLegalInfo.TabStop = true;
            this.linkLabelLegalInfo.Text = "Powered by XXXXXXXX";
            this.linkLabelLegalInfo.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // timerAutoHide
            // 
            this.timerAutoHide.Interval = 1000;
            this.timerAutoHide.Tick += new System.EventHandler(this.timerAutoHide_Tick);
            // 
            // TranslateForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(300, 200);
            this.ControlBox = false;
            this.Controls.Add(this.tableLayoutPanelContent);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(300, 200);
            this.Name = "TranslateForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TranslateForm_FormClosing);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TranslateForm_MouseDown);
            this.tableLayoutPanelContent.ResumeLayout(false);
            this.tableLayoutPanelContent.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBoxContent;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelContent;
        private System.Windows.Forms.LinkLabel linkLabelLegalInfo;
        private System.Windows.Forms.Timer timerAutoHide;
    }
}