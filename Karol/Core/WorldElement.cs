using System.Drawing;

namespace Karol.Core
{
    public class WorldElement  
    {
        internal Bitmap BitMap { get; set; }
        internal int XOffset { get; set; }
        internal int YOffset { get; set; }

        public WorldElement(Bitmap bitMap)
        {
            BitMap = bitMap;
        }

        public WorldElement() : this(null) { }
    }
}
