using Karol;
using Karol.Core;
using System;
using System.Drawing;

namespace TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            World w1 = new World(35, 10, 35);
            w1.PlaceRandomBricks(10);

            var map = new Bitmap(@"E:\Files\Schule\Schule 12\TrFi\C_Gartenzaun_Karol\Karol\Karol\Resources\Images\Freddy.png");
            RobotOptions options = new RobotOptions(w1)
            {
                StartX = 0,
                StartZ = 0,
                NorthImage = map,
                SouthImage = map,
                EastImage = map,
                WestImage = map
            };

            Robot freddy = new Robot(options);
            freddy.MakeSound();
        }
    }
}
