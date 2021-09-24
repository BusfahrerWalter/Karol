using System;
using System.Collections.Generic;
using System.Text;

namespace Karol.Core.Exceptions
{
    public class InvalidActionException : Exception
    {
        public InvalidActionException(string msg) : base(msg) { }
    }
}
