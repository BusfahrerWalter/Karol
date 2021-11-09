using Karol;
using Karol.Core;
using System;
using System.Drawing;
using Karol.Core.Rendering;
using System.Threading;

namespace TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            World w = new World(10, 5, 10);
            
            for(int i = 0; i < 3; i++)
            {
                Robot r = new Robot(i, i, w);
                Thread t = new Thread(() =>
                {
                    r.Delay = 0;

                    while (true)
                    {
                        r.TurnRight();
                    }

                });

                t.Start();
            }
        }
    }
}
