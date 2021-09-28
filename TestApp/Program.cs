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
            World w1 = new World(5, 5, 5);
            Robot r1 = new Robot(0, 0, w1);
        }
    }
}
