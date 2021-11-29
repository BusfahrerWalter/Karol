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

        public static Bitmap[] LoadRobotBitmaps(int roboNumber)
        {

            roboNumber++;
            if (roboNumber > 9)
                roboNumber = 1;

            var flags = BindingFlags.Static | BindingFlags.NonPublic;
            return new Bitmap[]
            {
                (Bitmap)typeof(Resources).GetProperty($"robotN{roboNumber}", flags).GetValue(null),
                (Bitmap)typeof(Resources).GetProperty($"robotO{roboNumber}", flags).GetValue(null),
                (Bitmap)typeof(Resources).GetProperty($"robotS{roboNumber}", flags).GetValue(null),
                (Bitmap)typeof(Resources).GetProperty($"robotW{roboNumber}", flags).GetValue(null),
            };
        }
    }
}
