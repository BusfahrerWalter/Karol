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

            w1.SetCell(1, 0);
            w1.SetCell(0, 0);
            w1.SetCell(0, 1);
        }
    }
}
