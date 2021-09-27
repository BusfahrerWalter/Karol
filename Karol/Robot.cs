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
        #region Properties / Felder
        private Direction _faceDirection = Direction.North;

        private Bitmap[] RoboterBitmaps { get; set; }
        private DateTime WaitStartTime { get; set; }

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
        /// Die Farbe in der die von diesem Roboter platzierten Ziegel angemalt werden.
        /// Standard ist Rot
        /// </summary>
        public Color Paint { get; set; }

        /// <summary>
        /// Die Anzahl der Ziegel die sich im Rucksack befinden.
        /// </summary>
        public int BricksInBackpack { get; private set; }
        /// <summary>
        /// Maximale Rucksackgröße. Durch setzen von -1 wird die Rucksack funktion deaktiviert. <br></br>
        /// Standard ist -1
        /// </summary>
        public int MaxBackpackSize { get; set; }

        /// <summary>
        /// Gibt zurück ob der Roboter einen Schritt nach vorne machen kann oder nicht.
        /// </summary>
        public bool CanMove
        {
            get
            {
                Position facePos = FaceDirection.OffsetPosition(Position);
                if (!World.IsPositionValid(facePos))
                    return false;

                int stackSize = World.GetStackSize(facePos.X, facePos.Z);
                var cell = World.GetCell(facePos.X, Math.Max(stackSize - 1, 0), facePos.Z);
                bool canClimb = Math.Abs(Position.Y - stackSize) <= JumpHeight && stackSize != World.SizeY;

                return canClimb && (cell == null || cell.CanStackOnTop);
            }
        }
        /// <summary>
        /// Gibt zurück ob sich vor dem Roboter eine Wand oder ein Ziegel befindet.
        /// </summary>
        public bool HasWall
        {
            get
            {
                Position facePos = FaceDirection.OffsetPosition(Position);
                if (!World.IsPositionValid(facePos))
                    return true;

                return World.HasCellAt(facePos.X, facePos.Y, facePos.Z, out WorldElement e) && e is Brick;
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
                    return false;

                return World.HasCellAt(facePos.X, facePos.Y, facePos.Z, out WorldElement e) && e is Robot;
            }
        }
        /// <summary>
        /// Gibt die anzahl der Ziegel vor dem Roboter zurück.
        /// </summary>
        public int BricksInFront
        {
            get
            {
                Position facePos = FaceDirection.OffsetPosition(Position);
                if (!World.IsPositionValid(facePos))
                    return 0;

                return World.GetStackSize(facePos.X, facePos.Z);
            }
        }
        /// <summary>
        /// Gibt die Farbe des höchsten Ziegels vor dem Roboter zurück. <br></br>
        /// Wenn sich vor dem Roboter kein Ziegel befindet wird Color.Transparent zurück gegeben.
        /// </summary>
        public Color BrickColor
        {
            get
            {
                Position facePos = FaceDirection.OffsetPosition(Position);
                if (!World.IsPositionValid(facePos))
                    return Color.Transparent;

                int stackSize = World.GetStackSize(facePos.X, facePos.Z);
                var cell = World.GetCell(facePos.X, Math.Max(0, stackSize - 1), facePos.Z);
                if (cell is Brick brick)
                    return brick.Paint;

                return Color.Transparent;
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
            internal set
            {
                _faceDirection = value;
                BitMap = RoboterBitmaps[FaceDirection.Offset];
                World.Update(Position.X, Position.Z, this);
            }
        }
        #endregion

        #region Konstruktoren
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
            MaxBackpackSize = -1;
            Paint = Color.Red;

            CanStackOnTop = false;
            CanPickUp = false;
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
        #endregion

        #region Util
        private void Wait()
        {
            int time = (int)(DateTime.Now - WaitStartTime).TotalMilliseconds;
            Thread.Sleep(Math.Max(Delay - time, 0));
        }

        private void PrepareWait()
        {
            WaitStartTime = DateTime.Now;
        }
        #endregion

        #region Public
        /// <summary>
        /// Dreht den Roboter um 90 grad nach Links
        /// </summary>
        public void TurnLeft()
        {
            PrepareWait();
            FaceDirection -= 1;
            Wait();
        }

        /// <summary>
        /// Dreht den Roboter um 90 grad nach Rechts
        /// </summary>
        public void TurnRight()
        {
            PrepareWait();
            FaceDirection += 1;
            Wait();
        }

        /// <summary>
        /// Lässt den Roboter einen Schritt nach vorne Machen
        /// </summary>
        /// <exception cref="InvalidMoveException"></exception>
        public void Move()
        {
            PrepareWait();

            Position newPos = FaceDirection.OffsetPosition(Position);
            if (!World.IsPositionValid(newPos))
                throw new InvalidMoveException(Position, newPos);

            newPos.Y = World.GetStackSize(newPos.X, newPos.Z);
            if (HasRobot)
                throw new InvalidMoveException(Position, newPos, "Auf einer Position kann sich maximal ein Roboter befinden!");

            if (!CanMove)
                throw new InvalidMoveException(Position, newPos, $"Ziel {newPos} liegt außerhalb der Sprunghöhe!");

            World.SetCell(Position.X, Position.Y, Position.Z, null, false);
            World.SetCell(newPos.X, newPos.Y, newPos.Z, this);
            Position = newPos;

            Wait();
        }

        /// <summary>
        /// Platziert einen Ziegel vor dem Roboter.
        /// </summary>
        /// <param name="paintOverride">Überschreibung der Standard Farbe für diesen Roboter.</param>
        /// <exception cref="InvalidActionException"></exception>
        public void Place(Color paintOverride)
        {
            PrepareWait();

            if (MaxBackpackSize != -1 && BricksInBackpack <= 0)
                throw new InvalidActionException($"Kann keine Ziegel mehr platzieren. Rucksack ist leer!");

            Position facePos = FaceDirection.OffsetPosition(Position);
            if (!World.IsPositionValid(facePos))
            {
                facePos.Y = 0;
                throw new InvalidActionException($"An der Position {facePos} kann kein Ziegel platziert werden!");
            }

            int stackSize = World.GetStackSize(facePos.X, facePos.Z);
            var cell = World.GetCell(facePos.X, Math.Max(stackSize - 1, 0), facePos.Z);

            if (stackSize >= World.SizeY || (cell != null && !cell.CanStackOnTop))
            {
                facePos.Y = stackSize;
                throw new InvalidActionException($"An der Position {facePos} kann kein Ziegel platziert werden!");
            }

            var newCell = World.AddToStack(facePos.X, facePos.Z, new Brick(paintOverride));
            BricksInBackpack--;
            World.Update(facePos.X, facePos.Z, newCell);

            Wait();
        }

        /// <summary>
        /// Platziert einen Ziegel vor dem Roboter.
        /// </summary>
        /// <exception cref="InvalidActionException"></exception>
        public void Place()
        {
            Place(Paint);
        }

        /// <summary>
        /// Hebt den Ziegel vor dem Roboter auf.
        /// </summary>
        /// <exception cref="InvalidActionException"></exception>
        public void PickUp()
        {
            PrepareWait();

            if (MaxBackpackSize != -1 && BricksInBackpack == MaxBackpackSize)
                throw new InvalidActionException($"Kann keine Ziegel mehr aufheben. Maximale Rucksackgröße von {MaxBackpackSize} erreicht!");

            var facePos = FaceDirection.OffsetPosition(Position);
            int stackSize = World.GetStackSize(facePos.X, facePos.Z);
            var cell = World.GetCell(facePos.X, Math.Max(stackSize - 1, 0), facePos.Z);

            if (cell == null || !cell.CanPickUp)
            {
                Wait();
                return;
            }

            BricksInBackpack++;
            World.SetCell(facePos.X, Math.Max(stackSize - 1, 0), facePos.Z, null, true);

            Wait();
        }
        #endregion
    }
}
