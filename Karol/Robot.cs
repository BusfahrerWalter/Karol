using Karol.Core;
using Karol.Core.Annotations;
using Karol.Core.Exceptions;
using Karol.Core.Rendering;
using Karol.Core.WorldElements;
using Karol.Extensions;
using Karol.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Media;
using System.Runtime.CompilerServices;
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
        /// <summary>
        /// Standard Verzögerung
        /// </summary>
        public const int DefaultDelay = 300;
        /// <summary>
        /// Standard Sprunghöhe
        /// </summary>
        public const int DefaultJumpHeight = 1;
        /// <summary>
        /// Standard Rucksackgröße
        /// </summary>
        public const int DefaultBackpackSize = -1;
        /// <summary>
        /// Standard Ziegelfarbe
        /// </summary>
        public static Color DefaultPaint = Color.Red;

        private Direction _faceDirection = Direction.North;
        private bool _isVisible = true;
        private int _bricksInBackpack;
        private bool reloadData = true;
        private bool isMoving = false;
        private bool addToList = false;

        private Bitmap[] RoboterBitmaps { get; set; } = new Bitmap[4];
        private DateTime WaitStartTime { get; set; }
        internal Marker Mark { get; set; }
        internal override string Metadata
        {
            get => $"{FaceDirection.Offset}";
            set
            {
                if (int.TryParse(value, out int offset))
                    _faceDirection = Direction.FromOffset(offset);
            }
        }

        /// <summary>
        /// Event wird ausgelöst wenn der Roboter eine Marke betritt.
        /// </summary>
        internal event EventHandler onEnterMarkPreview;
        /// <summary>
        /// Event wird ausgelöst wenn der Roboter eine Marke verlässt.
        /// </summary>
        internal event EventHandler onLeaveMarkPreview;
        /// <summary>
        /// Event wird ausgelöst wenn der Roboter einen Ziegel platziert.
        /// </summary>
        internal event EventHandler onPlaceBrickPreview;
        /// <summary>
        /// Event wird ausgelöst wenn der Roboter einen Ziegel aufhebt.
        /// </summary>
        internal event EventHandler onPickUpBrickPreview;

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
        private bool CanMove
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
        /// Gibt zurück ob sich vor dem Roboter eine Wand bzw. ein Quader befindet.
        /// </summary>
        public bool HasWall
        {
            get
            {
                return HasWallInDirection(FaceDirection);
            }
        }
        /// <summary>
        /// Gibt zurück ob sich rechts neben dem Roboter eine Wand bzw. ein Quader befindet.
        /// </summary>
        public bool HasWallRight
        {
            get
            {
                return HasWallInDirection(FaceDirection + 1);
            }
        }
        /// <summary>
        /// Gibt zurück ob sich links neben dem Roboter eine Wand bzw. ein Quader befindet.
        /// </summary>
        public bool HasWallLeft
        {
            get
            {
                return HasWallInDirection(FaceDirection - 1);
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

                int stackSize = World.GetStackSize(facePos.X, facePos.Z);
                return World.HasCellAt(facePos.X, Math.Max(stackSize - 1, 0), facePos.Z, out WorldElement e) && e is Robot;
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

                int stackSize = World.GetStackSize(facePos.X, facePos.Z);
                if (World.GetCell(facePos.X, Math.Max(stackSize - 1, 0), facePos.Z) is Marker)
                    stackSize--;

                return stackSize;
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
        public bool IsFacingEast => FaceDirection == Direction.East;
        /// <summary>
        /// Schaut der Roboter gerade nach Süden
        /// </summary>
        public bool IsFacingSouth => FaceDirection == Direction.South;
        /// <summary>
        /// Schaut der Roboter gerade nach Westen
        /// </summary>
        public bool IsFacingWest => FaceDirection == Direction.West;
        /// <summary>
        /// Aktuelle Blickrichtung des Roboters
        /// </summary>
        public Direction FaceDirection
        {
            get => _faceDirection;
            internal set
            {
                _faceDirection = value;
                BitMap = IsVisible ? RoboterBitmaps[value.Offset] : ImageExtension.EmptyBitmap;
            }
        }
        #endregion
        #endregion

        #region Konstruktoren
        /// <summary>
        /// Parameterloser Konstruktor damit der Roboter automatisch erzeugt werden kann.
        /// </summary>
        internal Robot()
        {
            addToList = true;
        }

        /// <summary>
        /// Erstellt einen neuen Roboter
        /// </summary>
        internal Robot(int xStart, int zStart, World world, Direction initDir, bool updateView = true, bool placeInWorld = true)
        {
            _faceDirection = initDir;
            Position = new Position(xStart, world.GetStackSize(xStart, zStart), zStart);
            World = world;

            CheckStartPos();

            if (placeInWorld)
                world.SetCell(xStart, zStart, this, updateView);

            World.RobotCollection.Add(this);
        }

        /// <summary>
        /// Erstellt einen neuen Roboter.
        /// </summary>
        /// <param name="xStart">Start X Position des Roboters</param>
        /// <param name="zStart">Start Z Position des Roboters</param>
        /// <param name="world">Welt in der der Roboter leben soll</param>
        /// <param name="initialDirection">Start Blickrichtung des Roboters. <br></br>Standard ist Direction.North
        /// </param>
        /// <exception cref="InvalidActionException">Wird geworfen wenn der Roboter an einer Ungültigen Position platziert wird</exception>
        public Robot(int xStart, int zStart, World world, Direction initialDirection) 
            : this(xStart, zStart, world, initialDirection, true) { }

        /// <summary>
        /// Erstellt einen neuen Roboter.
        /// </summary>
        /// <param name="xStart">Start X Position des Roboters</param>
        /// <param name="zStart">Start Z Position des Roboters</param>
        /// <param name="world">Welt in der der Roboter leben soll</param>
        /// <exception cref="InvalidActionException">Wird geworfen wenn der Roboter an einer Ungültigen Position platziert wird</exception>
        public Robot(int xStart, int zStart, World world)
            : this(xStart, zStart, world, Direction.North) { }

        /// <summary>
        /// Erstellt einen neuen Roboter. An der Position 0, 0
        /// </summary>
        /// <param name="world">Welt in der der Roboter leben soll</param>
        /// <exception cref="InvalidActionException">Wird geworfen wenn der Roboter an einer Ungültigen Position platziert wird</exception>
        public Robot(World world) 
            : this(0, 0, world) { }

        /// <summary>
        /// Erstellt einen neuen Roboter. An der Position 0, 0
        /// </summary>
        /// <param name="world">Welt in der der Roboter leben soll</param>
        /// <param name="initialDirection">Start Blickrichtung des Roboters. <br></br>Standard ist Direction.North</param>
        /// <exception cref="InvalidActionException">Wird geworfen wenn der Roboter an einer Ungültigen Position platziert wird</exception>
        public Robot(World world, Direction initialDirection) 
            : this(0, 0, world, initialDirection) { }

        /// <summary>
        /// Erstellt einen neuen Roboter anhand der übergebennen Optionen
        /// </summary>
        /// <param name="options">Roboter Optionen</param>
        public Robot(RobotOptions options) 
            : this(options.StartX, options.StartZ, options) { }

        /// <summary>
        /// Erstellt einen neuen Roboter anhand der übergebennen Optionen
        /// </summary>
        /// <param name="options">Roboter Optionen</param>
        /// <param name="startX">X Start Position des Roboters</param>
        /// <param name="startZ">Z Start Posotion des Roboters</param>
        public Robot(int startX, int startZ, RobotOptions options)
        {
            _faceDirection = options.InitialDirection;
            Position = new Position(startX, options.World.GetStackSize(startX, startZ), startZ);
            World = options.World;
            Delay = options.Delay;

            CheckStartPos();

            if (options.NorthImage != null)
                RoboterBitmaps[0] = options.NorthImage;

            if (options.EastImage != null)
                RoboterBitmaps[1] = options.EastImage;

            if (options.SouthImage != null)
                RoboterBitmaps[2] = options.SouthImage;

            if (options.WestImage != null)
                RoboterBitmaps[3] = options.WestImage;

            if (options.OffsetX != 0)
                XOffset = options.OffsetX;

            if (options.OffsetY != 0)
                YOffset = options.OffsetY;

            BitMap = RoboterBitmaps[FaceDirection.Offset];
            World.SetCell(startX, startZ, this);
            World.RobotCollection.Add(this);
        }
        #endregion

        #region Util
        private void CheckStartPos()
        {
            var tPos = new Position(Position.X, Position.Y - 1, Position.Z);
            if (World.HasCellAt(tPos, out WorldElement e) || (e != null && !e.CanStackOnTop))
                throw new InvalidActionException($"An der gegebenen Position {Position} befindet sich bereits etwas! Oder auf das darunter liegende Objekt kann nicht gestapelt werden");
        }

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

        private bool HasWallInDirection(Direction dir)
        {
            Position facePos = dir.OffsetPosition(Position);
            if (!World.IsPositionValid(facePos))
                return true;

            int stackSize = World.GetStackSize(facePos.X, facePos.Z);
            int y = Math.Max(stackSize - 2, 0);

            return World.HasCellAt(facePos.X, y, facePos.Z, out WorldElement e) && e is Cube;
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

        internal override void OnDestroy()
        {
            if (isMoving)
                return;

            World.RobotCollection.Remove(this);
        }

        internal override void OnWorldSet()
        {
            if (!reloadData)
                return;

            Delay = DefaultDelay;
            JumpHeight = DefaultJumpHeight;
            MaxBackpackSize = DefaultBackpackSize;
            Paint = DefaultPaint;

            CanStackOnTop = false;
            CanPickUp = false;
            ViewColor2D = Color.Black;
            XOffset = -2;
            YOffset = -2;

            RoboterBitmaps = ResourcesLoader.LoadRobotBitmaps(World.RoboterCount);
            FaceDirection = _faceDirection;
            BitMap = RoboterBitmaps[FaceDirection.Offset];
            Identifier = World.RoboterCount;

            if(addToList)
                World.RobotCollection.Add(this);

            reloadData = false;
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
            World.Update(Position.X, Position.Z, this);
            Wait();
        }

        /// <summary>
        /// Dreht den Roboter um 90 grad nach Rechts
        /// </summary>
        public void TurnRight()
        {
            PrepareWait();
            FaceDirection += 1;
            World.Update(Position.X, Position.Z, this);
            Wait();
        }

        /// <summary>
        /// Lässt den Roboter einen Schritt nach vorne Machen
        /// </summary>
        /// <exception cref="InvalidMoveException">Wird geworfen wenn beim bewegen nach vorne ein Fehler auftritt</exception>
        public void Move()
        {
            PrepareWait();

            isMoving = true;
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
                    World.SetCell(Position, null);               
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
                    World.SetCell(Position, null, World.RenderingMode == WorldRenderingMode.Render2D); //TODO:
                    World.SetCell(newPos, this);
                    Position = newPos;
                }
            }
            
            Wait();
            isMoving = false;
        }

        /// <summary>
        /// Platziert einen Ziegel vor dem Roboter.
        /// </summary>
        /// <param name="paintOverride">Überschreibung der Standard Farbe für diesen Roboter.</param>
        /// <exception cref="InvalidActionException">Wird geworfen wenn kein Ziegel platziert werden kann</exception>
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
        /// <exception cref="InvalidActionException">Wird geworfen wenn kein Ziegel platziert werden kann</exception>
        public void Place()
        {
            Place(Paint);
        }

        /// <summary>
        /// Hebt den Ziegel vor dem Roboter auf.
        /// </summary>
        /// <exception cref="InvalidActionException">Wird geworfen wenn kein Ziegel aufgehoben werden kann</exception>
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
        /// <exception cref="InvalidActionException">Wird geworfen wenn keine Marke platziert werden kann</exception>
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
        /// <exception cref="InvalidActionException">Wird geworfen wenn keine Marke aufgehoben werden kann</exception>
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
        /// <exception cref="InvalidActionException">Wird geworfen wenn kein Quader platziert werden kann</exception>
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
        /// <exception cref="InvalidActionException">Wird geworfen wenn kein Quader aufgehoben werden kann</exception>
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
            SoundPlayer player = new SoundPlayer(Resources.RobotSound);
            player.Play();
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

        /// <summary>
        /// Gibt den Hashcode für dieses Objekt zurück
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return HashCode.Combine(Identifier);
        }
        #endregion

        #region Events
        internal void OnEnterMark()
        {
            onEnterMarkPreview?.Invoke(this, EventArgs.Empty);
        }

        internal void OnLeaveMark()
        {
            onLeaveMarkPreview?.Invoke(this, EventArgs.Empty);
        }

        internal void OnPlaceBrick()
        {
            onPlaceBrickPreview?.Invoke(this, EventArgs.Empty);
        }

        internal void OnPickUpBrick()
        {
            onPickUpBrickPreview?.Invoke(this, EventArgs.Empty);
        }
        #endregion
    }
}
