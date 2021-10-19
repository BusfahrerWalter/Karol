using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Karol.Core
{
    /// <summary>
    /// Enthält einstellungen aus denen ein neuer Roboter erzeugt werden kann.
    /// </summary>
    public class RobotOptions
    {
        /// <summary>
        /// Überschreibt die standard Textur des Roboters wenn er nach Norden schaut.
        /// </summary>
        public Bitmap NorthImage { get; set; }
        /// <summary>
        /// Überschreibt die standard Textur des Roboters wenn er nach Osten schaut.
        /// </summary>
        public Bitmap EastImage { get; set; }
        /// <summary>
        /// Überschreibt die standard Textur des Roboters wenn er nach Süden schaut.
        /// </summary>
        public Bitmap SouthImage { get; set; }
        /// <summary>
        /// Überschreibt die standard Textur des Roboters wenn er nach Westen schaut.
        /// </summary>
        public Bitmap WestImage { get; set; }

        /// <summary>
        /// Welt in der der Roboter leben soll
        /// </summary>
        public World World { get; private set; }
        /// <summary>
        /// Richtung in die der Roboter am anfang schauen soll <br></br>
        /// Standard ist Norden
        /// </summary>
        public Direction InitialDirection { get; set; }

        /// <summary>
        /// Start X Position des Roboters
        /// </summary>
        public int StartX { get; set; }
        /// <summary>
        /// Start Z Position des Roboters
        /// </summary>
        public int StartZ { get; set; }

        /// <summary>
        /// X Offset um das die Bitmap beim Rendern der Welt verschoben wird.
        /// </summary>
        public int OffsetX { get; set; }
        /// <summary>
        /// Y Offset um das die Bitmap beim Rendern der Welt verschoben wird.
        /// </summary>
        public int OffsetY { get; set; }

        /// <summary>
        /// Erzeugt eine neue Instantz der RobotOptions Klasse
        /// </summary>
        public RobotOptions(World world) 
            : this(0, 0, world) { }

        /// <summary>
        /// Erzeugt eine neue Instantz der RobotOptions Klasse
        /// </summary>
        /// <param name="startX">Start X Position</param>
        /// <param name="startZ">Start Z Position</param>
        /// <param name="world">Zielwelt</param>
        public RobotOptions(int startX, int startZ, World world)
        {
            World = world;
            StartX = startX;
            StartZ = startZ;
            InitialDirection = Direction.North;
        }

        public Bitmap[] GetImages()
        {
            return new Bitmap[]
            {
                NorthImage, EastImage, SouthImage, WestImage
            };
        }
    }
}
