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
        /// <summary>
        /// Erstellt eine neue Instanz
        /// </summary>
        /// <param name="msg">Nachricht die in der Exception angezeigt wrden soll</param>
        public InvalidSizeException(string msg) : base(msg)
        { }
    }
}
