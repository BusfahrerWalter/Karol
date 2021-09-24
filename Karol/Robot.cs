using Karol.Core;
using Karol.Core.Exceptions;
using Karol.Core.WorldElements;
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
        /// Gibt an wie hoch der Roboter Springen kann. (in Zellen) <br></br>
        /// Standard ist 1
        /// </summary>
        public int JumpHeight { get; set; }

        /// <summary>
        /// Die Verzögerung in Millisekunden zwischen 2 Aktionen des Roboters. <br></br>
        /// Standard ist 300
        /// </summary>
        public int Delay { get; set; }

        /// <summary>
        /// Gibt zurück ob sich vor dem Roboter ein Hindernis befindet, das er nicht überwinden kann.
        /// </summary>
        public bool CanMove
        {
            get
            {
                Position facePos = FaceDirection.OffsetPosition(Position);
                if (!World.IsPositionValid(facePos))
                    return false;

                bool hasObstacle = World.HasCellAt(facePos.X, facePos.Y, facePos.Z, out WorldElement e) && e.IsObstacle;
                bool canClimbWall = Math.Abs(Position.Y - World.GetStackSize(facePos.X, facePos.Z)) <= JumpHeight;

                return !hasObstacle || canClimbWall;
            }
        }
        /// <summary>
        /// Gibt zurück ob sich vor dem Roboter ein anderer Roboter befindet.
        /// </summary>
        public bool HasRobot
        {
            get
            {
                Position facePos = FaceDirection.OffsetPosition(Position);
                if (!World.IsPositionValid(facePos))
                    return true;

                return World.HasCellAt(facePos.X, facePos.Y, facePos.Z, out WorldElement e) && e is Robot;
            }
        }

        /// <summary>
        /// Schaut der Roboter gerade nach Norden
        /// </summary>
        public bool IsFacingNorth => FaceDirection == Direction.North;
        /// <summary>
        /// Schaut der Roboter gerade nach Osten
        /// </summary>
        public bool IsFacingOst => FaceDirection == Direction.Ost;
        /// <summary>
        /// Schaut der Roboter gerade nach Süden
        /// </summary>
        public bool IsFacingSouth => FaceDirection == Direction.South;
        /// <summary>
        /// Schaut der Roboter gerade nach Westen
        /// </summary>
        public bool IsFacingEast => FaceDirection == Direction.East;

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
            JumpHeight = 1;

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

        /// <summary>
        /// Lässt den Roboter einen Schritt nach vorne Machen
        /// </summary>
        /// <exception cref="InvalidMoveException"></exception>
        public void Move()
        {
            Position newPos = FaceDirection.OffsetPosition(Position);
            if (!World.IsPositionValid(newPos))
                throw new InvalidMoveException(Position, newPos);

            newPos.Y = World.GetStackSize(newPos.X, newPos.Z);
            if (Math.Abs(Position.Y - newPos.Y) > JumpHeight)
                throw new InvalidMoveException(Position, newPos, $"Ziel {newPos} liegt außerhalb der Sprunghöhe!");

            World.SetCell(Position.X, Position.Y, Position.Z, null, false);
            World.SetCell(newPos.X, newPos.Y, newPos.Z, this);
            Position = newPos;

            Wait();
        }
        #endregion
    }
}
