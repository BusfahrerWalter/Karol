using Karol;
using Karol.Core;
using System;
using System.Drawing;
using Karol.Core.Rendering;
namespace TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            //World W = World.Load(@"C:\Users\danie\Desktop\KarolMap - 5.cskw");

            World W = new World(10, 5, 7);
            W.RenderingMode = WorldRenderingMode.Render2D;
            Robot R1 = new Robot(7, 4, W);
            R1.Delay = 700;
            R1.BricksInBackpack = 5;
            R1.MaxBackpackSize = 10;
            R1.Move();
            R1.TurnLeft();
            for (int i = 0; i < 3; i++)
            {
                R1.Move();
            }
            R1.TurnRight();
            for (int i = 0; i < 2; i++)
            {
                R1.TurnRight();
            }
            for (int i = 0; i < 2; i++)
            {
                R1.Place(Color.Blue);
            }
            R1.TurnLeft();
            R1.Move();
            R1.TurnRight();
            R1.Place(Color.Green);
            R1.Move();
            R1.TurnRight();
            R1.Move();
            Robot R2 = new Robot(3, 5, W);
            R2.Delay = 500;
            R2.BricksInBackpack = 5;
            R2.MaxBackpackSize = 10;
            R2.Move();
            R2.TurnRight();
            for (int i = 0; i < 3; i++)
            {
                R2.Move();
            }
            R2.TurnRight();
            for (int i = 0; i < 2; i++)
            {
                R2.Move();
            }
            R2.TurnRight();
            for (int i = 0; i < 1; i++)
            {
                R2.Move();
            }
        }    
    }
}
