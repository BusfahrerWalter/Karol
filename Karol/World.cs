using Karol.Core;
using Karol.Core.Exceptions;
using Karol.Core.Rendering;
using Karol.Core.WorldElements;
using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

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
        private WorldRenderingMode _renderingMode;
        #endregion

        #region Events
        /// <summary>
        /// Wirt aufgerufen wenn ein Roboter zu der Welt hinzugefügt wird.
        /// </summary>
        public event EventHandler<WorldChangedEventArgs> onRobotAdded;
        /// <summary>
        /// Wrid aufgerufen wenn die Welt geschlossen wird.
        /// </summary>
        public event EventHandler onWorldClosed;
        /// <summary>
        /// Wird aufgerufen wenn sich die Rendering Methode geändert hat.
        /// </summary>
        internal event EventHandler<WorldRenderingMode> onRenderingModeChanged;
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
            get => RobotCollection.Count;
        }
        /// <summary>
        /// Der aktuelle Render Methode dieser Welt
        /// </summary>
        public WorldRenderingMode RenderingMode
        {
            get => _renderingMode;
            set
            {
                InvokeFormMethod(() =>
                {
                    WorldRenderer = Renderer.ForRenderingMode(this, value);
                    WorldForm.SetUp(WorldRenderer.DrawGrid(), false);
                    Redraw();
                });

                if(value != _renderingMode)
                {
                    OnRenderingModeChanged(value);
                    _renderingMode = value;
                }
            }
        }
        /// <summary>
        /// Liste aller Roboter in dieser Welt
        /// </summary>
        public Robot[] Robots
        {
            get => RobotCollection.ToArray();
        }
        #endregion

        #region Privat
        internal RobotCollection RobotCollection { get; set; }
        internal KarolForm WorldForm { get; set; }
        internal Renderer WorldRenderer { get; set; }
        private WorldElement[,,] Grid { get; set; }
        private Thread UIThread { get; set; }
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
            RobotCollection = new RobotCollection(MaxRoboterCount);
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
            WorldForm = new KarolForm(this, $"Karol World - ({SizeX}, {SizeY}, {SizeZ})");
            WorldRenderer = new WorldRenderer3D(this);

            WorldForm.SetUp(WorldRenderer.DrawGrid());
            WorldForm.FormClosed += (e, args) =>
            {
                OnWorldClosed();
            };

            RobotCollection.onRobotAdded += (s, e) =>
            {
                WorldForm.AddRobotToList(e.NewElement as Robot);
                OnRobotAdded(e.NewElement as Robot);
            };

            RobotCollection.onRobotRemoved += (s, e) =>
            {
                WorldForm.RemoveRobotFromList(e.NewElement as Robot);
            };

            await Task.Run(() =>
            {
                UIThread = Thread.CurrentThread;
                Application.Run(WorldForm);
            });
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
        internal void InvokeFormMethod(Action method)
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
        /// <param name="element">Element an der Position</param>
        /// <returns>Pixel-Koordinate</returns>
        internal Point CellToPixelPos(Position pos, WorldElement element)
        {
            return WorldRenderer.CellToPixelPos(pos, element);
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
            return WorldRenderer.CellToPixelPos(xPos, yPos, zPos, element);
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
        /// Zeichnet das Grid in dem angegebenen Bereich neu (auch Langsam)
        /// </summary>
        internal void Redraw()
        {
            InvokeFormMethod(() =>
            {
                WorldRenderer.Redraw();
            });
        }

        /// <summary>
        /// Wird aufgerufen, nachdem sich etwas im Grid geändert hat.
        /// </summary>
        /// <param name="xPos">X Position des sich geänderten Blocks</param>
        /// <param name="zPos">Z Position des sich geänderten Blocks</param>
        /// <param name="newCell">Neu hinzugefügtes Element</param>
        internal void Update(int xPos, int zPos, WorldElement newCell) 
        {
            InvokeFormMethod(() =>
            {
                WorldRenderer.Update(xPos, zPos, newCell);
            });
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
        internal void SetCell(int xPos, int zPos, bool updateView = true)
        {
            SetCell(xPos, zPos, new Brick(), updateView);
        }

        /// <summary>
        /// Setzt das definierte World Element in die entsprechende Zelle
        /// </summary>
        /// <param name="xPos">X Position des Blocks</param>
        /// <param name="zPos">Z Position des Blocks</param>
        /// <param name="element">Element das Platziert werden soll</param>
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
        /// <param name="element">Element das Platziert werden soll</param>
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
        /// <param name="element">Element das Platziert werden soll</param>
        /// <param name="updateView">Soll das View neu Gerendert werden</param>
        internal void SetCell(int xPos, int yPos, int zPos, WorldElement element, bool updateView = true)
        {
            SetGridElement(xPos, yPos, zPos, element);

            if (updateView)
                Update(xPos, zPos, element);
        }

        internal void SetGridElement(int xPos, int yPos, int zPos, WorldElement element)
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
        /// <returns>Anzahl der tatsächlich Platzierten Ziegel</returns>
        public int PlaceRandomBricks(int count, int maxStackHeight, bool randomColor = false)
        {
            maxStackHeight = Math.Clamp(maxStackHeight, 0, SizeY);
            count = Math.Clamp(count, 0, SizeX * SizeZ * maxStackHeight);
            Random rand = new Random();
            int acctualCount = 0;

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
                acctualCount++;
            }

            Redraw();
            return acctualCount;
        }

        /// <summary>
        /// Plaziert zufällige Ziegel in der Welt
        /// </summary>
        /// <param name="count">Anzahl der zu plazierenden Steine</param>
        /// <param name="randomColor">Blöcke in zufälliger farbe platzieren oder nicht</param>
        /// <returns>Anzahl der tatsächlich Platzierten Ziegel</returns>
        public int PlaceRandomBricks(int count, bool randomColor = false)
        {
            return PlaceRandomBricks(count, int.MaxValue, randomColor);
        }

        /// <summary>
        /// Erstellt eine Welt aus einem Bild. Jeder Pixel in dem Bild repräsentiert eine Zelle in der Welt.
        /// </summary>
        /// <param name="filePath">Pfad zu dem Bild. <br></br>
        /// Unterstützte Dateitypen: bmp, gif, jpeg, png, exif, tiff
        /// </param>
        /// <param name="worldHeight">Höhe der Welt.</param>
        /// <returns>Aus dem Bild geladene Welt</returns>
        /// <exception cref="FileNotFoundException"></exception>
        public static World LoadImage(string filePath, int worldHeight = 5)
        {
            WorldParser parser = new WorldParser();
            return parser.LoadImage(filePath, worldHeight);
        }

        /// <summary>
        /// Lädt eine Welt aus einer .cskw (C Sharp Karol World) oder einer .kwd (Karol Welt Deutsch) Datei.
        /// </summary>
        /// <param name="filePath">Ort an dem die Datei liegt.</param>
        /// <param name="format">Format der Datei die geladen werden soll</param>
        /// <returns>Aus der Datei geladene Welt</returns>
        /// <exception cref="IOException"></exception>
        /// <exception cref="InvalidDataException"></exception>
        public static World Load(string filePath, KarolWorldFormat format = KarolWorldFormat.Auto)
        {
            WorldParser parser = new WorldParser();
            return parser.Load(filePath, format);
        }

        /// <summary>
        /// Lädt eine Welt aus einem StreamReader.
        /// </summary>
        /// <param name="reader">Reader der die Weltdaten lesen kann.</param>
        /// <param name="format">Format der Datei die geladen werden soll</param>
        /// <returns>Aus dem StreamReader geladene Welt</returns>
        /// <exception cref="IOException"></exception>
        /// <exception cref="InvalidDataException"></exception>
        public static World Load(StreamReader reader, KarolWorldFormat format = KarolWorldFormat.Auto)
        {
            WorldParser parser = new WorldParser();
            return parser.Load(reader, format);
        }

        /// <summary>
        /// Lädt eine Welt aus einem Stream.
        /// </summary>
        /// <param name="stream">Stream zu den Weltdaten.</param>
        /// <param name="format">Format der Datei die geladen werden soll</param>
        /// <returns>Aus dem Stream geladene Welt</returns>
        /// <exception cref="IOException"></exception>
        /// <exception cref="InvalidDataException"></exception>
        public static World Load(Stream stream, KarolWorldFormat format = KarolWorldFormat.Auto)
        {
            StreamReader reader = new StreamReader(stream);
            return Load(reader, format);
        }

        /// <summary>
        /// Speichert einen Screenshot der Welt an dem angegebenen Pfad.
        /// </summary>
        /// <param name="filePath">Pfad wo das Bild gespeichert werden soll.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ExternalException"></exception>
        public void SaveScreenshot(string filePath)
        {
            Bitmap map = WorldRenderer.GetScreenshot();
            map.Save(filePath);
        }

        /// <summary>
        /// Speichert eine Ebene der Welt als .png Datei ab die weider geladen werden kann.
        /// </summary>
        /// <param name="filePath">Pfad wo die Datei gespeichert werden soll.</param>
        /// <param name="layer">Welche Ebene der Welt soll gespeichert werden <br></br>
        /// Standard ist 0
        /// </param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ExternalException"></exception>
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
        /// <exception cref="IOException"></exception>
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

        internal void OnRenderingModeChanged(WorldRenderingMode mode)
        {
            onRenderingModeChanged?.Invoke(this, mode);
        }
        #endregion
    }
}
