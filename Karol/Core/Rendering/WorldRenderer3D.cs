using Karol.Core.WorldElements;
using Karol.Extensions;
using Karol.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Karol.Core.Rendering
{
    [RendererInfo(WorldRenderingMode.Render3D)]
    internal class WorldRenderer3D : Renderer
    {
        private const int PixelWidth = 30;
        private const int PixelHeight = 15;
        private readonly Bounds Padding = new Bounds(30, 0, 50, 0);

        /// <summary>
        /// Versatz von zeile zu zeile in Pixeln
        /// </summary>
        private int LineOffset => PixelWidth / 2;

        public WorldRenderer3D(World world)
            : base(world) { }

        public override void Redraw()
        {
            var g = Graphics.FromImage(BlockMap.Image);
            g.Clear(Color.Transparent);

            for (int x = 0; x < SizeX; x++)
            {
                for (int z = SizeZ - 1; z >= 0; z--)
                {
                    for (int y = 0; y < SizeY; y++)
                    {
                        if (!World.HasCellAt(x, y, z, out WorldElement cell))
                            continue;

                        var pos = World.CellToPixelPos(x, y, z, cell);
                        g.DrawImage(cell.BitMap, pos);
                    }
                }
            }

            g.Flush();
            BlockMap.Invalidate();
            BlockMap.Update();
        }

        public override void Update(int xPos, int zPos, WorldElement newCell)
        {
            //if (newCell == null)
            //{
            //    Redraw();
            //    return;
            //}

            //Point pos = World.CellToPixelPos(newCell.Position, newCell);
            //var rect = new Rectangle(pos, newCell.BitMap.Size);

            //Funktioniert gut..ist aber unfassbar langsam...
            Redraw();

            //var map = (Bitmap)BlockMap.Image;
            //map.DrawImage(pos, newCell.GetDifferenceMask());

            //BlockMap.Invalidate(rect);
            //BlockMap.Update();


            #region 1 Ist gut... Layert Blöcke aber falsch
            //InvokeFormMethod(() =>
            //{
            //    var map = ((Bitmap)BlockMap.Image);
            //    map.Clear(rect);
            //    map.DrawImage(pos, newCell.BitMap);
            //    BlockMap.Invalidate(rect);
            //    BlockMap.Update();
            //});
            #endregion

            #region 2 Geht noch nich
            //var box = new PictureBox();
            //var map = new Bitmap(BrickBitmap.Width, BrickBitmap.Height);

            //map.DrawImage(0, 0, BrickBitmap);
            //box.BackColor = Color.Transparent;
            //box.Image = map;
            //box.Location = pos;

            //BlockMap.Controls.Add(box);
            //BlockMap.Controls.SetChildIndex(box, zPos);
            //BlockMap.Invalidate(rect);
            //BlockMap.Update();

            //Console.WriteLine(pos);
            //BlockMap.Controls.SetChildIndex() // vllt. was
            #endregion

            #region AAAAAAAAAAA
            //TODO: AAAAAAA
            //var blocks = new List<WorldElement>();
            //AddBlock(newCell.Position);
            //AddBlock(newCell.Position + new Position(1, 0, 0));
            //AddBlock(newCell.Position + new Position(0, 0, -1));
            //AddBlock(newCell.Position + new Position(1, 0, -1));

            //var rect = GetRect();
            //var map = (Bitmap)BlockMap.Image;
            ////map.Clear(rect);
            //Console.WriteLine(rect);
            //foreach(var e in blocks)
            //{
            //    if (e == null)
            //        continue;

            //    var pos = CellToPixelPos(e.Position, e);
            //    map.DrawImage(pos, e.BitMap);
            //}

            //InvokeFormMethod(() =>
            //{
            //    BlockMap.Invalidate(rect);
            //    BlockMap.Update();
            //});

            //void AddBlock(Position pos)
            //{
            //    if (IsPositionValid(pos))
            //    {
            //        var cell = GetCell(pos);
            //        blocks.Add(cell);
            //    }
            //}

            //Rectangle GetRect()
            //{
            //    var first = blocks.FirstOrDefault(bl => bl != null);
            //    if (first == null)
            //        return new Rectangle();

            //    if (blocks.Count(b => b != null) == 1)
            //        return first.Rect;

            //    int minX = first.Rect.X;
            //    int maxX = 0;
            //    int minY = first.Rect.Y;
            //    int maxY = 0;
            //    foreach(var b in blocks.Where(bl => bl != null))
            //    {
            //        var rect = b.Rect;
            //        if (rect.X < minX)
            //            minX = rect.X;
            //        if (rect.X > maxX)
            //            maxX = rect.X;

            //        if (rect.Y < minY)
            //            minY = rect.Y;
            //        if (rect.Y > maxY)
            //            maxY = rect.Y;
            //    }

            //    int height = maxY - minY;
            //    int widht = maxX - minX;

            //    if (minX == maxX)
            //        widht = first.Rect.Width;

            //    if (minY == maxY)
            //        height = first.Rect.Height;

            //    return new Rectangle(minX, minY, widht, height);
            //}
            #endregion
        }

        public override Bitmap DrawGrid()
        {
            int topPadding = PixelHeight * SizeY;           // Abstand von oben damit die Vertikalen Striche passen
            int height = SizeY * PixelHeight;               // Höhe in Pixeln
            int width = SizeX * PixelWidth;                 // Breite in Pixeln

            BottomLeft = new Point(Padding.Left, Padding.Top + topPadding + SizeZ * PixelHeight);
            BottomRight = new Point(BottomLeft.X + width, BottomLeft.Y);
            TopLeft = new Point(SizeZ * LineOffset + Padding.Left, Padding.Top + topPadding);
            TopRight = new Point(TopLeft.X + width, TopLeft.Y);

            Bitmap map = new Bitmap(TopRight.X + 5, BottomRight.Y + 5);

            for (int i = SizeZ; i >= 0; i--)
            {
                int x1 = BottomLeft.X + LineOffset * i;
                int y = BottomLeft.Y - PixelHeight * i;
                map.DrawLine(x1, y, x1 + width, y, Color.Blue);
            }

            for (int i = SizeX; i >= 0; i--)
            {
                int x1 = BottomLeft.X + PixelWidth * i;
                int x2 = TopLeft.X + PixelWidth * i;
                map.DrawLine(x1, BottomLeft.Y, x2, TopLeft.Y, Color.Blue);
            }

            for (int i = SizeZ; i >= 0; i--)
            {
                int x = BottomLeft.X + LineOffset * i;
                for (int j = 0; j < SizeY; j++)
                {
                    int y1 = BottomLeft.Y - PixelHeight * j - PixelHeight * i;
                    int y2 = y1 - PixelHeight;
                    map.DrawSplitLine(x, y1, x, y2, Color.Blue);
                }
            }

            for (int i = SizeX; i >= 0; i--)
            {
                int x = TopLeft.X + PixelWidth * i;
                for (int j = 0; j < SizeY; j++)
                {
                    int y1 = TopLeft.Y - PixelHeight * j;
                    int y2 = y1 - PixelHeight;
                    map.DrawSplitLine(x, y1, x, y2, Color.Blue);
                }
            }

            map.DrawLine(BottomLeft.X, BottomLeft.Y - height, TopLeft.X, TopLeft.Y - height, Color.Blue);
            map.DrawLine(TopLeft.X, TopLeft.Y - height, TopRight.X, TopRight.Y - height, Color.Blue);

            Bitmap cross = Resources.KoordinatensystemKreuz;
            Point crossPos = new Point(Math.Max(BottomLeft.X - 20, 0), Math.Max(BottomLeft.Y - height - cross.Height - 50, 0));
            map.DrawImage(crossPos, cross);

            //map.DrawLine(arrow, new Point(arrow.X - 40, arrow.Y + 40), Color.Blue);
            //map.DrawLine(arrow, new Point(arrow.X - 10, arrow.Y), Color.Blue);
            //map.DrawLine(arrow, new Point(arrow.X, arrow.Y + 10), Color.Blue);

            //Point nStart = new Point(arrow.X - 40, arrow.Y + 20);
            //map.DrawPath(Color.Blue, false, nStart,
            //    new Point(nStart.X, nStart.Y - 15),
            //    new Point(nStart.X + 8, nStart.Y),
            //    new Point(nStart.X + 8, nStart.Y - 15));

            return map;
        }

        public override Point CellToPixelPos(int xPos, int yPos, int zPos, WorldElement element)
        {
            int x = BottomLeft.X + zPos * LineOffset + xPos * PixelWidth;
            int y = BottomLeft.Y - element.BitMap.Height - (zPos + yPos) * PixelHeight + 1;
            return new Point(x + element.XOffset, y + element.YOffset);
        }
    }
}
