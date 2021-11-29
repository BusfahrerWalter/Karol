using Karol;
using Karol.Core;
using Karol.Core.Rendering;
using System;
using System.Drawing;
using System.Threading;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            World w = new World(20, 5, 20);
            w.RenderingMode = WorldRenderingMode.Render2D;

            for (int i = 0; i < 20; i++)
            {
                Robot r = new Robot(i, 0, w);
            }

            while (!false)
            {
                Thread.Sleep(10);
                w.RenderingMode = w.RenderingMode == WorldRenderingMode.Render2D ? WorldRenderingMode.Render3D : WorldRenderingMode.Render2D;
            }
        }
    }
}
