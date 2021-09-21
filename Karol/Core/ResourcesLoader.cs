using Karol.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Resources;
using System.Text;
using System.Windows.Forms;

namespace Karol.Core
{
    public class ResourcesLoader
    {
        public static Bitmap LoadBitmap(string name)
        {
            return (Bitmap)Resources.ResourceManager.GetObject(@$"Resources\Images\{name}");
        }

        public static Bitmap[] LoadRoboters()
        {
            return new Bitmap[]
            {
                LoadBitmap("robot0")
            };
        }
    }
}
