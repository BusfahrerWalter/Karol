using Karol.Core;
using Karol.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Karol
{
    public class Robot : WorldElement
    {
        private static readonly Bitmap[] RoboterBitmaps = ResourcesLoader.LoadRoboters();
        private static int RoboterCount { get; set; }

        /// <summary>
        /// Die Welt in der dieser Roboter lebt
        /// </summary>
        public World World { get; set; }
        /// <summary>
        /// Aktuelle X Position des Roboters
        /// </summary>
        public int PositionX { get; set; }
        /// <summary>
        /// Aktuelle Z Position des Roboters
        /// </summary>
        public int PositionZ { get; set; }

        public Robot(int xStart, int zStart, World world) : base(RoboterBitmaps[RoboterCount])
        {
            PositionX = xStart;
            PositionZ = zStart;
            World = world;
            RoboterCount++;

            world.SetCell(xStart, zStart, this);
        }
    }
}
