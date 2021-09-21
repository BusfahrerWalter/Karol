using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Karol.Core
{
    public struct Direction
    {
        private static readonly Direction[] Directions;

        public static readonly Direction North;
        public static readonly Direction Ost;
        public static readonly Direction South;
        public static readonly Direction East;

        public char DirectionChar { get; private set; }
        public string Name { get; private set; }
        private int Offset { get; set; }

        static Direction()
        {
            North = new Direction('N', "North", 0);
            Ost = new Direction('O', "Ost", 1);
            South = new Direction('S', "South", 2);
            East = new Direction('E', "East", 3);
            Directions = new Direction[] { North, Ost, South, East };
        }

        private Direction(char dir, string name, int offset)
        {
            DirectionChar = char.ToUpper(dir);
            Name = name;
            Offset = offset;
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
            return dir + -offset;
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
