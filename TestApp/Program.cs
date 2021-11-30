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
            World w = new World(10, 5, 10);
            Robot r = new Robot(1, 1, w);
        }
    }
}
