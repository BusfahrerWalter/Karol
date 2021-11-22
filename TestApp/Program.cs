using Karol;
using Karol.Core;
using Karol.Core.Rendering;
using System;
using System.Drawing;
using System.Threading;
using System.Collections.Generic;
using System.IO;

namespace TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            World w = World.Load(@"C:\Users\damuelle\Desktop\Brams\Test.cskw");

            //foreach(string file in Directory.EnumerateFiles(@"C:\Users\damuelle\Desktop\Brams"))
            //{
            //    if (!file.EndsWith(".cskw") && !file.EndsWith(".kdw"))
            //        continue;

            //    Console.WriteLine($"Next: {file}");
            //    World w = World.Load(file);
            //}

            //World w = new World(40, 3, 12);
            //RobotOptions o = new RobotOptions(w)
            //{
            //    InitialDirection = Direction.South,
            //    Delay = 20
            //};

            //Robot r = new Robot(5, 5, o);
            //r.Wait(10000);

            //for (int i = 0; i < 10; i++)
            //{
            //    Robot r = new Robot(i, i, w);
            //}

            //Robot[] rs = w.Robots;
            //while (true)
            //{
            //    foreach (var r in rs)
            //    {
            //        r.IsVisible = true;
            //        Thread t = new Thread(() =>
            //        {
            //            Thread.Sleep(350);
            //            r.IsVisible = false;
            //        });
            //        t.Start();
            //    }
            //}

            //int x, y, z;
            //Console.Write("X: ");
            //while (true)
            //{
            //    if (int.TryParse(Console.ReadLine(), out int xIn))
            //    {
            //        x = xIn;
            //        break;
            //    }
            //}

            //Console.Write("Y: ");
            //while (true)
            //{
            //    if (int.TryParse(Console.ReadLine(), out int yIn))
            //    {
            //        y = yIn;
            //        break;
            //    }
            //}

            //Console.Write("Z: ");
            //while (true)
            //{
            //    if (int.TryParse(Console.ReadLine(), out int zIn))
            //    {
            //        z = zIn;
            //        break;
            //    }
            //}

            //try
            //{
            //    World w = new World(x, y, z);
            //}
            //catch (Exception e) 
            //{
            //    Console.Write(e);
            //}

            //Main(args);

            //for (int i = 0; i < 300; i++)
            //{
            //    Thread t = new Thread(() =>
            //    {
            //        World w = new World(20, 2, 20);
            //        List<Robot> list = new List<Robot>(w.Width * w.Depth);

            //        for (int i = 0; i < w.Width; i++)
            //        {
            //            for (int j = 0; j < w.Depth; j++)
            //            {
            //                Robot r = new Robot(i, j, w);
            //                r.Delay = 0;
            //                list.Add(r);
            //            }
            //        }

            //        while (!!!!!!!!!!!!!!!!!!!!!!!!!!!!!false)
            //        {
            //            foreach (var r in list)
            //            {
            //                r.TurnRight();
            //                r.TurnRight();
            //            }
            //        }
            //    });

            //    t.Start();
            //}

            //World w = new World(20, 2, 20);
            //for (int i = 0; i < 3; i++)
            //{
            //    Robot r = new Robot(i, i, w);
            //    Thread t = new Thread(() =>
            //    {
            //        r.Delay = 20;

            //        while (true)
            //        {
            //            r.TurnRight();
            //        }
            //    });

            //    t.Start();
            //}
        }
    }
}
