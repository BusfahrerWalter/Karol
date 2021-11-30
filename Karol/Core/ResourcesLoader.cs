using Karol.Properties;
using System;
using System.Drawing;
using System.Linq;
using System.Reflection;

namespace Karol.Core
{
    /// <summary>
    /// Hilfsklasse um Resourcen zu laden.
    /// </summary>
    internal class ResourcesLoader
    {
        public static Bitmap[] RobotBitmaps2D = new Bitmap[]
        {
            Resources.Robot_2D_N,
            Resources.Robot_2D_E,
            Resources.Robot_2D_S,
            Resources.Robot_2D_W
        };

        public static Bitmap[] RobotBitmaps = new Bitmap[]
        {
            Resources.robotN1,
            Resources.robotO1,
            Resources.robotS1,
            Resources.robotW1
        };
    }
}
