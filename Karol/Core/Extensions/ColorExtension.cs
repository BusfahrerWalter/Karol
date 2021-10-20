using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Karol.Extensions
{
    /// <summary>
    /// Erweiterungsklasse für Color
    /// </summary>
    internal static class ColorExtension
    {
        /// <summary>
        /// Invertiert eine Farbe
        /// </summary>
        /// <param name="c">Farbe</param>
        /// <returns>Invertierte Farbe</returns>
        public static Color Invert(this Color c)
        {
            return Color.FromArgb(255, 255 - c.R, 255 - c.G, 255 - c.B);
        }
    }
}
