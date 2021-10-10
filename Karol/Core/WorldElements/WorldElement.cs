using Karol.Core.Annotations;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
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

        internal char ID => GetInfo().ID;

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
        public WorldElement(Bitmap bitMap)
        {
            BitMap = bitMap;
        }

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
        internal static WorldElement ForID(char id, params object[] parameter)
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

            return (WorldElement)Activator.CreateInstance(type, parameter);
        }

        /// <summary>
        /// Gibt zurück ob sich der Pixel im bereich dieses Elements befindet.
        /// </summary>
        /// <param name="pixelWorldPos">Globale Position des Pixels</param>
        /// <returns>True wenn sich der Pixel im bereich des Elements befindet und nicht Transparent ist. Ansonsten False</returns>
        internal bool HasPixel(Point pixelWorldPos)
        {
            var pos = World.CellToPixelPos(Position, this);
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

            var pixelPos = World.CellToPixelPos(Position, this);
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

            foreach (var r in World.Robots.Where(r => r != this && (r.Position.Z < Position.Z || r.Position.X > Position.X)))
            {
                AddNotNull(r);
            }

            return list;

            void AddPath(Position start)
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
