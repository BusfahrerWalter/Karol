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
            World w1 = new World(15,5,15);

            RobotOptions options = new RobotOptions(w1)
            {
                StartX = 0,
                StartZ = 0,
                NorthImage = new Bitmap(@"O:\muster\_alt_aus_10b\Web\Praktikum\pics\steinhuber.png"),
                EastImage = new Bitmap(@"O:\muster\_alt_aus_10b\Web\Praktikum\pics\steinmetz.png"),
                SouthImage = new Bitmap(@"O:\muster\_alt_aus_10b\Web\Praktikum\pics\nissen.png"),
                WestImage = new Bitmap(@"H:\Daten\sachen\fettekatz.jpg")
            };

            Robot r1 = new Robot(options);

            r1.Wait(1000);
        }
    }
}
