using System;
using Karol.Core;
using Karol.Extensions;
using System.Collections.Generic;
using System.Numerics;
using System.Windows.Forms;
using System.Drawing;
using System.Threading.Tasks;
using System.Threading;
using Karol.Core.Exceptions;
using System.Reflection;
using Karol.Properties;
using System.Linq;
using Karol.Core.WorldElements;
using System.IO;
using System.Text;

namespace Karol
{
    /// <summary>
    /// Eine Karol Welt in der Roboter, Ziegel, Marken, usw.. platziert werden können.
    /// </summary>
    public class World
    {
        #region Properties & Felder
        #region Hilfs zeug
        private const int MaxRoboterCount = 9;
        private const int PixelWidth = 30;
        private const int PixelHeight = 15;
        private readonly Bounds Padding = new Bounds(30, 0, 50, 0);

        private int _robotCount;

        /// <summary>
        /// Versatz von zeile zu zeile in Pixeln
        /// </summary>
        private int LineOffset => PixelWidth / 2;
        /// <summary>
        /// Obere Linke ecke der Grundfläche
        /// </summary>
        private Point TopLeft { get; set; }
        /// <summary>
        /// Untere Linke ecke der Grundfläche
        /// </summary>
        private Point BottomLeft { get; set; }
        /// <summary>
        /// Obere Rechte ecke der Grundfläche
        /// </summary>
        private Point TopRight { get; set; }
        /// <summary>
        /// Untere Rechte ecke der Grundfläche
        /// </summary>
        private Point BottomRight { get; set; }
        #endregion

        #region Events
        public event EventHandler<WorldChangedEventArgs> onRobotAdded;
        public event EventHandler onWorldClosed;
        #endregion

        #region Welt Größe
        /// <summary>
        /// Breite der Welt
        /// </summary>
        internal int SizeX { get; set; }
        /// <summary>
        /// Höhe der Welt
        /// </summary>
        internal int SizeY { get; set; }
        /// <summary>
        /// Länge/Tiefe der Welt
        /// </summary>
        internal int SizeZ { get; set; }

        /// <summary>
        /// Breite der Welt
        /// </summary>   
        public int Width => SizeX;
        /// <summary>
        /// Höhe der Welt
        /// </summary>
        public int Height => SizeY;
        /// <summary>
        /// Länge/Tiefe der Welt
        /// </summary>
        public int Depth => SizeZ;
        #endregion

        #region Public zeug
        /// <summary>
        /// Anzahl der Zellen die in der Welt zur Verfügung stehen. <br></br>
        /// Immer gleich (SizeX * SizeY * SizeZ)
        /// </summary>
        public int CellCount => SizeX * SizeY * SizeZ;
        /// <summary>
        /// Anzahl der Roboter die sich in dieser Welt befinden.
        /// </summary>
        public int RoboterCount
        {
            get => _robotCount;
            internal set
            {
                if (value > MaxRoboterCount)
                    throw new ArgumentOutOfRangeException($"In einer Welt können sich maximal {MaxRoboterCount} Roboter befinden!");

                _robotCount = value;
            }
        }
        #endregion

        #region Privat
        internal List<Robot> Robots { get; set; }
        private WorldElement[,,] Grid { get; set; }

        private KarolForm WorldForm { get; set; }
        private Thread UIThread { get; set; }
        private PictureBox BlockMap => WorldForm.BlockMap;
        #endregion
        #endregion

        #region Konstruktoren
        /// <summary>
        /// Erstellt eine neue Welt für Karol. <br></br>
        /// Die Welt muss mindestens 1x1x1 groß sein.
        /// </summary>
        /// <param name="sizeX">Breite der Welt</param>
        /// <param name="sizeY">Höhe der Welt</param>
        /// <param name="sizeZ">Länge/Tiefe der Welt</param>
        /// <exception cref="InvalidSizeException"></exception>
        public World(int sizeX, int sizeY, int sizeZ)
        {
            if (sizeX < 1 || sizeY < 1 || sizeZ < 1)
                throw new InvalidSizeException($"Unzulässige Weltengröße: ({sizeX}, {sizeY}, {sizeZ})");

            SizeX = sizeX;
            SizeY = sizeY;
            SizeZ = sizeZ;
            Robots = new List<Robot>();
            Grid = new WorldElement[sizeX, sizeZ, sizeY];

            OpenWindow();
            Pulse();
        }
        #endregion

        #region Zeug
        /// <summary>
        /// Öffnet und Konfiguriert das Fenster
        /// </summary>
        private async void OpenWindow()
        {
            WorldForm = new KarolForm();
            WorldForm.World = this;
            WorldForm.Text = $"Karol World - ({SizeX}, {SizeY}, {SizeZ})";
            WorldForm.SetUp(CreateGrid());

            WorldForm.FormClosed += (e, args) =>
            {
                OnWorldClosed();
            };

            await Task.Run(() =>
            {
                UIThread = Thread.CurrentThread;
                Application.Run(WorldForm);
            });
        }

        /// <summary>
        /// Erstellt das Hintergrund Grid
        /// </summary>
        /// <returns>Bitmap auf der das Grid gemalt ist.</returns>
        private Image CreateGrid()
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
                for(int j = 0; j < SizeY; j++)
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

            Point arrow = new Point(BottomLeft.X + 20, BottomLeft.Y - height - 50);
            map.DrawLine(arrow, new Point(arrow.X - 40, arrow.Y + 40), Color.Blue);
            map.DrawLine(arrow, new Point(arrow.X - 10, arrow.Y), Color.Blue);
            map.DrawLine(arrow, new Point(arrow.X, arrow.Y + 10), Color.Blue);

            Point nStart = new Point(arrow.X - 40, arrow.Y + 20);
            map.DrawPath(Color.Blue, false, nStart,
                new Point(nStart.X, nStart.Y - 15),
                new Point(nStart.X + 8, nStart.Y),
                new Point(nStart.X + 8, nStart.Y - 15));

            return map;
        }
        
        /// <summary>
        /// Sorgt dafür das sich das Fenster nach ablauf aller anweisungen nicht schließt
        /// </summary>
        private void Pulse()
        {
            Thread worldThread = Thread.CurrentThread;
            Thread pulseThread = new Thread(() =>
            {
                while (!WorldForm.IsDisposed)
                {
                    Thread.Sleep(100);
                    lock (worldThread)
                    {
                        Monitor.Pulse(worldThread);
                    }
                }
            });

            pulseThread.Start();
        }

        /// <summary>
        /// Führt die übergebene Methode auf dem UI Thred von WorldForm aus.
        /// </summary>
        /// <param name="method">Methode zum Ausführen</param>
        private void InvokeFormMethod(Action method)
        {
            while (!WorldForm.IsHandleCreated)
                Thread.Sleep(10);

            if (WorldForm.IsDisposed || UIThread == null || !UIThread.IsAlive)
                return;

            try
            {
                WorldForm.Invoke(method);
            }
            catch (Exception) { }
        }
        #endregion

        #region Util
        /// <summary>
        /// Übersetzt eine Grid-Koordinate in eine Pixel-Koordinate um einen Block an der gegebenen Stelle 
        /// zeichnen zu können.
        /// </summary>
        /// <param name="pos">Grid-Koordinate</param>
        /// <param name="map">Bild das gezeichnet werden soll</param>
        /// <returns>Pixel-Koordinate</returns>
        internal Point CellToPixelPos(Position pos, WorldElement element)
        {
            return CellToPixelPos(pos.X, pos.Y, pos.Z, element);
        }

        /// <summary>
        /// Übersetzt eine Grid-Koordinate in eine Pixel-Koordinate um einen Block an der gegebenen Stelle 
        /// zeichnen zu können.
        /// </summary>
        /// <param name="xPos">X Grid-Koordinate</param>
        /// <param name="zPos">Z Grid-Koordinate</param>
        /// <param name="map">Bild das gezeichnet werden soll</param>
        /// <returns>Pixel-Koordinate</returns>
        internal Point CellToPixelPos(int xPos, int zPos, WorldElement element)
        {
            int stackSize = GetStackSize(xPos, zPos);
            return CellToPixelPos(xPos, stackSize, zPos, element);
        }

        /// <summary>
        /// Übersetzt eine Grid-Koordinate in eine Pixel-Koordinate um einen Block an der gegebenen Stelle 
        /// zeichnen zu können.
        /// </summary>
        /// <param name="xPos">X Grid-Koordinate</param>
        /// <param name="yPos">Y Grid-Koordinate</param>
        /// <param name="zPos">Z Grid-Koordinate</param>
        /// <param name="element">Bild das gezeichnet werden soll</param>
        /// <returns>Pixel-Koordinate</returns>
        internal Point CellToPixelPos(int xPos, int yPos, int zPos, WorldElement element)
        {
            int x = BottomLeft.X + zPos * LineOffset + xPos * PixelWidth;
            int y = BottomLeft.Y - element.BitMap.Height - (zPos + yPos) * PixelHeight + 1;
            return new Point(x + element.XOffset, y + element.YOffset);
        }

        /// <summary>
        /// Zählt wie viele Blöcke auf dem Stapel liegen
        /// </summary>
        /// <returns>Anzahl der Blöcke auf dem Stapel</returns>
        internal int GetStackSize(int xPos, int zPos)
        {
            int firstEmpty = 0;
            for(int i = 0; i < SizeY; i++)
            {
                if (Grid[xPos, zPos, i] == null)
                {
                    firstEmpty = i;
                    break;
                }    

                if(i == SizeY - 1)
                {
                    firstEmpty = SizeY;
                    break;
                }
            }

            return firstEmpty;
        }

        /// <summary>
        /// Gibt zurück ob ein Stapel bis an die maximale höhe reicht
        /// </summary>
        /// <param name="xPos">X Position des Stapels</param>
        /// <param name="zPos">Z Position des Stapels</param>
        /// <returns>True wenn der Stapel voll ist, ansonsten false</returns>
        internal bool IsStackFull(int xPos, int zPos)
        {
            return GetStackSize(xPos, zPos) == SizeY;
        }

        /// <summary>
        /// Gibt zurück ob eine Position innerhalt der Welt ist oder nicht
        /// </summary>
        /// <param name="xPos">X Position</param>
        /// <param name="yPos">Y Position</param>
        /// <param name="zPos">Z Position</param>
        /// <returns>True wenn die Position innerhalb der Welt ist, ansonsten false.</returns>
        internal bool IsPositionValid(int xPos, int yPos, int zPos)
        {
            return xPos >= 0 && xPos < SizeX &&
                   yPos >= 0 && yPos < SizeY &&
                   zPos >= 0 && zPos < SizeZ;
        }

        /// <summary>
        /// Gibt zurück ob eine Position innerhalt der Welt ist oder nicht
        /// </summary>
        /// <param name="pos">Position</param>
        /// <returns>True wenn die Position innerhalb der Welt ist, ansonsten false.</returns>
        internal bool IsPositionValid(Position pos)
        {
            return IsPositionValid(pos.X, pos.Y, pos.Z);
        }

        /// <summary>
        /// Gibt zurück ob an der Position eine Zelle ist
        /// </summary>
        /// <returns>True wenn ja, ansonsten false</returns>
        internal bool HasCellAt(int xPos, int yPos, int zPos, out WorldElement cell)
        {
            xPos = Math.Max(xPos, 0);
            yPos = Math.Max(yPos, 0);
            zPos = Math.Max(zPos, 0);

            if (!IsPositionValid(xPos, yPos, zPos))
            {
                cell = null;
                return false;
            }

            cell = Grid[xPos, zPos, yPos];
            return cell != null;
        }

        /// <summary>
        /// Gibt zurück ob an der Position eine Zelle ist
        /// </summary>
        /// <returns>True wenn ja, ansonsten false</returns>
        internal bool HasCellAt(Position pos, out WorldElement cell)
        {
            return HasCellAt(pos.X, pos.Y, pos.Z, out cell);
        }
        #endregion

        #region Grid Rendern
        /// <summary>
        /// Zeichnet das gesammte Grid neu (Langsam)
        /// </summary>
        internal void Redraw()
        {
            Redraw(BlockMap.ClientRectangle);
        }

        /// <summary>
        /// Zeichnet das Grid in dem angegebenen Bereich neu (auch Langsam)
        /// </summary>
        internal void Redraw(Rectangle rect)
        {
            InvokeFormMethod(() =>
            {
                var map = (Bitmap)BlockMap.Image;
                map.Clear(rect);

                for (int x = 0; x < SizeX; x++)
                {
                    for (int z = SizeZ - 1; z >= 0; z--)
                    {
                        for (int y = 0; y < SizeY; y++)
                        {
                            if (!HasCellAt(x, y, z, out WorldElement cell))
                                continue;

                            var pos = CellToPixelPos(x, y, z, cell);
                            map.DrawImage(pos, cell.BitMap);
                        }
                    }
                }

                BlockMap.Invalidate(rect);
                BlockMap.Update();
            });
        }

        /// <summary>
        /// Wird aufgerufen, nachdem sich etwas im Grid geändert hat.
        /// </summary>
        /// <param name="xPos">X Position des sich geänderten Blocks</param>
        /// <param name="zPos">Z Position des sich geänderten Blocks</param>
        /// <param name="newCell">Neu hinzugefügtes Element</param>
        internal void Update(int xPos, int zPos, WorldElement newCell) // TODO: Besser machen...
        {
            //if (newCell == null)
            //{
            //    Redraw();
            //    return;
            //}

            //Point pos = CellToPixelPos(newCell.Position, newCell);
            //var rect = new Rectangle(pos, newCell.BitMap.Size);

            //Funktioniert gut..ist aber unfassbar langsam...
            Redraw();

            //InvokeFormMethod(() =>
            //{
            //    var map = (Bitmap)BlockMap.Image;
            //    map.DrawImage(pos, newCell.GetDifferenceMask());

            //    BlockMap.Invalidate(rect);
            //    BlockMap.Update();
            //});


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
        #endregion

        #region Zellen ändern
        /// <summary>
        /// Fügt einen Ziegel zu einem Stapel hinzu
        /// </summary>
        /// <returns>Ziegel der hinzugefügt wurde</returns>
        internal WorldElement AddToStack(int xPos, int zPos)
        {
            var element = new Brick();
            return AddToStack(xPos, zPos, element);
        }

        /// <summary>
        /// Fügt einen Block zu einem Stapel hinzu
        /// </summary>
        /// <returns>Block der hinzugefügt wurde</returns>
        internal WorldElement AddToStack(int xPos, int zPos, WorldElement element)
        {
            int stackSize = GetStackSize(xPos, zPos);
            if (stackSize == SizeY)
                throw new InvalidOperationException($"Kann an der Position ({xPos}, {stackSize}, {zPos}) keinen Block platzieren!");

            if (HasCellAt(xPos, Math.Max(stackSize - 1, 0), zPos, out WorldElement e) && !e.CanStackOnTop)
                return null;

            SetGridElement(xPos, stackSize, zPos, element);
            return element;
        }

        /// <summary>
        /// Setzt einen Ziegel in die entsprechende Zelle
        /// </summary>
        /// <param name="xPos">X Position des Blocks</param>
        /// <param name="zPos">Z Position des Blocks</param>
        /// <param name="updateView">Soll das View neu Gerendert werden</param>
        public void SetCell(int xPos, int zPos, bool updateView = true)
        {
            SetCell(xPos, zPos, new Brick(), updateView);
        }

        /// <summary>
        /// Setzt das definierte World Element in die entsprechende Zelle
        /// </summary>
        /// <param name="xPos">X Position des Blocks</param>
        /// <param name="zPos">Z Position des Blocks</param>
        /// <param name="updateView">Soll das View neu Gerendert werden</param>
        internal void SetCell(int xPos, int zPos, WorldElement element, bool updateView = true)
        {
            AddToStack(xPos, zPos, element);

            if (updateView)
                Update(xPos, zPos, element);
        }

        /// <summary>
        /// Setzt das definierte World Element in die entsprechende Zelle
        /// </summary>
        /// <param name="pos">Position des Blocks</param>
        /// <param name="updateView">Soll das View neu Gerendert werden</param>
        internal void SetCell(Position pos, WorldElement element, bool updateView = true)
        {
            SetCell(pos.X, pos.Y, pos.Z, element, updateView);
        }

        /// <summary>
        /// Setzt das definierte World Element in die entsprechende Zelle
        /// </summary>
        /// <param name="xPos">X Position des Blocks</param>
        /// <param name="yPos">Y Position des Blocks</param>
        /// <param name="zPos">Z Position des Blocks</param>
        /// <param name="updateView">Soll das View neu Gerendert werden</param>
        internal void SetCell(int xPos, int yPos, int zPos, WorldElement element, bool updateView = true)
        {
            SetGridElement(xPos, yPos, zPos, element);

            if (updateView)
                Update(xPos, zPos, element);
        }

        private void SetGridElement(int xPos, int yPos, int zPos, WorldElement element)
        {
            if (Grid[xPos, zPos, yPos] != null)
                Grid[xPos, zPos, yPos].OnDestroy();

            Grid[xPos, zPos, yPos] = element;
            if (element != null)
            {
                element.Position = new Position(xPos, yPos, zPos);
                element.World = this;
            }
        }

        /// <summary>
        /// Gibt das World Element an der entsprechenden Position zurück.
        /// </summary>
        /// <returns>World Element an der entsprechenden Position</returns>
        internal WorldElement GetCell(Position pos)
        {
            return GetCell(pos.X, pos.Y, pos.Z);
        }

        /// <summary>
        /// Gibt das World Element an der entsprechenden Position zurück.
        /// </summary>
        /// <returns>World Element an der entsprechenden Position</returns>
        internal WorldElement GetCell(int xPos, int yPos, int zPos)
        {
            return Grid[xPos, zPos, yPos];
        }
        #endregion

        #region Public
        /// <summary>
        /// Plaziert zufällige Ziegel in der Welt
        /// </summary>
        /// <param name="count">Anzahl der zu plazierenden Ziegel</param>
        /// <param name="maxStackHeight">Maximale höhe wie hoch die Ziegel gestapelt werden können. <br></br>
        /// Sollte die gegebenne Anzahl nicht in den Bereich passen werden keine Ziegel mehr plaziert.
        /// </param>
        /// <param name="randomColor">Blöcke in zufälliger farbe platzieren oder nicht</param>
        public void PlaceRandomBricks(int count, int maxStackHeight, bool randomColor = false)
        {
            maxStackHeight = Math.Clamp(maxStackHeight, 0, SizeY);
            count = Math.Clamp(count, 0, SizeX * SizeZ * maxStackHeight);
            Random rand = new Random();

            for (int i = 0; i < count; i++)
            {
                int xPos = rand.Next(0, SizeX);
                int zPos = rand.Next(0, SizeZ);
                int searches = 0;

                while (GetStackSize(xPos, zPos) >= maxStackHeight)
                {
                    if (xPos < SizeX - 1)
                    {
                        xPos++;
                    }
                    else
                    {
                        xPos = 0;
                        if(zPos < SizeZ - 1) zPos++;
                        else
                        {
                            zPos = 0;
                            searches++;
                        }
                    }

                    if (searches > 1)
                        break;
                }

                Color color = Color.Red;
                if (randomColor)
                    color = Color.FromArgb(rand.Next(0, 256), rand.Next(0, 256), rand.Next(0, 256));

                AddToStack(xPos, zPos, new Brick(color));
            }

            Redraw();
        }

        /// <summary>
        /// Plaziert zufällige Ziegel in der Welt
        /// </summary>
        /// <param name="count">Anzahl der zu plazierenden Steine</param>
        /// <param name="randomColor">Blöcke in zufälliger farbe platzieren oder nicht</param>
        public void PlaceRandomBricks(int count, bool randomColor = false)
        {
            PlaceRandomBricks(count, int.MaxValue, randomColor);
        }

        /// <summary>
        /// Erstellt eine Welt aus einem Bild. Jeder Pixel in dem Bild repräsentiert eine Zelle in der Welt.
        /// </summary>
        /// <param name="filePath">Pfad zu dem Bild. <br></br>
        /// Unterstützte Dateitypen: bmp, gif, jpeg, png, exif, tiff
        /// </param>
        /// <param name="worldHeight">Höhe der Welt.</param>
        /// <returns></returns>
        public static World LoadImage(string filePath, int worldHeight = 5)
        {
            if (!File.Exists(filePath))
                return null;

            var map = new Bitmap(filePath);
            World world = new World(map.Width, worldHeight, map.Height);
            world.WorldForm.ProgressBar.Visible = true;
            world.WorldForm.ProgressBar.Value = 0;
            world.WorldForm.ProgressBar.MarqueeAnimationSpeed = 30;

            for (int x = 0; x < map.Width; x++)
            {
                for(int y = 0; y < map.Height; y++)
                {
                    Color color = map.GetPixel(x, y);
                    if (color.A == 0)
                        continue;

                    world.AddToStack(x, map.Height - y - 1, new Brick(color));
                }
            }

            world.Redraw();
            map.Dispose();
            return world;
        }

        /// <summary>
        /// Lädt eine Welt aus einer .cskw (C Sharp Karol World) Datei.
        /// </summary>
        /// <param name="filePath">Ort an dem die Datei liegt.</param>
        /// <returns></returns>
        public static World Load(string filePath, KarolWorldFormat format = KarolWorldFormat.Auto)
        {
            try
            {
                WorldParser parser = new WorldParser();
                World world = parser.Load(filePath, format);
                world.Redraw();
                return world;
            }
            catch (InvalidDataException e)
            {
                throw e;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Speichert einen Screenshot der Welt an dem angegebenen Pfad.
        /// </summary>
        /// <param name="filePath">Pfad wo das Bild gespeichert werden soll.</param>
        public void SaveScreenshot(string filePath)
        {
            Bitmap map = new Bitmap(WorldForm.GridPicture.Image);
            map.DrawImage(0, 0, (Bitmap)BlockMap.Image);
            map.Save(filePath);
        }

        /// <summary>
        /// Speichert eine Ebene der Welt als .png Datei ab die weider geladen werden kann.
        /// </summary>
        /// <param name="filePath">Pfad wo die Datei gespeichert werden soll.</param>
        /// <param name="layer">Welche Ebene der Welt soll gespeichert werden <br></br>
        /// Standard ist 0
        /// </param>
        public void SaveImage(string filePath, int layer = 0)
        {
            if (layer >= SizeY)
                return;

            Bitmap map = new Bitmap(SizeX, SizeZ);
            for(int x = 0; x < SizeX; x++)
            {
                for(int z = 0; z < SizeZ; z++)
                {
                    var cell = GetCell(x, layer, z);
                    if (cell is Brick brick)
                    {
                        map.SetPixel(x, SizeZ - z - 1, brick.Paint);
                    }
                }
            }

            map.Save(filePath);
        }

        /// <summary>
        /// Speichert eine Welt als .cskw (C Sharp Karol World). Diese kann jederzeit wieder geladen werden.
        /// </summary>
        /// <param name="filePath">Ort an dem die Welt gespeichert werden soll.</param>
        public void Save(string filePath)
        {
            WorldParser parser = new WorldParser();
            parser.Save(this, filePath);
        }
        #endregion

        #region Events
        internal void OnRobotAdded(Robot robo)
        {
            var args = new WorldChangedEventArgs(robo);
            onRobotAdded?.Invoke(this, args);
        }

        internal void OnWorldClosed()
        {
            onWorldClosed?.Invoke(this, EventArgs.Empty);
        }
        #endregion
    }
}
