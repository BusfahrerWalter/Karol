using Karol.Core.WorldElements;
using Karol.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace Karol.Core.Rendering
{
    /// <summary>
    /// Gibt an wie eine Welt gerendert werden soll.
    /// </summary>
    public enum WorldRenderingMode : int
    {
        Render2D = 2,
        Render3D = 3
    }

    /// <summary>
    /// Basisklasse für alle Welt Renderer
    /// </summary>
    internal abstract class Renderer
    {
        private World _world;

        /// <summary>
        /// Zielwelt
        /// </summary>
        public World World
        {
            get => _world;
            set
            {
                _world = value;
                BlockMap = value.WorldForm.BlockMap;
                GridMap = value.WorldForm.GridPicture;
            }
        }
        /// <summary>
        /// Hintergrund (Wird nur einmal am anfang gemalt)
        /// </summary>
        public PictureBox GridMap { get; set; }
        /// <summary>
        /// Vordergrund (Wird bei jedem Update / Redraw geändert)
        /// </summary>
        public PictureBox BlockMap { get; set; }

        /// <summary>
        /// Obere Linke ecke der Grundfläche
        /// </summary>
        public Point TopLeft { get; set; }
        /// <summary>
        /// Untere Linke ecke der Grundfläche
        /// </summary>
        public Point BottomLeft { get; set; }
        /// <summary>
        /// Obere Rechte ecke der Grundfläche
        /// </summary>
        public Point TopRight { get; set; }
        /// <summary>
        /// Untere Rechte ecke der Grundfläche
        /// </summary>
        public Point BottomRight { get; set; }

        /// <summary>
        /// X Größe der Welt
        /// </summary>
        protected int SizeX => World.SizeX;
        /// <summary>
        /// Y Größe der Welt
        /// </summary>
        protected int SizeY => World.SizeY;
        /// <summary>
        /// Z Größe der Welt
        /// </summary>
        protected int SizeZ => World.SizeZ;

        public Renderer(World world)
        {
            World = world;
        }

        /// <summary>
        /// Zeichnet die gesammte Welt neu
        /// </summary>
        public abstract void Redraw();

        /// <summary>
        /// Updated die Welt um einen nuen Block Korrekt zu darzustellen
        /// </summary>
        /// <param name="newCell">Der neu platzierte Block. Oder null wenn ein Block entfernt wurde.</param>
        public abstract void Update(int xPos, int zPos, WorldElement newCell);
        
        /// <summary>
        /// Zeichnet den Hintergrund
        /// </summary>
        public abstract Bitmap DrawGrid();

        /// <summary>
        /// Übersetzt eine Grid-Koordinate in eine Pixel-Koordinate um einen Block an der gegebenen Stelle 
        /// zeichnen zu können.
        /// </summary>
        /// <param name="xPos">X Grid-Koordinate</param>
        /// <param name="yPos">Y Grid-Koordinate</param>
        /// <param name="zPos">Z Grid-Koordinate</param>
        /// <param name="element">Bild das gezeichnet werden soll</param>
        /// <returns>Pixel-Koordinate</returns>
        public abstract Point CellToPixelPos(int xPos, int yPos, int zPos, WorldElement element);

        /// <summary>
        /// Übersetzt eine Grid-Koordinate in eine Pixel-Koordinate um einen Block an der gegebenen Stelle 
        /// zeichnen zu können.
        /// </summary>
        /// <param name="pos">Grid-Koordinate</param>
        /// <param name="map">Bild das gezeichnet werden soll</param>
        /// <returns>Pixel-Koordinate</returns>
        public Point CellToPixelPos(Position pos, WorldElement element)
        {
            return CellToPixelPos(pos.X, pos.Y, pos.Z, element);
        }

        /// <summary>
        /// Gibt ein Bild der Welt zurück.
        /// </summary>
        public Bitmap GetScreenshot()
        {
            Bitmap map = new Bitmap(GridMap.Image);
            map.DrawImage(0, 0, (Bitmap)BlockMap.Image);
            return map;
        }

        /// <summary>
        /// Gibt den mit der übergebenen Rendering Methode assoziierten Renderer zurück.
        /// </summary>
        /// <param name="targetWorld">Zielwelt des Renderers</param>
        /// <param name="mode">Rendering Methode</param>
        /// <returns></returns>
        public static Renderer ForRenderingMode(World targetWorld, WorldRenderingMode mode)
        {
            var list = typeof(Renderer).Assembly
                .GetTypes()
                .Where(t => t.IsSubclassOf(typeof(Renderer)))
                .ToArray();

            var erg = list
                .Where(t => t.GetCustomAttribute<RendererInfoAttribute>().Mode == mode)
                .FirstOrDefault();

            if (erg == null)
                throw new ArgumentException();

            return (Renderer)Activator.CreateInstance(erg, targetWorld);
        }
    }
}
