using Karol.Core;
using Karol.Core.Annotations;
using Karol.Core.Exceptions;
using Karol.Core.WorldElements;
using Karol.Extensions;
using Karol.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Media;
using System.Text;
using System.Threading;

namespace Karol
{
    /// <summary>
    /// Ein Roboter der sich in einer Welt bewegen und dort leben kann...
    /// </summary>
    [WorldElementInfo('R')]
    public class Robot : WorldElement
    {
        #region Properties / Felder
        private Direction _faceDirection = Direction.North;
        private bool _isVisible = true;
        private int _bricksInBackpack;

        private Bitmap[] RoboterBitmaps { get; set; }
        private DateTime WaitStartTime { get; set; }
        internal Marker Mark { get; set; }
        internal override string Metadata
        {
            get => $"{FaceDirection.Offset}";
            set
            {
                if (int.TryParse(value, out int offset))
                    FaceDirection += offset;
            }
        }

        public event EventHandler onEnterMark;
        public event EventHandler onLeaveMark;
        public event EventHandler onPlaceBrick;
        public event EventHandler onPickUpBrick;

        #region Anderes Public
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
        /// Gibt die nummer des Roboters zurück.
        /// </summary>
        public int Identifier { get; private set; }

        /// <summary>
        /// Sichtbarkeit des Roboters <br></br>
        /// Standard ist True
        /// </summary>
        public bool IsVisible
        {
            get => _isVisible;
            set
            {
                _isVisible = value;
                BitMap = value ? RoboterBitmaps[FaceDirection.Offset] : ImageExtension.EmptyBitmap;
                World.Update(Position.X, Position.Z, this);
            }
        }
        #endregion

        #region Rucksack
        /// <summary>
        /// Die Anzahl der Ziegel die sich im Rucksack befinden.
        /// </summary>
        public int BricksInBackpack
        {
            get => _bricksInBackpack;
            set
            {
                if (value > MaxBackpackSize && MaxBackpackSize != -1)
                    throw new InvalidOperationException($"Kann maximale Rucksack größe von {MaxBackpackSize} nicht überschreiben.");

                _bricksInBackpack = value;
            }
        }
        /// <summary>
        /// Maximale Rucksackgröße. Durch setzen von -1 wird die Rucksack funktion deaktiviert. <br></br>
        /// Standard ist -1
        /// </summary>
        public int MaxBackpackSize { get; set; }

        /// <summary>
        /// Gibt zurück ob der Rucksack voll ist. <br></br>
        /// Gibt immer false zurück wenn MaxBackpackSize = -1 ist.
        /// </summary>
        public bool IsBackpackFull
        {
            get => BricksInBackpack == MaxBackpackSize && MaxBackpackSize != -1;
        }

        /// <summary>
        /// Gibt zurück ob der Rucksack leer ist. <br></br>
        /// Gibt immer false zurück wenn MaxBackpackSize = -1 ist.
        /// </summary>
        public bool IsBackpackEmpty
        {
            get => BricksInBackpack == 0 && MaxBackpackSize != -1;
        }
        #endregion

        #region Abfragen auf andere Zellen
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
                if (cell is IContainer)
                    stackSize = Math.Max(stackSize - 1, 0);

                bool canClimb = Math.Abs(Position.Y - stackSize) <= JumpHeight && stackSize != World.SizeY;
                bool isEmptyContainer = cell is IContainer c && c.IsEmpty;
                return canClimb && (cell == null || cell.CanStackOnTop || isEmptyContainer);
            }
        }
        /// <summary>
        /// Gibt zurück ob sich der Roboter vor einer Wand bzw. ein Quader befindet.
        /// </summary>
        public bool HasWall
        {
            get
            {
                Position facePos = FaceDirection.OffsetPosition(Position);
                if (!World.IsPositionValid(facePos))
                    return true;

                return World.HasCellAt(facePos.X, facePos.Y, facePos.Z, out WorldElement e) && e is Cube;
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
        /// Gibt zurück ob sich der Roboter auf einer Marke befindet.
        /// </summary>
        public bool HasMark
        {
            get => Mark != null;
        }

        /// <summary>
        /// Gibt zurück ob sich vor dem Roboter ein Ziegel befindet
        /// </summary>
        public bool HasBrick
        {
            get => HasBrickInDirection(FaceDirection);
        }
        /// <summary>
        /// Gibt zurück ob sich links neben dem Roboter ein Ziegel befindet
        /// </summary>
        public bool HasBrickLeft
        {
            get => HasBrickInDirection(FaceDirection + 1);
        }
        /// <summary>
        /// Gibt zurück ob sich rechts neben dem Roboter ein Ziegel befindet
        /// </summary>
        public bool HasBrickRight
        {
            get => HasBrickInDirection(FaceDirection - 1);
        }

        /// <summary>
        /// Gibt die anzahl der Ziegel vor dem Roboter zurück.
        /// </summary>
        public int FrontBrickCount
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
        /// Gibt die nummer des Roboters vor dem Roboter zurück. <br></br>
        /// Gibt -1 zurück wenn sich vor dem Roboter kein anderer befindet.
        /// </summary>
        public int FrontRobotIdentifier
        {
            get
            {
                var pos = FaceDirection.OffsetPosition(Position);
                pos.Y = World.GetStackSize(pos.X, pos.Z) - 1;
                if (!World.IsPositionValid(pos))
                    return -1;

                var cell = World.GetCell(pos);
                if (!(cell is Robot robo))
                    return -1;

                return robo.Identifier;
            }
        }
        #endregion

        #region Direction
        /// <summary>
        /// Schaut der Roboter gerade nach Norden
        /// </summary>
        public bool IsFacingNorth => FaceDirection == Direction.North;
        /// <summary>
        /// Schaut der Roboter gerade nach Osten
        /// </summary>
        public bool IsFacingOst => FaceDirection == Direction.East;
        /// <summary>
        /// Schaut der Roboter gerade nach Süden
        /// </summary>
        public bool IsFacingSouth => FaceDirection == Direction.South;
        /// <summary>
        /// Schaut der Roboter gerade nach Westen
        /// </summary>
        public bool IsFacingEast => FaceDirection == Direction.West;
        /// <summary>
        /// Aktuelle Blickrichtung des Roboters
        /// </summary>
        public Direction FaceDirection
        {
            get => _faceDirection;
            internal set
            {
                _faceDirection = value;
                BitMap = IsVisible ? RoboterBitmaps[FaceDirection.Offset] : ImageExtension.EmptyBitmap;
                World.Update(Position.X, Position.Z, this);
            }
        }
        #endregion
        #endregion

        #region Konstruktoren
        /// <summary>
        /// Erstellt einen neuen Roboter
        /// </summary>
        internal Robot(int xStart, int zStart, World world, Direction initDir, bool updateView)
        {
            Position = new Position(xStart, world.GetStackSize(xStart, zStart), zStart);
            World = world;
            Delay = 300;
            JumpHeight = 1;
            MaxBackpackSize = -1;
            Paint = Color.Red;

            if (World.HasCellAt(Position, out WorldElement e) || (e != null && !e.CanStackOnTop))
                throw new InvalidActionException($"An der gegebenen Position {Position} befindet sich bereits etwas!");

            CanStackOnTop = false;
            CanPickUp = false;
            XOffset = -2;
            YOffset = -2;

            world.RoboterCount++;
            world.Robots.Add(this);

            RoboterBitmaps = ResourcesLoader.LoadRobotBitmaps(world.RoboterCount - 1);
            BitMap = RoboterBitmaps[FaceDirection.Offset];
            Identifier = world.RoboterCount;

            world.SetCell(xStart, zStart, this, updateView);
            world.OnRobotAdded(this);
        }

        /// <summary>
        /// Erstellt einen neuen Roboter.
        /// </summary>
        /// <param name="xStart">Start X Position des Roboters</param>
        /// <param name="zStart">Start Z Position des Roboters</param>
        /// <param name="world">Welt in der der Roboter leben soll</param>
        /// <exception cref="InvalidActionException"></exception>
        public Robot(int xStart, int zStart, World world) : this(xStart, zStart, world, Direction.North, true) { }

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

        /// <summary>
        /// Erstellt einen neuen Roboter. An der Position 0, 0
        /// </summary>
        /// <param name="world">Welt in der der Roboter leben soll</param>
        public Robot(World world) : this(0, 0, world) { }

        /// <summary>
        /// Erstellt einen neuen Roboter. An der Position 0, 0
        /// </summary>
        /// <param name="world">Welt in der der Roboter leben soll</param>
        /// <param name="initialDirection">Start Blickrichtung des Roboters. <br></br>Standard ist Direction.North
        public Robot(World world, Direction initialDirection) : this(0, 0, world, initialDirection) { }
        #endregion

        #region Util
        /// <summary>
        /// Lässt den Roboter warten.
        /// </summary>
        /// <param name="time">Dauer die der Roboter warten soll.</param>
        public void Wait(int time)
        {
            Thread.Sleep(time);
        }

        private void Wait()
        {
            int time = (int)(DateTime.Now - WaitStartTime).TotalMilliseconds;
            Thread.Sleep(Math.Max(Delay - time, 0));
        }

        private void PrepareWait()
        {
            WaitStartTime = DateTime.Now;
        }

        private bool HasBrickInDirection(Direction dir)
        {
            var pos = dir.OffsetPosition(Position);
            if (!World.IsPositionValid(pos))
                return false;

            return World.HasCellAt(pos, out WorldElement cell) && cell is Brick;
        }

        private void CheckFacePos(out Position facePos)
        {
            facePos = FaceDirection.OffsetPosition(Position);
            if (!World.IsPositionValid(facePos))
            {
                facePos.Y = 0;
                throw new InvalidActionException($"An der Position {facePos} kann kein Block platziert werden!");
            }

            int stackSize = World.GetStackSize(facePos.X, facePos.Z);
            var cell = World.GetCell(facePos.X, Math.Max(stackSize - 1, 0), facePos.Z);

            if (stackSize >= World.SizeY || (cell != null && !cell.CanStackOnTop))
            {
                facePos.Y = stackSize;
                throw new InvalidActionException($"An der Position {facePos} kann kein Block platziert werden!");
            }
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

            if (World.HasCellAt(newPos.X, newPos.Y - 1, newPos.Z, out WorldElement cell) && cell is Marker mark)
            {
                if (HasMark)
                {
                    Mark.Reset();
                    Mark = null;
                    OnLeaveMark();
                }
                else
                {
                    World.SetCell(Position, null, false);               
                }

                Mark = mark;
                Mark.Content = this;
                Position = Mark.Position;
                World.Update(Position.X, Position.Z, Mark);
                OnEnterMark();
            }
            else
            {
                if (HasMark)
                {
                    Mark.Reset();
                    Mark = null;
                    OnLeaveMark();

                    World.SetCell(newPos, this);
                    Position = newPos;
                }
                else
                {
                    World.SetCell(Position, null, false);
                    World.SetCell(newPos, this);
                    Position = newPos;
                }
            }
            
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

            CheckFacePos(out Position facePos);

            var newCell = World.AddToStack(facePos.X, facePos.Z, new Brick(paintOverride));
            BricksInBackpack--;
            World.Update(facePos.X, facePos.Z, newCell);

            OnPlaceBrick();
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
            if (!World.IsPositionValid(facePos))
                throw new InvalidActionException($"An der Position {facePos} kann kein Ziegel aufgehogen werden!");

            int stackSize = World.GetStackSize(facePos.X, facePos.Z);
            var cell = World.GetCell(facePos.X, Math.Max(stackSize - 1, 0), facePos.Z);

            if (cell == null || !cell.CanPickUp)
            {
                Wait();
                return;
            }

            BricksInBackpack++;
            World.SetCell(facePos.X, Math.Max(stackSize - 1, 0), facePos.Z, null, true);

            OnPickUpBrick();
            Wait();
        }

        /// <summary>
        /// Platziert eine Marke unter dem Roboter.
        /// </summary>
        /// <exception cref="InvalidActionException"></exception>
        public void PlaceMark()
        {
            PrepareWait();
            if (HasMark)
                throw new InvalidActionException($"Kann an Position {Position} keine Marke platzieren!");

            Mark = new Marker(this);
            World.SetCell(Position, Mark);
            OnEnterMark();
            Wait();
        }

        /// <summary>
        /// Hebt eine Marke unter dem Roboter auf.
        /// </summary>
        /// <exception cref="InvalidActionException"></exception>
        public void PickUpMark()
        {
            PrepareWait();
            if (!HasMark)
                throw new InvalidActionException($"Kann an Position {Position} keine Marke aufheben!");

            World.SetCell(Position, this);
            Mark = null;
            OnLeaveMark();
            Wait();
        }

        /// <summary>
        /// Platziert einen Quader vor dem Roboter
        /// </summary>
        public void PlaceCube()
        {
            PrepareWait();
            CheckFacePos(out Position facePos);

            var newCell = World.AddToStack(facePos.X, facePos.Z, new Cube());
            World.Update(facePos.X, facePos.Y, newCell);

            Wait();
        }

        /// <summary>
        /// Hebt einen Quader vor dem Roboter auf
        /// </summary>
        public void PickUpCube()
        {
            PrepareWait();

            var facePos = FaceDirection.OffsetPosition(Position);
            if (!World.IsPositionValid(facePos))
                throw new InvalidActionException($"An der Position {facePos} kann kein Quader aufgehogen werden!");

            int stackSize = World.GetStackSize(facePos.X, facePos.Z);
            int y = Math.Max(stackSize - 1, 0);
            var cell = World.GetCell(facePos.X, y, facePos.Z);

            if(cell is Dummy)
            {
                y--;
                cell = World.GetCell(facePos.X, y, facePos.Z);
            }

            if (!(cell is Cube))
            {
                Wait();
                return;
            }

            World.SetCell(facePos.X, y, facePos.Z, null, true);
            Wait();
        }

        /// <summary>
        /// Der Roboter gibt einen Piep-Ton aus
        /// </summary>
        public void MakeSound()
        {
            SystemSounds.Beep.Play();
        }

        /// <summary>
        /// Gibt den Zustand des Roboters als String aus.
        /// </summary>
        /// <returns>Zustand des Roboters</returns>
        public override string ToString()
        {
            return $"Robot {Identifier}: {Position} {FaceDirection}";
        }

        /// <summary>
        /// Prüft ob das übergebene Objekt dieser Roboter ist.
        /// </summary>
        /// <param name="obj">Anderes Objekt</param>
        /// <returns>True wenn sie gleich sind. Ansonsten False</returns>
        public override bool Equals(object obj)
        {
            return obj is Robot robot &&
                   Identifier == robot.Identifier;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Identifier);
        }
        #endregion

        #region Events
        internal void OnEnterMark()
        {
            onEnterMark?.Invoke(this, EventArgs.Empty);
        }

        internal void OnLeaveMark()
        {
            onLeaveMark?.Invoke(this, EventArgs.Empty);
        }

        internal void OnPlaceBrick()
        {
            onPlaceBrick?.Invoke(this, EventArgs.Empty);
        }

        internal void OnPickUpBrick()
        {
            onPickUpBrick?.Invoke(this, EventArgs.Empty);
        }
        #endregion
    }
}
