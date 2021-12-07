using Karol;
using Karol.Core;
using System;
using System.Drawing;
using Karol.Core.Rendering;
using System.IO;
using System.Text;

namespace TestApp
{
    class Program
    {
        public static World w => new World(10, 10, 10);

        static void Main(string[] args)
        {
            World w = new World(10, 10, 10);
            Robot r = new Robot(w, 5, 5);
            Random rand = new Random();
            Action[] acts = new Action[]
            {
                r.Move,
                r.TurnLeft,
                r.TurnRight,
                r.Place,
                r.PickUp
            };

            r.Delay = 10;
            r.JumpHeight = 100;

            while (!false)
            {
                int num = rand.Next(0, acts.Length);
                try
                {
                    acts[num].Invoke();
                }
                catch(Exception e) 
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}
