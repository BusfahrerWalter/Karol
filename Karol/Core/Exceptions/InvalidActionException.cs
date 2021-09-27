using System;
using System.Collections.Generic;
using System.Text;

namespace Karol.Core.Exceptions
{
    /// <summary>
    /// Wird geworfen wenn eine Ungültige aktion ausgeführt wird.
    /// </summary>
    public class InvalidActionException : KarolException
    {
        public InvalidActionException(string msg) : base(msg) { }
    }
}
