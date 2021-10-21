using Karol.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Karol.Core
{
    /// <summary>
    /// Enthält einstellungen aus denen ein neuer Roboter erzeugt werden kann.
    /// </summary>
    public class RobotOptions
    {
        /// <summary>
        /// Überschreibt die standard Textur des Roboters wenn er nach Norden schaut.
        /// </summary>
        public Bitmap NorthImage { get; set; }
        /// <summary>
        /// Überschreibt die standard Textur des Roboters wenn er nach Osten schaut.
        /// </summary>
        public Bitmap EastImage { get; set; }
        /// <summary>
        /// Überschreibt die standard Textur des Roboters wenn er nach Süden schaut.
        /// </summary>
        public Bitmap SouthImage { get; set; }
        /// <summary>
        /// Überschreibt die standard Textur des Roboters wenn er nach Westen schaut.
        /// </summary>
        public Bitmap WestImage { get; set; }

        /// <summary>
        /// Ein Set das alle Bilder für einen Roboter enthält. <br></br>
        /// Sollte nicht in Kombination mit NorthImage, EastImage, SouthImage oder WestImage verwendet werden.
        /// </summary>
        public ImageSet Set 
        {
            get => new ImageSet(NorthImage, EastImage, SouthImage, WestImage);
            set
            {
                NorthImage = value.Images[0];
                EastImage = value.Images[1];
                SouthImage = value.Images[2];
                WestImage = value.Images[3];
            }
        }

        /// <summary>
        /// Welt in der der Roboter leben soll
        /// </summary>
        public World World { get; private set; }
        /// <summary>
        /// Richtung in die der Roboter am anfang schauen soll <br></br>
        /// Standard ist Norden
        /// </summary>
        public Direction InitialDirection { get; set; }

        /// <summary>
        /// Start X Position des Roboters
        /// </summary>
        public int StartX { get; set; }
        /// <summary>
        /// Start Z Position des Roboters
        /// </summary>
        public int StartZ { get; set; }

        /// <summary>
        /// X Offset um das die Bitmap beim Rendern der Welt verschoben wird.
        /// </summary>
        public int OffsetX { get; set; }
        /// <summary>
        /// Y Offset um das die Bitmap beim Rendern der Welt verschoben wird.
        /// </summary>
        public int OffsetY { get; set; }

        /// <summary>
        /// Zeit in Millisekunden die der Roboter zwischen 2 Aktionen wartet.
        /// </summary>
        public int Delay { get; set; }

        /// <summary>
        /// Erzeugt eine neue Instantz der RobotOptions Klasse
        /// </summary>
        public RobotOptions(World world) 
            : this(0, 0, world) { }

        /// <summary>
        /// Erzeugt eine neue Instantz der RobotOptions Klasse
        /// </summary>
        /// <param name="startX">Start X Position</param>
        /// <param name="startZ">Start Z Position</param>
        /// <param name="world">Zielwelt</param>
        public RobotOptions(int startX, int startZ, World world)
        {
            World = world;
            StartX = startX;
            StartZ = startZ;
            InitialDirection = Direction.North;
            Delay = Robot.DefaultDelay;
        }

        /// <summary>
        /// Gibt eine Liste aller Bilder zurück die in dieser Optionen Instanz gespeichert sind. <br></br>
        /// Diese Liste ist immer 4 stellen lang. Nicht gesetzte Bilder werden mit null gefüllt.
        /// </summary>
        /// <returns>Liste aller Bilder</returns>
        public Bitmap[] GetImages()
        {
            return new Bitmap[]
            {
                NorthImage, EastImage, SouthImage, WestImage
            };
        }
    }

    /// <summary>
    /// Ein Set von Bildern für einen Roboter
    /// </summary>
    public class ImageSet
    {
        /// <summary>
        /// Liste aller Bilder in diesem Set
        /// </summary>
        public Bitmap[] Images { get; set; }

        internal ImageSet(params Bitmap[] images)
        {
            Images = images;
        }

        /// <summary>
        /// Roboter Skin Freddy
        /// </summary>
        public static ImageSet Freddy => Create(Resources.Freddy);

        /// <summary>
        /// Standard Skin eines Roboters (Ohne Nummer)
        /// </summary>
        public static ImageSet Default => Create(Resources.robot2, Resources.robot3, Resources.robot0, Resources.robot1);

        /// <summary>
        /// Erstellt ein neues Image Set aus einer Bitmap
        /// </summary>
        /// <param name="image">Bild für den Roboter</param>
        /// <returns>Image set in dem jedes Bild gleich ist</returns>
        public static ImageSet Create(Bitmap image)
        {
            return new ImageSet(image, image, image, image);
        }

        /// <summary>
        /// Erstellt ein neues Image Set aus 4 Bitmaps
        /// </summary>
        /// <param name="northImage">Bild wenn der Roboter nach Norden schaut</param>
        /// <param name="eastImage">Bild wenn der Roboter nach Osten schaut</param>
        /// <param name="southImage">Bild wenn der Roboter nach Süden schaut</param>
        /// <param name="westImage">Bild wenn der Roboter nach Westen schaut</param>
        /// <returns>Image set mit 4 verschiedenen Bildern</returns>
        public static ImageSet Create(Bitmap northImage, Bitmap eastImage, Bitmap southImage, Bitmap westImage)
        {
            return new ImageSet(northImage, eastImage, southImage, westImage);
        }
    }
}
