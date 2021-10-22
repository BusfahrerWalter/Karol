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
            World w2 = new World(10, 15, 20);
            World w1 = World.LoadImage(@"H:\Daten\sachen\fehler.PNG");
            //World w1 = new World(40, 88, 40);
            //Egon 
            RobotOptions o = new RobotOptions(w1)
            {
                StartX = 0,
                StartZ = 0,
                Delay = 10,
                Set = ImageSet.Default
            };

            SuperRobot r1 = new SuperRobot(o);
        }
    }
}
