using System;
using System.Collections.Generic;
using System.Text;

namespace Karol.Core.Exceptions
{
    /// <summary>
    /// Wird geworfen wenn die größe der Welt ungültig ist.
    /// </summary>
    public class InvalidSizeException : KarolException
    {
        public InvalidSizeException(string msg) : base(msg)
        { }
    }
}
