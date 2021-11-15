using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Karol.Core.Rendering
{
    internal class CellInfo2D
    {
        public bool DrawSolidColor => Image == null || FillColor != null;
        public Color? FillColor { get; set; }
        public Bitmap Image { get; set; }

        public CellInfo2D()
        {
            FillColor = null;
            Image = null;
        }
    }
}
