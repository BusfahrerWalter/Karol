using Karol.Properties;
using Karol.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Karol.Core.Annotations;

namespace Karol.Core.WorldElements
{
    /// <summary>
    /// Stellt einen Ziegel in der Welt dar.
    /// </summary>
    [WorldElementInfo('B')]
    internal class Brick : WorldElement
    {
        private static readonly Bitmap BrickBitmap = Resources.Ziegel;
        private Color _paint;

        public Color Paint
        {
            get => _paint;
            set
            {
                _paint = value;
                BitMap = new Bitmap(BrickBitmap);
                BitMap.MultiplyColor(value);
            }
        }

        internal override string Metadata
        {
            get
            {
                if (Paint.R == 255 && Paint.G == 0 && Paint.B == 0)
                    return string.Empty;

                return $"{Paint.R},{Paint.G},{Paint.B}";
            }
            set
            {
                string[] arr = value.Split(",");
                if (arr.Length < 3)
                    return;

                if (int.TryParse(arr[0], out int r) && int.TryParse(arr[1], out int g) && int.TryParse(arr[2], out int b))
                {
                    Paint = Color.FromArgb(255, r, g, b);
                    ViewColor2D = Paint;
                }
            }
        }

        public Brick(Color paint) : base()
        {
            Paint = paint;
            ViewColor2D = Paint;
        }

        public Brick() : this(Color.Red) { }
    }
}
