using Karol;
using Karol.Core;
using System;
using System.Diagnostics;
using System.Threading;
using System.Reflection;
using System.Drawing;
using Karol.Core.Rendering;
using System.Threading.Tasks;

namespace TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            for (int i = 0; i < 10; i++)
            {
                Solve(i);
            }
        }

        private static void Solve(int i)
        {
            World w1 = World.Load(@$"H:\Daten\Fächer\java\lib\karol\labs\lab{i}.kdw");
            if (w1 == null)
                return;

            w1.RenderingMode = WorldRenderingMode.Render2D;
            Robot r1 = w1.Robots[0];
            Random rand = new Random();
            DateTime start = DateTime.Now;

            r1.Delay = 0;

            while (!r1.HasMark)
            {
                if (!r1.HasWall)
                {
                    r1.Move();
                }

                int a = rand.Next(100);
                if (a < 33)
                {
                    r1.TurnRight();
                }
                else if(a >= 33 && a < 66)
                {
                    r1.TurnLeft();
                }
            }

            Console.WriteLine("JAAAAAAAA   " + (DateTime.Now - start).TotalSeconds);
        }
    }
}
