using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ACT.FFXIVTranslate
{
    public class MainController
    {
        public delegate void OnOverlayMovedDelegate(bool fromView, int x, int y);

        public event OnOverlayMovedDelegate OverlayMoved;

        public void NotifyOverlayMoved(bool fromView, int x, int y)
        {
            OverlayMoved?.Invoke(fromView, x, y);
        }

        public delegate void OnOverlayResizedDelegate(bool fromView, int w, int h);

        public event OnOverlayResizedDelegate OverlayResized;

        public void NotifyOverlayResized(bool fromView, int w, int h)
        {
            OverlayResized?.Invoke(fromView, w, h);
        }

        public delegate void OnOpacityChangedDelegate(bool fromView, double value);

        public event OnOpacityChangedDelegate OpacityChanged;

        public void NotifyOpacityChanged(bool fromView, double value)
        {
            OpacityChanged?.Invoke(fromView, value);
        }
    }
}
