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
            World w = new World(10, 10, 10);
            Robot r = new Robot(w);

            while (!false)
            {
                r.Move();
            }

            //World w = new World(40, 3, 1);
            //RobotOptions o = new RobotOptions(w)
            //{
            //    InitialDirection = Direction.South,
            //    Set = ImageSet.Create(
            //        @"C:\Users\damuelle\Downloads\drachen\N.jpg", 
            //        @"C:\Users\damuelle\Downloads\drachen\O.jpg",
            //        @"C:\Users\damuelle\Downloads\drachen\S.jpg", 
            //        @"C:\Users\damuelle\Downloads\drachen\W.jpg")
            //};

            //for(int i = 0; i < w.Width; i++)
            //{
            //    Robot r = new Robot(i, 0, o);
            //    r.IsVisible = false;
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

            //for(int i = 0; i < 300; i++)
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
