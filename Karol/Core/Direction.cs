using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Karol.Core
{
    /// <summary>
    /// Gibt eine der folgenden Richtung an: <br></br>
    /// Norden, Süden, Osten oder Westen
    /// </summary>
    public struct Direction
    {
        private static readonly Direction[] Directions;

        /// <summary>
        /// Richtung Norden (Oben)
        /// </summary>
        public static readonly Direction North;
        /// <summary>
        /// Richtung Osten (Rechts)
        /// </summary>
        public static readonly Direction East;
        /// <summary>
        /// Richtung Süden (Unten)
        /// </summary>
        public static readonly Direction South;
        /// <summary>
        /// Richtung Westen (Links)
        /// </summary>
        public static readonly Direction West;

        internal const char NorthChar = 'N';
        internal const char EastChar = 'E';
        internal const char SouthChar = 'S';
        internal const char WestChar = 'W';

        /// <summary>
        /// Character der die Richtung angibt.
        /// </summary>
        public char DirectionChar { get; private set; }
        /// <summary>
        /// Name der Richtung
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// Offset der Richtung. Norden = 0
        /// </summary>
        public int Offset { get; internal set; }

        static Direction()
        {
            North = new Direction(NorthChar, "North", 0);
            East = new Direction(EastChar, "East", 1);
            South = new Direction(SouthChar, "South", 2);
            West = new Direction(WestChar, "West", 3);
            Directions = new Direction[] { North, East, South, West };
        }

        private Direction(char dir, string name, int offset)
        {
            DirectionChar = char.ToUpper(dir);
            Name = name;
            Offset = offset;
        }

        /// <summary>
        /// Verschiebt eine Position um das angegebene Offset in die aktuelle richtung.
        /// Verändert die übergebene Position nicht.
        /// </summary>
        /// <param name="pos">Position die verschoben werden soll</param>
        /// <param name="offset">Zahl um wie viel die Position verschoben werden soll</param>
        /// <returns>Neue und verchobene Position</returns>
        public Position OffsetPosition(Position pos, int offset = 1)
        {
            Position newPos = new Position(pos);
            switch (DirectionChar)
            {
                case NorthChar: newPos.Z += offset; break;
                case WestChar: newPos.X -= offset; break;
                case SouthChar: newPos.Z -= offset; break;
                case EastChar: newPos.X += offset; break;
            }

            return newPos;
        }

        /// <summary>
        /// Gibt die zum Offset passende Richtung zurück. <br></br>
        /// Offset 0 = Norden
        /// </summary>
        /// <param name="offset">Offset aus dem die Richtung besitmmt werden soll</param>
        /// <returns>Zum Offset passende Richtung</returns>
        public static Direction FromOffset(int offset)
        {
            return North + offset;
        }

        public override string ToString()
        {
            return $"{Name} ({DirectionChar})";
        }

        public override bool Equals(object obj)
        {
            return obj is Direction direction &&
                   DirectionChar == direction.DirectionChar;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(DirectionChar);
        }

        public static Direction operator +(Direction dir, int offset)
        {
            int newDir = (offset + dir.Offset) % Directions.Length;
            return Directions[newDir];
        }

        public static Direction operator -(Direction dir, int offset)
        {
            return dir + (Directions.Length - offset);
        }

        public static bool operator ==(Direction dir, object obj)
        {
            return dir.Equals(obj);
        }

        public static bool operator !=(Direction dir, object obj)
        {
            return !dir.Equals(obj);
        }
    }
}
