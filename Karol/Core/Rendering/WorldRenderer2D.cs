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
        public static int EdgeLength = 30;
        public static Size CellSize = new Size(EdgeLength - 1, EdgeLength - 1);
        public static Bounds Padding = new Bounds(10, 0, 0, 0);
        private static readonly Font Font = new Font(FontFamily.GenericSansSerif, 10);
        private static readonly StringFormat Format = new StringFormat(StringFormatFlags.NoWrap | StringFormatFlags.NoClip);

        private int Height => EdgeLength * SizeZ;
        private int Width => EdgeLength * SizeX;

        public WorldRenderer2D(World world) 
            : base(world) { }

        public override Point CellToPixelPos(int xPos, int yPos, int zPos, WorldElement element)
        {
            return new Point(xPos * EdgeLength + 1 + Padding.Left, Height - ((zPos + 1) * EdgeLength - 1 - Padding.Top));
        }

        public override Bitmap DrawGrid()
        {
            TopLeft = new Point(Padding.Left, Padding.Top);
            TopRight = new Point(Width + Padding.Left, Padding.Top);
            BottomLeft = new Point(Padding.Left, Height + Padding.Top);
            BottomRight = new Point(Width + Padding.Left, Height + Padding.Top);

            Bitmap map = new Bitmap(Width + Padding.Horizontal + 1, Height + Padding.Vertical + 1);
            Graphics g = Graphics.FromImage(map);
            Pen pen = new Pen(Brushes.Blue);

            g.DrawPolygon(pen, new Point[] { TopLeft, TopRight, BottomRight, BottomLeft, TopLeft });

            for(int x = 1; x < SizeX; x++)
            {
                Point start = TopLeft;
                Point end = BottomLeft;

                start.X += EdgeLength * x;
                end.X = start.X;

                g.DrawLine(pen, start, end);
            }

            for (int y = 1; y < SizeZ; y++)
            {
                Point start = BottomLeft;
                Point end = BottomRight;

                start.Y -= EdgeLength * y;
                end.Y = start.Y;

                g.DrawLine(pen, start, end);
            }

            g.Flush();
            return map;
        }

        public override void Redraw()
        {
            Bitmap map = (Bitmap)BlockMap.Image;
            Graphics g = Graphics.FromImage(map);

            g.Clear(Color.Transparent);
            
            for (int x = 0; x < SizeX; x++)
            {
                for (int z = 0; z < SizeZ; z++)
                {
                    int stackSize = World.GetStackSize(x, z);
                    int y = Math.Max(stackSize - 1, 0);
                    if (!World.HasCellAt(x, y, z, out WorldElement e))
                        continue;

                    DrawCell(stackSize, e, g);
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
            DrawCell(stackSize, newCell, g);

            g.Flush();
            BlockMap.Invalidate(rect);
            BlockMap.Update();
        }

        private void DrawCell(int stackSize, WorldElement e, Graphics g, bool clear = true)
        {
            Rectangle rect = new Rectangle(CellToPixelPos(e.Position, null), CellSize);
            SizeF textSize = g.MeasureString(stackSize.ToString(), Font);
            Brush textBrush;

            if (e.Info2D.DrawSolidColor)
            {
                var c = (Color)e.Info2D.FillColor;
                g.FillRectangle(new SolidBrush(c), rect);
                textBrush = new SolidBrush(c.Invert());
            }
            else
            {
                if(clear)
                    g.FillRectangle(Brushes.White, rect);

                if(e.Position.Y > 0)
                {
                    var cellBelow = World.GetCell(e.Position.X, e.Position.Y - 1, e.Position.Z);
                    DrawCell(stackSize, cellBelow, g);
                }

                g.DrawImage(e.Info2D.Image, rect);
                textBrush = Brushes.White;     
            }

            if (e is IContainer container && !container.IsEmpty)
            {
                DrawCell(stackSize, container.Content, g, false);
            }

            rect.X += rect.Width / 2 - (int)(textSize.Width / 2);
            rect.Y += rect.Height / 2 - (int)(textSize.Height / 2);
            g.DrawString(stackSize.ToString(), Font, textBrush, rect, Format);
        }
    }
}
