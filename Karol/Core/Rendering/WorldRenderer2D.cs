using Karol.Core.WorldElements;
using Karol.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Karol.Core.Rendering
{
    [RendererInfo(WorldRenderingMode.Render2D)]
    internal class WorldRenderer2D : Renderer
    {
        public const int EdgeLength = 40;
        public static Size CellSize = new Size(EdgeLength - 1, EdgeLength - 1);

        private int Height => EdgeLength * SizeZ;
        private int Width => EdgeLength * SizeX;

        public WorldRenderer2D(World world) 
            : base(world) { }

        public override Point CellToPixelPos(int xPos, int yPos, int zPos, WorldElement element)
        {
            return new Point(xPos * EdgeLength + 1, Height - ((zPos + 1) * EdgeLength - 1));
        }

        public override Bitmap DrawGrid()
        {
            TopLeft = new Point(0, 0);
            TopRight = new Point(Width, 0);
            BottomLeft = new Point(0, Height);
            BottomRight = new Point(Width, Height);

            Bitmap map = new Bitmap(Width + 1, Height + 1);

            map.DrawPath(Color.Blue, true, TopLeft, TopRight, BottomRight, BottomLeft);

            for(int x = 1; x < SizeX; x++)
            {
                Point start = TopLeft;
                Point end = BottomLeft;

                start.X += EdgeLength * x;
                end.X = start.X;

                map.DrawLine(start, end, Color.Blue);
            }

            for (int y = 1; y < SizeZ; y++)
            {
                Point start = BottomLeft;
                Point end = BottomRight;

                start.Y -= EdgeLength * y;
                end.Y = start.Y;

                map.DrawLine(start, end, Color.Blue);
            }

            return map;
        }

        public override void Redraw()
        {
            Bitmap map = (Bitmap)BlockMap.Image;
            Graphics g = Graphics.FromImage(map);

            map.Clear();
            
            for (int x = 0; x < SizeX; x++)
            {
                for (int z = 0; z < SizeZ; z++)
                {
                    int stackSize = World.GetStackSize(x, z);
                    int y = Math.Max(stackSize - 1, 0);
                    if (!World.HasCellAt(x, y, z, out WorldElement e))
                        continue;

                    Rectangle rect = new Rectangle(CellToPixelPos(e.Position, null), CellSize);
                    g.FillRectangle(new SolidBrush(e.ViewColor2D), rect);
                    g.DrawString(stackSize.ToString(), new Font(FontFamily.GenericSansSerif, 16), new SolidBrush(e.ViewColor2D.Invert()), rect);
                }
            }

            g.Flush();
            BlockMap.Invalidate();
            BlockMap.Update();
        }

        public override void Update(int xPos, int zPos, WorldElement newCell)
        {
            Bitmap map = (Bitmap)BlockMap.Image;
            Graphics g = Graphics.FromImage(map);
            int stackSize = World.GetStackSize(xPos, zPos);
            
            if(newCell == null)
            {
                Point pos = CellToPixelPos(xPos, 0, zPos, newCell);
                Rectangle r = new Rectangle(pos, CellSize);

                if (stackSize == 0)
                {
                    map.Clear(r);
                    BlockMap.Invalidate(r);
                    BlockMap.Update();
                }
                else
                {
                    var cell = World.GetCell(xPos, stackSize - 1, zPos);
                    Update(xPos, zPos, cell);
                }

                return;
            }

            Rectangle rect = new Rectangle(CellToPixelPos(newCell.Position, null), CellSize);

            map.Clear(rect);
            g.FillRectangle(new SolidBrush(newCell.ViewColor2D), rect);
            g.DrawString(stackSize.ToString(), new Font(FontFamily.GenericSansSerif, 16), 
                new SolidBrush(newCell.ViewColor2D.Invert()), rect);

            g.Flush();
            BlockMap.Invalidate(rect);
            BlockMap.Update();
        }
    }
}
