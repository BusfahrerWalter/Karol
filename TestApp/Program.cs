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

            World w2 = World.Load(@$"H:\Daten\Fächer\java\lib\karol\labs\lab10.kdw");
        }
    }
}
