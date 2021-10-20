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
        /// <summary>
        /// Erstellt eine neue Instanz
        /// </summary>
        /// <param name="msg">Nachricht die in der Exception angezeigt wrden soll</param>
        public InvalidActionException(string msg) : base(msg) { }
    }
}
