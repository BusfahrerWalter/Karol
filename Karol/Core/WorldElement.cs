using System.Drawing;

namespace Karol.Core
{
    public class WorldElement  
    {
        public Bitmap BitMap { get; set; }

        public WorldElement(Bitmap bitMap)
        {
            BitMap = bitMap;
        }
    }
}
