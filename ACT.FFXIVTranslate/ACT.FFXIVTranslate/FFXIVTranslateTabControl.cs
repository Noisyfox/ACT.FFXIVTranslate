using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ACT.FFXIVTranslate
{
    public partial class FFXIVTranslateTabControl : UserControl
    {
        public FFXIVTranslateTabControl()
        {
            InitializeComponent();
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
            settings.AddControlSetting(checkBoxResizeByDrag);
        }

        private void ParentTabPageOnResize(object sender, EventArgs eventArgs)
        {
            Location = new Point(0, 0);
            Size = ((TabPage)sender).Size;
        }

        private void trackBarOpacity_ValueChanged(object sender, EventArgs e)
        {
            labelOpacityValue.Text = $"{trackBarOpacity.Value}%";
        }

        private void buttonFont_Click(object sender, EventArgs e)
        {

        }
    }
}
