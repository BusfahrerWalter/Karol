using Karol.Properties;
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
        public static Bitmap[] LoadRobotBitmaps(int roboNumber)
        {
            roboNumber += 1;
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
