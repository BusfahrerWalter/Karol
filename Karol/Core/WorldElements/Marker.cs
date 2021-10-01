using Karol.Properties;
using Karol.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Karol.Core.Annotations;

namespace Karol.Core.WorldElements
{
    [WorldElementInfo('M')]
    public class Marker : WorldElement, IContainer
    {
        private WorldElement _content;

        /// <summary>
        /// Roboter der auf der Marke steht
        /// </summary>
        public WorldElement Content
        {
            get => _content;
            set
            {
                _content = value;
                if(value != null)
                {
                    int absX = Math.Abs(value.XOffset);
                    int absY = Math.Abs(value.YOffset);
                    int width = Math.Max(value.BitMap.Width, Resources.Marke.Width) + absX;
                    int height = value.BitMap.Height + absY;

                    BitMap = new Bitmap(width, height);
                    BitMap.DrawImage(absX, BitMap.Height - Resources.Marke.Height, Resources.Marke);
                    BitMap.DrawImage(0, 0, value.BitMap);

                    XOffset = value.XOffset;
                }
                else
                {
                    BitMap = new Bitmap(Resources.Marke);
                    XOffset = 0;
                    YOffset = 0;
                }
            }
        }

        public bool IsEmpty => Content == null;

        public Marker() : base(new Bitmap(Resources.Marke))
        {
            CanStackOnTop = false;
            CanPickUp = false;
        }

        public Marker(Robot robot) 
        {
            CanStackOnTop = false;
            CanPickUp = false;
            Content = robot;
        }

        public void Reset()
        {
            Content = null;
        }
    }
}
