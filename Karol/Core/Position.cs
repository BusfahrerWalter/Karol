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
        public int X { get; internal set; }
        public int Y { get; internal set; }
        public int Z { get; internal set; }

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
    }
}
