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
        /// <summary>
        /// Gleich (0, 0, 0)
        /// </summary>
        public static readonly Position Zero;
        /// <summary>
        /// Gleich (1, 1, 1)
        /// </summary>
        public static readonly Position One;

        /// <summary>
        /// X Koordinate dieser Position
        /// </summary>
        public int X { get; internal set; }
        /// <summary>
        /// Y Koordinate dieser Position
        /// </summary>
        public int Y { get; internal set; }
        /// <summary>
        /// Z Koordinate dieser Position
        /// </summary>
        public int Z { get; internal set; }

        static Position()
        {
            Zero = new Position(0, 0, 0);
            One = new Position(1, 1, 1);
        }

        /// <summary>
        /// Erstellt eine neue Position.
        /// </summary>
        /// <param name="x">X Koordinate</param>
        /// <param name="y">Y Koordinate</param>
        /// <param name="z">Z Koordinate</param>
        public Position(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        /// <summary>
        /// Erstellt eine neue Position anhand einer anderen.
        /// </summary>
        /// <param name="pos">Andere Position</param>
        public Position(Position pos)
        {
            X = pos.X;
            Y = pos.Y;
            Z = pos.Z;
        }

        /// <summary>
        /// Wandelt die Position in einen String um
        /// </summary>
        /// <returns>Position als String</returns>
        public override string ToString()
        {
            return $"({X}, {Y}, {Z})";
        }

        /// <summary>
        /// Vergleicht die Position mit einem anderen Objekt
        /// </summary>
        /// <param name="obj">Anderes Objekt</param>
        /// <returns>True wenn sie gleich sind. Ansonsten False</returns>
        public override bool Equals(object obj)
        {
            return obj is Position position &&
                   X == position.X &&
                   Y == position.Y &&
                   Z == position.Z;
        }

        /// <summary>
        /// Gibt den Hash Code für dieses Objekt zurück
        /// </summary>
        /// <returns>Hash Code für dieses Objekt</returns>
        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y, Z);
        }

        /// <summary>
        /// Subtrahiert zwei Positionen voneinander. 
        /// </summary>
        /// <param name="p1">Position 1</param>
        /// <param name="p2">Position 2</param>
        /// <returns>Position die sich aus der Subtraktion ergibt</returns>
        public static Position operator -(Position p1, Position p2)
        {
            return new Position(p1.X - p2.X, p1.Y - p2.Y, p1.Z - p2.Z);
        }

        /// <summary>
        /// Addiert 2 Positionen zusammen
        /// </summary>
        /// <param name="p1">Position 1</param>
        /// <param name="p2">Position 2</param>
        /// <returns>Position die sich aus der Addition ergibt</returns>
        public static Position operator +(Position p1, Position p2)
        {
            return new Position(p1.X + p2.X, p1.Y + p2.Y, p1.Z + p2.Z);
        }

        /// <summary>
        /// Multipliziert eine Position mit einer ganzen Zahl
        /// </summary>
        /// <param name="p1">Zu Multiplizierende Position</param>
        /// <param name="i">Multiplikator</param>
        /// <returns>Position die sich aus der Multiplikation ergibt</returns>
        public static Position operator *(Position p1, int i)
        {
            return new Position(p1.X * i, p1.Y * i, p1.Z * i);
        }

        /// <summary>
        /// Dividiert eine Position mit einer ganzen Zahl
        /// </summary>
        /// <param name="p1">Zu Dividierende Position</param>
        /// <param name="i">Divisor</param>
        /// <returns>Position die sich aus der Division ergibt</returns>
        public static Position operator /(Position p1, int i)
        {
            return new Position(p1.X / i, p1.Y / i, p1.Z / i);
        }

        /// <summary>
        /// Vergleicht 2 Positionen miteinander
        /// </summary>
        /// <param name="p1">Position 1</param>
        /// <param name="p2">Position 2</param>
        /// <returns>True wenn sie gleich sind. Ansonsten False</returns>
        public static bool operator ==(Position p1, Position p2)
        {
            return p1.Equals(p2);
        }

        /// <summary>
        /// Vergleicht 2 Positionen miteinander
        /// </summary>
        /// <param name="p1">Position 1</param>
        /// <param name="p2">Position 2</param>
        /// <returns>True wenn sie ungleich sind. Ansonsten False</returns>
        public static bool operator !=(Position p1, Position p2)
        {
            return !p1.Equals(p2);
        }
    }
}
