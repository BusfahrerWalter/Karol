using System;

namespace Karol.Core.Exceptions
{
    /// <summary>
    /// Basisklasse für alle Karol Exceptions
    /// </summary>
    public abstract class KarolException : Exception
    {
        /// <summary>
        /// Erstellt eine neue Instanz
        /// </summary>
        public KarolException() { }

        /// <summary>
        /// Erstellt eine neue Instanz
        /// </summary>
        /// <param name="msg">Nachricht die in der Exception angezeigt wrden soll</param>
        public KarolException(string msg) : base(msg) { }
    }
}
