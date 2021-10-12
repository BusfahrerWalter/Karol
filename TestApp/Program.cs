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
            //World w1 = World.Load(@"C:\Users\damuelle\Desktop\map2.cskw");
            //for (int i = 0; i < 9; i++)
            //{
            //    World w2 = World.Load(@$"H:\Daten\Fächer\java\lib\karol\labs\lab{i}.kdw");
            //}

            //World w2 = World.Load(@$"H:\Daten\Fächer\java\lib\karol\labs\lab0.1.kdw");

            //World w1 = World.Load(@"C:\Users\damuelle\Desktop\KarolMap - 1.cskw");
            //Robot r1 = new Robot(w1);

            World w1 = new World(5, 5, 5);
            Robot r1 = new Robot(0, 0, w1);

            r1.Wait(1000);
            r1.Move();
        }
    }
}
