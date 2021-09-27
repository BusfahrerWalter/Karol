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
        public Position CurrentPosition { get; set; }
        public Position TargetPosition { get; set; }

        public InvalidMoveException(Position current, Position target) 
            : base($"Bewegung auf Position {target} nicht möglich!")
        {
            CurrentPosition = current;
            TargetPosition = target;
        }

        public InvalidMoveException(Position current, Position target, string message)
            : base(message)
        {
            CurrentPosition = current;
            TargetPosition = target;
        }
    }
}
