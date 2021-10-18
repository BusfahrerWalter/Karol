using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Karol.Core.Annotations;
using Karol.Properties;

namespace Karol.Core.WorldElements
{
    [WorldElementInfo('Q')]
    internal class Cube : WorldElement
    {
        private bool isDummyDead;

        public Cube() : base(Resources.Quader)
        {
            CanPickUp = true;
            CanStackOnTop = false;
            ViewColor2D = Color.DarkGray;
        }

        internal override void OnWorldSet()
        {
            var pos = new Position(Position.X, Position.Y + 1, Position.Z);
            if (!World.IsPositionValid(pos))
                return;

            var dummy = new Dummy(false, CanStackOnTop, () =>
            {
                isDummyDead = true;
                Destroy(this);
            })
            {
                ViewColor2D = ViewColor2D
            };

            World.SetCell(pos, dummy, false);
        }

        internal override void OnDestroy()
        {
            if (isDummyDead)
                return;

            var topPos = new Position(Position.X, Position.Y + 1, Position.Z);
            if (!World.IsPositionValid(topPos))
                return;

            World.SetCell(topPos, null);
        }
    }
}
