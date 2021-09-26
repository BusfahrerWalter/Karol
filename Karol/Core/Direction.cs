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

        public static readonly Direction North;
        public static readonly Direction Ost;
        public static readonly Direction South;
        public static readonly Direction East;

        internal const char NorthChar = 'N';
        internal const char OstChar = 'O';
        internal const char SouthChar = 'S';
        internal const char EastChar = 'E';

        public char DirectionChar { get; private set; }
        public string Name { get; private set; }
        public int Offset { get; set; }

        static Direction()
        {
            North = new Direction(NorthChar, "North", 0);
            Ost = new Direction(OstChar, "Ost", 1);
            South = new Direction(SouthChar, "South", 2);
            East = new Direction(EastChar, "East", 3);
            Directions = new Direction[] { North, Ost, South, East };
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
                case OstChar: newPos.X += offset; break;
                case SouthChar: newPos.Z -= offset; break;
                case EastChar: newPos.X -= offset; break;
            }

            return newPos;
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
