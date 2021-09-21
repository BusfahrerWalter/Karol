using Karol;
using Karol.Core;
using System;
using System.Diagnostics;
using System.Threading;
using System.Reflection;
using System.Drawing;

namespace TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            World w1 = new World(10, 4, 10);
            w1.PlaceRandomBricks(10, 4);

            Robot[] rs = new Robot[]
            {
                new Robot(0, 0, w1),
                new Robot(0, 4, w1),
                new Robot(1, 4, w1),
                new Robot(2, 4, w1),
                new Robot(3, 4, w1),
                new Robot(4, 4, w1),
                new Robot(1, 0, w1),
                new Robot(2, 0, w1),
                new Robot(3, 0, w1)
            };

            foreach (var r in rs)
            {
                r.Delay = 50;
                for (int i = 0; i < 2; i++)
                {
                    r.TurnRight();
                }
            }
        }
    }
}
