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
    public class Brick : WorldElement
    {
        private static readonly Bitmap BrickBitmap = Resources.Ziegel;

        public Color Paint { get; set; }

        public Brick(Color paint)
        {
            Paint = paint;

            BitMap = new Bitmap(BrickBitmap);
            BitMap.MultiplyColor(paint);
        }

        public Brick() : this(Color.Red) { }
    }
}
