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
            w1.PlaceRandomBricks(44, 2);


            Robot r1 = new Robot(0, 0, w1);
            Robot r3 = new Robot(2, 2, w1);
            r1.Delay = 1;
            r3.Delay = 1;
            r1.JumpHeight = 10;
            r3.JumpHeight = 10;
            Thread.Sleep(1000);

            for (int i = 0; i < 1000; i++)
            {
                Console.WriteLine(r1.CanMove);
                if (!r1.CanMove)
                {
                    r1.TurnRight();
                }
                else
                {
                    r1.Move();
                }

                if (!r3.CanMove)
                {
                    r3.TurnLeft();
                }
                else
                {

                r3.Move();
                }
                
            }
        }
    }
}
