using System;
using System.Collections.Generic;
using System.Text;

namespace Karol.Core.Exceptions
{
    /// <summary>
    /// Wird geworfen wenn ein Roboter eine Ungültige bewegung ausführt.
    /// </summary>
    public class InvalidMoveException : KarolException
    {
        /// <summary>
        /// Aktuelle Position des Roboters
        /// </summary>
        public Position CurrentPosition { get; set; }
        /// <summary>
        /// Position auf die sich der Roboter bewegen wollte
        /// </summary>
        public Position TargetPosition { get; set; }

        /// <summary>
        /// Erstellt eine neue Instanz
        /// </summary>
        /// <param name="current">Position des Roboters</param>
        /// <param name="target">Position auf die sich der Roboter bewegen wollte</param>
        public InvalidMoveException(Position current, Position target) 
            : base($"Bewegung auf Position {target} nicht möglich!")
        {
            CurrentPosition = current;
            TargetPosition = target;
        }

        /// <summary>
        /// Erstellt eine neue Instanz
        /// </summary>
        /// <param name="current">Position des Roboters</param>
        /// <param name="target">Position auf die sich der Roboter bewegen wollte</param>
        /// <param name="message">Nachricht die in der Exception angezeigt wrden soll</param>
        public InvalidMoveException(Position current, Position target, string message)
            : base(message)
        {
            CurrentPosition = current;
            TargetPosition = target;
        }
    }
}
