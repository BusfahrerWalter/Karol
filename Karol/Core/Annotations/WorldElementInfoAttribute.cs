using System;
using System.Collections.Generic;
using System.Text;

namespace Karol.Core.Annotations
{
    internal class WorldElementInfoAttribute : Attribute
    {
        /// <summary>
        /// ID des beschriebenen Elements (für Speichern und Laden)
        /// </summary>
        public char ID { get; private set; }

        /// <summary>
        /// Gibt an ob das beschriebene Element im Editor platziert werden kann
        /// </summary>
        public bool IncludeInEditor { get; private set; }

        /// <summary>
        /// Gibt an ob das beschriebene Element beim Laden gecasht werden kann
        /// </summary>
        public bool IsCasheable { get; set; } = true;

        public WorldElementInfoAttribute(char iD) : this(iD, true) { }

        public WorldElementInfoAttribute(char iD, bool includeInEditor)
        {
            ID = iD;
            IncludeInEditor = includeInEditor;
        }
    }
}
