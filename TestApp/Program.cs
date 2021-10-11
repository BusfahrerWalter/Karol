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
            World w1 = World.Load(@"C:\Users\damuelle\Desktop\map2.cskw");
            World w2 = World.Load(@"H:\Daten\Fächer\java\lib\karol\labs\lab0.kdw");
        }
    }
}
