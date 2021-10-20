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
            World w1 = new World(100, 100, 100);
            w1.PlaceRandomBricks(10);

            var map = new Bitmap(@"E:\Files\Schule\Schule 12\TrFi\C_Gartenzaun_Karol\Karol\Karol\Resources\Images\Freddy.png");
            RobotOptions options = new RobotOptions(w1)
            {
                StartX = 0,
                StartZ = 0,
                Set = ImageSet.Default
            };

            Robot freddy = new Robot(options);
            freddy.Delay = 40;


            SuperRobot r = new SuperRobot(3, 3, new RobotOptions(w1) { Set = ImageSet.Default });
            r.Delay = 40;

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
