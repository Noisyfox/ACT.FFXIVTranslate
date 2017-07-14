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
        private MainController _controller;

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

            _controller = plugin.Controller;

            numericUpDownX.ValueChanged+= NumericUpDownPositionOnValueChanged;
            numericUpDownY.ValueChanged += NumericUpDownPositionOnValueChanged;
            numericUpDownWidth.ValueChanged += NumericUpDownSizeOnValueChanged;
            numericUpDownHeight.ValueChanged += NumericUpDownSizeOnValueChanged;

            _controller.OverlayMoved += ControllerOnOverlayMoved;
            _controller.OverlayResized += ControllerOnOverlayResized;

            trackBarOpacity_ValueChanged(this, EventArgs.Empty);
            NumericUpDownPositionOnValueChanged(this, EventArgs.Empty);
            NumericUpDownSizeOnValueChanged(this, EventArgs.Empty);
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
    }
}
