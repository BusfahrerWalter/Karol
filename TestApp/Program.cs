using Karol;
using Karol.Core;
using System;
using System.Drawing;
using Karol.Core.Rendering;
using System.Threading;
using System.Collections.Generic;

namespace TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            for(int i = 0; i < 300; i++)
            {
                Thread t = new Thread(() =>
                {
                    World w = new World(20, 2, 20);
                    List<Robot> list = new List<Robot>(w.Width * w.Depth);

                    for (int i = 0; i < w.Width; i++)
                    {
                        for (int j = 0; j < w.Depth; j++)
                        {
                            Robot r = new Robot(i, j, w);
                            r.Delay = 0;
                            list.Add(r);
                        }
                    }

                    while (!!!!!!!!!!!!!!!!!!!!!!!!!!!!!false)
                    {
                        foreach (var r in list)
                        {
                            r.TurnRight();
                            r.TurnRight();
                        }
                    }
                });

                t.Start();
            }

            //for(int i = 0; i < 9; i++)
            //{
            //    Robot r = new Robot(i, i, w);
            //    Thread t = new Thread(() =>
            //    {
            //        r.Delay = 20;

            //        while (true)
            //        {
            //            r.TurnRight();
            //            if (!r.HasWall && !r.HasRobot)
            //                r.Move();
            //        }

            //    });

            //    t.Start();
            //}
        }
    }
}
