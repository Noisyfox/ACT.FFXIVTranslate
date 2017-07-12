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

        public void AttachToACT(TabPage parentTabPage)
        {
            parentTabPage.Controls.Add(this);
            parentTabPage.Resize += ParentTabPageOnResize;
            ParentTabPageOnResize(parentTabPage, null);
        }

        private void ParentTabPageOnResize(object sender, EventArgs eventArgs)
        {
            Location = new Point(0, 0);
            Size = ((TabPage)sender).Size;
        }
    }
}
