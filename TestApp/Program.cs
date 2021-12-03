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
            string str = "C_Gartenzaun_Karol_World\n" +
                         "Size: 3,2,3\n---\nB _ R(2)\nQ Q\n_ Q Q\n---\nB\n";

            MemoryStream stream = new MemoryStream(Encoding.ASCII.GetBytes(str));
            World world = World.Load(stream);
            Robot robo = world.GetRobot(0);
            Controller.Create(robo);
        }
    }
}
