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
            World w1 = World.LoadImage(@"C:\Users\danie\Downloads\map.png", 2);
            //w1.PlaceRandomBricks(30 * 30 / 2, 1, true);

            Robot r1 = new Robot(0, 0, w1);
            r1.JumpHeight = 2;

            Console.WriteLine(r1.BrickColor);
            r1.Place();
            r1.Place(Color.Orange);
            Console.WriteLine(r1.BrickColor);

            //Random rand = new Random();
            //Color[] paints = new Color[]
            //{
            //    Color.Red,
            //    Color.Green,
            //    Color.Blue,
            //    Color.Yellow,
            //    Color.Purple,
            //    Color.Orange,
            //    Color.Cyan,
            //    Color.Magenta
            //};


            //for(int j = 0; j < w1.SizeX; j++)
            //{
            //    for (int i = 0; i < w1.SizeZ - 1; i++)
            //    {
            //        r1.Paint = paints[rand.Next(0, paints.Length)];
            //        r1.Place();
            //        r1.Move();
            //    }

            //    r1.Paint = paints[rand.Next(0, paints.Length)];
            //    if (j % 2 == 0)
            //    {
            //        r1.TurnRight();
            //        if (!r1.CanMove)
            //            break;

            //        r1.Place();
            //        r1.Move();
            //        r1.TurnRight();
            //    }
            //    else
            //    {
            //        r1.TurnLeft();
            //        r1.Place();
            //        r1.Move();
            //        r1.TurnLeft();
            //    }
            //}


























            //Random rand = new Random();

            //for(int i = 0; i < 5000; i++)
            //{
            //    if (i % 2 == 0) r1.Paint = Color.Green;
            //    else r1.Paint = Color.Blue;

            //    int num = rand.Next(1, 100);

            //    if(num <= 50)
            //    {
            //        r1.TurnLeft();
            //    }
            //    else
            //    {
            //        r1.TurnRight();
            //    }

            //    if(rand.Next(1, 100) > 30 && !r1.HasWall && r1.BricksInFront < w1.SizeY)
            //    {
            //        r1.Place();
            //    }

            //    if (r1.CanMove)
            //    {
            //        r1.Move();
            //    }
            //}





















            //Robot r2 = new Robot(0, 1, w1);
            //Robot r3 = new Robot(0, 1, w1);
            //Random rand = new Random();

            //r1.Delay = 5;
            //r2.Delay = 5;
            //r3.Delay = 5;

            //for(int i = 0; i < 100; i++)
            //while (true)
            //{
            //    int num = rand.Next(1, 100);
            //    if(num <= 30)
            //    {
            //        r1.TurnLeft();
            //        r2.TurnRight();
            //    }
            //    else if(num > 70)
            //    {
            //        r1.TurnRight();
            //        r3.TurnLeft();
            //    }
            //    else
            //    {
            //        r3.TurnRight();
            //        r2.TurnLeft();
            //    }

            //    if(r1.CanMove)
            //    {
            //        r1.Move();
            //    }

            //    if(r2.CanMove)
            //    {
            //        r2.Move();
            //    }

            //    if (r3.CanMove)
            //    {
            //        r3.Move();
            //    }
            //}
        }
    }
}
