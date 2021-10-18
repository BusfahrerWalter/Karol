using Karol.Core.WorldElements;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Karol.Core.Rendering
{
    internal class WorldRendererLSD : WorldRenderer3D
    {
        public WorldRendererLSD(World world) : base(world)
        {
        }

        public override Point CellToPixelPos(int xPos, int yPos, int zPos, WorldElement element)
        {
            throw new NotImplementedException();
        }

        public override Bitmap DrawGrid()
        {
            throw new NotImplementedException();
        }

        public override void Redraw()
        {
            throw new NotImplementedException();
        }

        public override void Update(int xPos, int zPos, WorldElement newCell)
        {
            throw new NotImplementedException();
        }
    }
}
