using System;

namespace Karol.Core.Exceptions
{
    /// <summary>
    /// Basisklasse für alle Karol Exceptions
    /// </summary>
    public abstract class KarolException : Exception
    {
        public KarolException() { }

        public KarolException(string msg) : base(msg) { }
    }
}
