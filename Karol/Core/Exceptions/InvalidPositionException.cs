using System;
using System.Collections.Generic;
using System.Text;

namespace Karol.Core.Exceptions
{
    /// <summary>
    /// Wird geworfen wenn mit einer Ungültigen Position interagiert wird.
    /// </summary>
    public class InvalidPositionException : KarolException
    {
        /// <summary>
        /// Betroffene Position
        /// </summary>
        public Position Position { get; set; }

        /// <summary>
        /// Erstellt eine neue Instanz
        /// </summary>
        /// <param name="pos">Position mit der Interagiert wurde.</param>
        public InvalidPositionException(Position pos) : base($"Position {pos} ist nicht gültig.") 
        {
            Position = pos;
        }

        /// <summary>
        /// Erstellt eine neue Instanz
        /// </summary>
        /// <param name="msg">Nachricht</param>
        /// <param name="pos">Position mit der Interagiert wurde.</param>
        public InvalidPositionException(string msg, Position pos) : base(msg)
        {
            Position = pos;
        }
    }
}
