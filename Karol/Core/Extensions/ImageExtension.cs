using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using System.Text;

namespace Karol.Extensions
{
    internal static class ImageExtension
    {
        #region Draw Line / Path
        public static void DrawLine(this Bitmap img, int x1, int y1, int x2, int y2, Color color)
        {
            int dx = Math.Abs(x1 - x2);
            int dy = -Math.Abs(y1 - y2);
            int sx = x1 < x2 ? 1 : -1;
            int sy = y1 < y2 ? 1 : -1;
            int err = dx + dy;

            while (true)
            {
                img.SetPixel(x1, y1, color);
                if (x1 == x2 && y1 == y2)
                    break;

                int e2 = 2 * err;
                if(e2 >= dy)
                {
                    err += dy;
                    x1 += sx;
                }
                
                if(e2 <= dx)
                {
                    err += dx;
                    y1 += sy;
                }
            }

            #region Alt
            //int xDist = Math.Abs(x1 - x2);
            //int yDist = Math.Abs(y1 - y2);
            //int xStep = Math.Max(xDist / Math.Max(yDist, 1), 1);
            //int yStep = Math.Max(yDist / Math.Max(xDist, 1), 1);
            //int xRest = xDist % Math.Max(yDist, 1);
            //int yRest = yDist % Math.Max(xDist, 1);

            //Console.WriteLine(xRest + "  " + yRest);

            //if (xDist == 0)
            //{
            //    yStep = yDist;
            //    xStep = 0;
            //}

            //if (yDist == 0)
            //{
            //    xStep = xDist;
            //    yStep = 0;
            //}

            //while (x1 != x2 || y1 != y2)
            //{
            //    for (int i = 0; i < xStep; i++)
            //    {
            //        img.SetPixel(x1, y1, color);
            //        x1 = x1 < x2 ? 1 : -1;
            //    }

            //    for (int i = 0; i < yStep; i++)
            //    {
            //        img.SetPixel(x1, y1, color);
            //        y1 = y1 < y2 ? 1 : -1;
            //    }
            //}
            #endregion
        }

        public static void DrawLine(this Bitmap img, Point from, Point to, Color color)
        {
            DrawLine(img, from.X, from.Y, to.X, to.Y, color);
        }

        public static void DrawSplitLine(this Bitmap img, int x1, int y1, int x2, int y2, Color color)
        {
            int xDist = (int)((x1 - x2) / 2.5f);
            int yDist = (int)((y1 - y2) / 2.5f);

            Point start1 = new Point(x1, y1);
            Point end1 = new Point(x1 - xDist, y1 - yDist);
            Point start2 = new Point(x2 + xDist, y2 + yDist);
            Point end2 = new Point(x2, y2);

            DrawLine(img, start1, end1, color);
            DrawLine(img, start2, end2, color);
        }

        public static void DrawSplitLine(this Bitmap img, Point from, Point to, Color color)
        {
            DrawSplitLine(img, from.X, from.Y, to.X, to.Y, color);
        }

        public static void DrawPath(this Bitmap img, Color color, bool close, Point start, params Point[] nextPoints)
        {
            var last = start;
            foreach(var point in nextPoints)
            {
                DrawLine(img, last, point, color);
                last = point;
            }

            if (close)
                DrawLine(img, last, start, color);
        }

        public static void DrawPath(this Bitmap img, Color color, Point start, params Point[] nextPoints)
        {
            DrawPath(img, color, true, start, nextPoints);
        }
        #endregion

        #region Draw Image
        public static void DrawImage(this Bitmap img, int xPos, int yPos, Bitmap image)
        {
            for(int x = 0; x < image.Width; x++)
            {
                for(int y = 0; y < image.Height; y++)
                {
                    Color color = image.GetPixel(x, y);
                    if(color.A != 0)
                    {
                        img.SetPixel(xPos + x, yPos + y, color);
                    }
                }
            }
        }

        public static void DrawImage(this Bitmap img, Point position, Bitmap image)
        {
            DrawImage(img, position.X, position.Y, image);
        }
        #endregion

        #region Clear
        public static void Clear(this Bitmap img)
        {
            for (int i = 0; i < img.Width; i++)
            {
                for (int j = 0; j < img.Height; j++)
                {
                    img.SetPixel(i, j, Color.Transparent);
                }
            }
        }

        public static void Clear(this Bitmap img, Rectangle rect)
        {
            for (int i = 0; i < rect.Width; i++)
            {
                for (int j = 0; j < rect.Height; j++)
                {
                    img.SetPixel(i + rect.Left, j + rect.Top, Color.Transparent);
                }
            }
        }

        public static void Clear(this Bitmap img, Point pos, Bitmap map)
        {
            for (int i = 0; i < map.Width; i++)
            {
                for (int j = 0; j < map.Height; j++)
                {
                    if (map.GetPixel(i, j).A != 0)
                    {
                        img.SetPixel(i + pos.X, j + pos.Y, Color.Transparent);
                    }
                }
            }
        }
        #endregion

        #region Color
        public static void MultiplyColor(this Bitmap img, Color multiplier)
        {
            for (int x = 0; x < img.Width; x++)
            {
                for (int y = 0; y < img.Height; y++)
                {
                    Color color = img.GetPixel(x, y);
                    float r1 = color.R / 255.0f;
                    float g1 = color.G / 255.0f;
                    float b1 = color.B / 255.0f;

                    float r2 = multiplier.R / 255.0f;
                    float g2 = multiplier.G / 255.0f;
                    float b2 = multiplier.B / 255.0f;

                    Color ergColor = Color.FromArgb(color.A, (int)(r1 * r2 * 255), (int)(g1 * g2 * 255), (int)(b1 * b2 * 255));
                    img.SetPixel(x, y, ergColor);
                }
            }
        }
        #endregion
    }
}
