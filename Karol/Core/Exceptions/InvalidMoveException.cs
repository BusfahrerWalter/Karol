using System;
using System.Collections.Generic;
using System.Text;

namespace Karol.Core.Exceptions
{
    public class InvalidMoveException : Exception
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
