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
            //World.LoadImage(@"C:\Users\danie\Pictures\LINDWURM.jpg");
            World w1 = new World(40, 88, 40);

            RobotOptions o = new RobotOptions(w1)
            {
                StartX = 0,
                StartZ = 0,
                Delay = 10,
                Set = ImageSet.Default
            };

            SuperRobot r1 = new SuperRobot(o);
            DateTime start = DateTime.Now;

            r1.Wait(10000);

            while (r1.Position.Y < w1.Height - 1)
            {
                if (r1.HasWall)
                {
                    r1.TurnRight();

                    if (r1.Position.X == 0 && r1.Position.Z == 0)
                    {
                        Console.WriteLine((DateTime.Now - start).TotalSeconds);
                        start = DateTime.Now;
                    }
                }
                else
                {
                    r1.Place();
                    r1.Move();
                }
            }










            //World w1 = new World(20, 20, 20);

            //RobotOptions options = new RobotOptions(w1)
            //{
            //    StartX = 0,
            //    StartZ = 0,
            //    Set = ImageSet.Default
            //};

            //Robot freddy = new Robot(options);
            //freddy.Delay = 40;


            //SuperRobot r = new SuperRobot(2, 3, new RobotOptions(w1) { Set = ImageSet.Default });
            //r.Delay = 40;

            //while (true)
            //{
            //    if (r.HasWall || r.HasRobot || r.FrontBrickCount > r.JumpHeight)
            //    {
            //        r.TurnRight();
            //    }

            //    r.Move();
            //}
        }
    }
}
