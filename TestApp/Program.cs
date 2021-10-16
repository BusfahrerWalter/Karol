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
            //for(int i = 0; i < 10; i++)
            //{
            //    Solve(i);
            //}

            World w1 = World.Load(@$"E:\Files\Schule\Schule 12\TrFi\C_Gartenzaun_Karol\labs\lab0.kdw");
            w1.SetRenderingMode(WorldRenderingMode.Render2D);
            Robot r1 = w1.Robots[0];

            while (!r1.HasMark)
            {
                if (r1.HasWall || !r1.HasWallLeft)
                    r1.TurnRight();
                else
                    r1.Move();
            }
        }

        private static void Solve(int i)
        {
            World w1 = World.Load(@$"E:\Files\Schule\Schule 12\TrFi\C_Gartenzaun_Karol\labs\lab{i}.kdw");
            w1.SetRenderingMode(WorldRenderingMode.Render2D);
            Robot r1 = w1.Robots[0];
            Random rand = new Random();
            DateTime start = DateTime.Now;

            r1.Delay = 0;

            while (!r1.HasMark)
            {
                if (!r1.HasWall && Math.Abs(r1.FrontBrickCount - r1.Position.Y) <= r1.JumpHeight)
                {
                    r1.Move();
                }

                if (rand.Next(100) > 50)
                {
                    r1.TurnRight();
                }
                else
                {
                    r1.TurnLeft();
                }
            }

            Console.WriteLine("JAAAAAAAA   " + (DateTime.Now - start).TotalSeconds);
        }
    }
}
