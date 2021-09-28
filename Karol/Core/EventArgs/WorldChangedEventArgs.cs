using Karol.Core.WorldElements;
using System;
using System.Collections.Generic;
using System.Text;

namespace Karol.Core
{
    public class WorldChangedEventArgs : EventArgs
    {
        public WorldElement NewElement { get; set; }

        public WorldChangedEventArgs(WorldElement element)
        {
            NewElement = element;
        }
    }
}
