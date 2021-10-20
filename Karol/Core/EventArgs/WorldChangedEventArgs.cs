using Karol.Core.WorldElements;
using System;
using System.Collections.Generic;
using System.Text;

namespace Karol.Core
{
    /// <summary>
    /// Event Argumente wenn sich die Welt geändert hat
    /// </summary>
    public class WorldChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Neues Element das Platziert wurde. Null wen ein Element gelöscht wurde.
        /// </summary>
        public WorldElement NewElement { get; set; }

        /// <summary>
        /// Erstellt eine neue Instanz von WorldChangedEventArgs
        /// </summary>
        /// <param name="element">Neues Element</param>
        public WorldChangedEventArgs(WorldElement element)
        {
            NewElement = element;
        }
    }
}
