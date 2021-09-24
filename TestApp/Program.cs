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
            //w1.PlaceRandomBricks(10, 4);
            
            Robot r1 = new Robot(0, 0, w1);
            r1.Delay = 10; 
            
            for(int i = 0; i < 5; i++)
            {
                r1.Move();
                Console.WriteLine(r1.Position + " | " + r1.FaceDirection);
                r1.Move();
                Console.WriteLine(r1.Position + " | " + r1.FaceDirection);
                r1.TurnRight();
                Console.WriteLine(r1.Position + " | " + r1.FaceDirection);
            }
        }
    }
}
