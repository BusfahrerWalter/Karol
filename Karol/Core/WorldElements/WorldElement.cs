using Karol.Core.Annotations;
using System;
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
        private static Type[] ElementTypes { get; set; }

        private bool _canStackOnTop = true;
        private bool _isObstacle = true;
        private bool _canPickUp = true;
        private Position _position;

        /// <summary>
        /// Bild das Gerendert werden soll.
        /// </summary>
        internal Bitmap BitMap { get; set; }

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

        internal WorldElementInfoAttribute GetInfo()
        {
            return GetType()
                .GetCustomAttributes(typeof(WorldElementInfoAttribute), true)
                .First() as WorldElementInfoAttribute;
        }

        /// <summary>
        /// Gibt das zu der ID gehörende World Element zurück. Funktioniert nicht für Roboter!
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

            return (WorldElement)Activator.CreateInstance(type);
        }
    }
}
