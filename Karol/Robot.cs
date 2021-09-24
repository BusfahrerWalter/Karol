using Karol.Core;
using Karol.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading;

namespace Karol
{
    public class Robot : WorldElement
    {
        private Direction _faceDirection = Direction.North;

        private Bitmap[] RoboterBitmaps { get; set; }

        /// <summary>
        /// Die Welt in der dieser Roboter lebt
        /// </summary>
        public World World { get; private set; }

        /// <summary>
        /// Gibt an wie hoch der Roboter Springen kann. (in Zellen)
        /// </summary>
        public int JumpHeight { get; set; }

        /// <summary>
        /// Die Verzögerung in Millisekunden zwischen 2 Aktionen des Roboters. <br></br>
        /// Standard ist 300
        /// </summary>
        public int Delay { get; set; }

        /// <summary>
        /// Aktuelle Blickrichtung des Roboters
        /// </summary>
        public Direction FaceDirection
        {
            get => _faceDirection;
            set
            {
                _faceDirection = value;
                BitMap = RoboterBitmaps[FaceDirection.Offset];
                World.Update(Position.X, Position.Z, this);
            }
        }

        /// <summary>
        /// Erstellt einen neuen Roboter.
        /// </summary>
        /// <param name="xStart">Start X Position des Roboters</param>
        /// <param name="zStart">Start Z Position des Roboters</param>
        /// <param name="world">Welt in der der Roboter leben soll</param>
        public Robot(int xStart, int zStart, World world) 
        {
            Position = new Position(xStart, 0, zStart);
            World = world;
            Delay = 300;

            CanStackOnTop = false;
            XOffset = -2;
            YOffset = -2;

            world.RoboterCount++;
            RoboterBitmaps = ResourcesLoader.LoadRobotBitmaps(world.RoboterCount - 1);
            BitMap = RoboterBitmaps[FaceDirection.Offset];

            world.SetCell(xStart, zStart, this);
        }

        /// <summary>
        /// Erstellt einen neuen Roboter.
        /// </summary>
        /// <param name="xStart">Start X Position des Roboters</param>
        /// <param name="zStart">Start Z Position des Roboters</param>
        /// <param name="world">Welt in der der Roboter leben soll</param>
        /// <param name="initialDirection">Start Blickrichtung des Roboters. <br></br>Standard ist Direction.North
        /// </param>
        public Robot(int xStart, int zStart, World world, Direction initialDirection) : this(xStart, zStart, world)
        {
            FaceDirection = initialDirection;
        }

        #region Util
        private void Wait()
        {
            Thread.Sleep(Delay);
        }
        #endregion

        #region Public
        /// <summary>
        /// Dreht den Roboter um 90 grad nach Links
        /// </summary>
        public void TurnLeft()
        {
            FaceDirection -= 1;
            Wait();
        }

        /// <summary>
        /// Dreht den Roboter um 90 grad nach Rechts
        /// </summary>
        public void TurnRight()
        {
            FaceDirection += 1;
            Wait();
        }

        public void Move()
        {
            if (!World.IsPositionValid(Position.X, Position.Y, Position.Z))
            {
                return;
            }

            Position newPos = FaceDirection.OffsetPosition(Position);
            newPos.Y = World.GetStackSize(newPos.X, newPos.Z);

            World.SetCell(Position.X, Position.Y, Position.Z, null, false);
            World.SetCell(newPos.X, newPos.Y, newPos.Z, this);
            Position = newPos;

            Wait();
        }
        #endregion
    }
}
