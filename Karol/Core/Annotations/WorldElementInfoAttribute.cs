using System;
using System.Collections.Generic;
using System.Text;

namespace Karol.Core.Annotations
{
    internal class WorldElementInfoAttribute : Attribute
    {
        public char ID { get; private set; }
        public bool IncludeInEditor { get; private set; }

        public WorldElementInfoAttribute(char iD) : this(iD, true) { }

        public WorldElementInfoAttribute(char iD, bool includeInEditor)
        {
            ID = iD;
            IncludeInEditor = includeInEditor;
        }
    }
}
