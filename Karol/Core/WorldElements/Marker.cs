using Karol.Properties;
using Karol.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Karol.Core.WorldElements
{
    internal class Marker : WorldElement
    {
        private Robot _robotOnTop;

        public Robot RobotOnTop
        {
            get => _robotOnTop;
            set
            {
                _robotOnTop = value;
                if(value != null)
                {
                    int width = Math.Max(value.BitMap.Width, Resources.Marke.Width);
                    BitMap = new Bitmap(width, value.BitMap.Height);
                    BitMap.DrawImage(0, BitMap.Height - Resources.Marke.Height, Resources.Marke);
                    BitMap.DrawImage(0, 0, value.BitMap);
                }
                else
                {
                    BitMap = new Bitmap(Resources.Marke);
                }
            }
        }

        public Marker() : base(new Bitmap(Resources.Marke))
        {
            CanStackOnTop = false;
            CanPickUp = true;
        }
    }
}
