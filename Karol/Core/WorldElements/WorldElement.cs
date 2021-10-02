using System.Drawing;

namespace Karol.Core.WorldElements
{
    /// <summary>
    /// Basisklasse für alle Objekte die in einer Welt sein sollen.
    /// </summary>
    public abstract class WorldElement
    {
        private bool _canStackOnTop = true;
        private bool _isObstacle = true;
        private bool _canPickUp = true;
        private Position _position;

        /// <summary>
        /// Die Welt in der sich dieses Objekt befindet.
        /// </summary>
        public World World { get; internal set; }

        /// <summary>
        /// Bild das Gerendert werden soll.
        /// </summary>
        internal Bitmap BitMap { get; set; }

        /// <summary>
        /// Rechteck das Position und Größe der Bitmap angibt.
        /// </summary>
        internal Rectangle Rect => new Rectangle(World.CellToPixelPos(Position, this), new Size(BitMap.Width, BitMap.Height));

        /// <summary>
        /// X Offset für das Bild
        /// </summary>
        internal int XOffset { get; set; }

        /// <summary>
        /// Y Offset für das Bild
        /// </summary>
        internal int YOffset { get; set; }

        /// <summary>
        /// Gibt an ob auf diesem Element andere gestapelt werden können oder nicht. <br></br>
        /// Standard ist True.
        /// </summary>
        internal bool CanStackOnTop
        {
            get => _canStackOnTop;
            set => _canStackOnTop = value;
        }

        /// <summary>
        /// Gibt an ob dieses World element ein Hindernis ist oder nicht <br></br>
        /// Standard ist True.
        /// </summary>
        internal bool IsObstacle
        {
            get => _isObstacle;
            set => _isObstacle = value;
        }

        /// <summary>
        /// Gibt an ob dieses World element aufgehoben werden kann oder nicht. <br></br>
        /// Standrad ist True.
        /// </summary>
        internal bool CanPickUp
        {
            get => _canPickUp;
            set => _canPickUp = value;
        }

        /// <summary>
        /// Aktuelle Position
        /// </summary>
        public Position Position
        {
            get => _position;
            set => _position = value;
        }

        public WorldElement(Bitmap bitMap)
        {
            BitMap = bitMap;
        }

        public WorldElement() : this(null) { }
    }
}
