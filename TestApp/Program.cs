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
        static void Main(string[] args)
        {
            //World w = World.Load(@"H:\Daten\Fächer\java\lib\karol\labs\lab8.kdw");
            World w = new World(30, 10, 30);

            for(int i = 0; i < w.Width; i++)
            {
                for(int j = 0; j < w.Depth; j++)
                {
                    Robot r = new Robot(w, i, j);
                }
            }

            while (true)
            {
                Console.ReadLine();
                foreach (var r in w.Robots)
                {
                    r.IsVisible = !r.IsVisible;
                }
            }
        }
    }
}
