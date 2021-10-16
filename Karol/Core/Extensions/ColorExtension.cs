using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Karol.Extensions
{
    public static class ColorExtension
    {
        public static Color Invert(this Color c)
        {
            return Color.FromArgb(255, 255 - c.R, 255 - c.G, 255 - c.B);
        }
    }
}
