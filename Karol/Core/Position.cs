using System;
using System.Collections.Generic;
using System.Text;

namespace Karol.Core
{
    /// <summary>
    /// Gibt eine Position im 3D Raum an.
    /// </summary>
    public struct Position
    {
        public static Position Zero;
        public static Position One;

        public int X { get; internal set; }
        public int Y { get; internal set; }
        public int Z { get; internal set; }

        static Position()
        {
            Zero = new Position(0, 0, 0);
            One = new Position(1, 1, 1);
        }

        public Position(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Position(Position pos)
        {
            X = pos.X;
            Y = pos.Y;
            Z = pos.Z;
        }

        public override string ToString()
        {
            return $"({X}, {Y}, {Z})";
        }

        public override bool Equals(object obj)
        {
            return obj is Position position &&
                   X == position.X &&
                   Y == position.Y &&
                   Z == position.Z;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y, Z);
        }

        public static Position operator -(Position p1, Position p2)
        {
            return new Position(p1.X - p2.X, p1.Y - p2.Y, p1.Z - p2.Z);
        }

        public static Position operator +(Position p1, Position p2)
        {
            return new Position(p1.X + p2.X, p1.Y + p2.Y, p1.Z + p2.Z);
        }

        public static Position operator *(Position p1, int i)
        {
            return new Position(p1.X * i, p1.Y * i, p1.Z * i);
        }

        public static Position operator /(Position p1, int i)
        {
            return new Position(p1.X / i, p1.Y / i, p1.Z / i);
        }

        public static bool operator ==(Position p1, Position p2)
        {
            return p1.Equals(p2);
        }

        public static bool operator !=(Position p1, Position p2)
        {
            return !p1.Equals(p2);
        }
    }
}
