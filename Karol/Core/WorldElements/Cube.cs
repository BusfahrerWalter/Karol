using System;
using System.Collections.Generic;
using System.Text;
using Karol.Core.Annotations;
using Karol.Properties;

namespace Karol.Core.WorldElements
{
    [WorldElementInfo('Q')]
    public class Cube : WorldElement
    {
        public Cube() : base(Resources.Quader)
        {
            CanPickUp = false;
            CanStackOnTop = true;
        }

        internal override void OnWorldSet()
        {
            var pos = new Position(Position.X, Position.Y + 1, Position.Z);
            if (!World.IsPositionValid(pos))
                return;

            World.SetCell(pos, new Dummy(false, true), false);
        }
    }
}
