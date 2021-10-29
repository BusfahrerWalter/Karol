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
            World w = World.LoadImage(@"H:\Daten\sachen\fehler.PNG");
            World w1 = World.Load(@"C:\Users\damuelle\Desktop\Brams\KarolWorld23.cskw");
        }
    }
}
