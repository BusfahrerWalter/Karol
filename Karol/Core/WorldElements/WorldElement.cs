using Karol.Core.Annotations;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace Karol.Core.WorldElements
{
    /// <summary>
    /// Basisklasse für alle Objekte die in einer Welt sein sollen.
    /// </summary>
    public abstract class WorldElement
    {
        #region Properties & Felder
        private static Type[] ElementTypes { get; set; }

        private bool _canStackOnTop = true;
        private bool _isObstacle = true;
        private bool _canPickUp = true;
        private Position _position;
        private World _world;
        private Point _pixelPosition = new Point(-1, -1);

        /// <summary>
        /// Die Welt in der sich dieses Objekt befindet.
        /// </summary>
        public World World
        {
            get => _world;
            set
            {
                _world = value;
                OnWorldSet();
            }
        }

        /// <summary>
        /// Bild das Gerendert werden soll.
        /// </summary>
        internal Bitmap BitMap { get; set; }

        /// <summary>
        /// Rechteck das Position und Größe der Bitmap angibt.
        /// </summary>
        internal Rectangle Rect => new Rectangle(PixelPosition, new Size(BitMap.Width, BitMap.Height));

        /// <summary>
        /// Pixel Position dieses Elements
        /// </summary>
        internal Point PixelPosition
        {
            get
            {
                if (_pixelPosition.X == -1)
                    _pixelPosition = World.CellToPixelPos(Position, this);
                
                return _pixelPosition;
            }
        }

        /// <summary>
        /// Farbe in der dieses Element in 2D Dargestellt werden soll
        /// </summary>
        internal Color ViewColor2D { get; set; }

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
        /// ID zum Speichern und Laden
        /// </summary>
        internal char ID => GetInfo().ID;

        /// <summary>
        /// Metadaten zum Speichern und Laden
        /// </summary>
        internal virtual string Metadata
        {
            get => string.Empty;
            set { }
        }

        /// <summary>
        /// Aktuelle Position
        /// </summary>
        public Position Position
        {
            get => _position;
            set => _position = value;
        }
        #endregion

        #region Konstruktoren
        /// <summary>
        /// Erstellt ein neues World Element
        /// </summary>
        /// <param name="bitMap">Bild für das Element</param>
        public WorldElement(Bitmap bitMap)
        {
            BitMap = bitMap;
        }

        /// <summary>
        /// Erstellt ein neues World Element
        /// </summary>
        public WorldElement() : this(null) { }
        #endregion

        #region Methoden
        internal virtual void OnWorldSet() { }

        internal virtual void OnDestroy() { }

        internal WorldElementInfoAttribute GetInfo()
        {
            return GetType()
                .GetCustomAttributes(typeof(WorldElementInfoAttribute), true)
                .First() as WorldElementInfoAttribute;
        }

        /// <summary>
        /// Gibt das zu der ID gehörende World Element zurück. Funktioniert für Roboter nur mit Parametern!
        /// </summary>
        /// <param name="id">ID des World Elements</param>
        /// <returns>World Element</returns>
        internal static WorldElement ForID(char id)
        {
            if(ElementTypes == null)
            {
                ElementTypes = typeof(WorldElement).Assembly
                    .GetTypes()
                    .Where(t => t.IsSubclassOf(typeof(WorldElement)))
                    .ToArray();
            }

            var type = ElementTypes
                .Where(t => t.GetCustomAttribute<WorldElementInfoAttribute>().ID == id)
                .FirstOrDefault();

            if (type == default)
                return null;

            return (WorldElement)Activator.CreateInstance(type, true);
        }

        internal static void Destroy(WorldElement element, bool isTargetInGrid = true)
        {
            if (element == null)
                return;

            if (isTargetInGrid)
                element.World.SetCell(element.Position, null);
            else
                element.OnDestroy();
        }

        /// <summary>
        /// Gibt zurück ob sich der Pixel im bereich dieses Elements befindet.
        /// </summary>
        /// <param name="pixelWorldPos">Globale Position des Pixels</param>
        /// <returns>True wenn sich der Pixel im bereich des Elements befindet und nicht Transparent ist. Ansonsten False</returns>
        internal bool HasPixel(Point pixelWorldPos)
        {
            var pos = PixelPosition;
            int x = Math.Abs(pixelWorldPos.X - pos.X);
            int y = Math.Abs(pixelWorldPos.Y - pos.Y);

            if (x >= BitMap.Width || y >= BitMap.Height)
                return false;

            return BitMap.GetPixel(x, y).A == 255;
        }

        internal Bitmap GetDifferenceMask()
        {
            var cells = GetObstructingCells();
            if (cells.Count == 0)
                return BitMap;

            var pixelPos = PixelPosition;
            var newMap = new Bitmap(BitMap);

            for(int x = 0; x < BitMap.Width; x++)
            {
                for(int y = 0; y < BitMap.Height; y++)
                {
                    foreach(var cell in cells)
                    {
                        Point pixel = new Point(pixelPos.X + x, pixelPos.Y + y);
                        if (!cell.HasPixel(pixel))
                            continue;

                        newMap.SetPixel(x, y, Color.Transparent);
                        break;
                    }
                }
            }

            return newMap;
        }

        private IList<WorldElement> GetObstructingCells()
        {
            var list = new List<WorldElement>();

            AddNotNull(GetNeighbor(new Position(1, 0, 0)));
            AddNotNull(GetNeighbor(new Position(0, 0, -1)));
            AddNotNull(GetNeighbor(new Position(0, 1, 0)));
            AddNotNull(GetNeighbor(new Position(1, 0, -1)));
            AddNotNull(GetNeighbor(new Position(0, -1, -1)));

            //AddPath(new Position(Position.X + 1, Position.Y, Position.Z));
            //AddPath(new Position(Position.X, Position.Y, Position.Z - 1));
            //AddPath(new Position(Position.X + 1, Position.Y, Position.Z - 1));

            foreach (var r in World.RobotCollection.Where(r => r != this && (r.Position.Z < Position.Z || r.Position.X > Position.X)))
            {
                AddNotNull(r);
            }

            return list;

            #pragma warning disable CS8321 // Die lokale Funktion ist deklariert, wird aber nie verwendet.
            void AddPath(Position start)
            #pragma warning restore CS8321 // Die lokale Funktion ist deklariert, wird aber nie verwendet.
            {
                while (World.IsPositionValid(start))
                {
                    WorldElement cell = World.GetCell(start);
                    AddNotNull(cell);

                    start.X += 1;
                    start.Y += 1;
                    start.Z -= 1;
                }
            }

            void AddNotNull(WorldElement e)
            {
                if (e != null)
                    list.Add(e);
            }
        }

        internal WorldElement GetNeighbor(Position offset)
        {
            Position newPos = Position + offset;
            if (!World.IsPositionValid(newPos))
                return null;

            return World.GetCell(newPos);
        }
        #endregion
    }
}
