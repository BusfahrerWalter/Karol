using Karol.Properties;
using System.Drawing;

namespace Karol.Core
{
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
        /// Magenta Skin für einen Roboter (Ohne Nummer)
        /// </summary>
        public static ImageSet Magenta => Create(Resources.robotMagentaN, Resources.robotMagentaE, Resources.robotMagentaS, Resources.robotMagentaW);

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
        /// Erstellt ein neues Image Set 
        /// </summary>
        /// <param name="filePath">Pfad zu einem Bild für den Roboter</param>
        /// <returns>Image set in dem jedes Bild gleich ist</returns>
        public static ImageSet Create(string filePath)
        {
            return Create(new Bitmap(filePath));
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

        /// <summary>
        /// Erstellt ein neues Image Set aus 4 Bildern
        /// </summary>
        /// <param name="northImage">Bild wenn der Roboter nach Norden schaut</param>
        /// <param name="eastImage">Bild wenn der Roboter nach Osten schaut</param>
        /// <param name="southImage">Bild wenn der Roboter nach Süden schaut</param>
        /// <param name="westImage">Bild wenn der Roboter nach Westen schaut</param>
        /// <returns>Image set mit 4 verschiedenen Bildern</returns>
        public static ImageSet Create(string northImage, string eastImage, string southImage, string westImage)
        {
            return Create(new Bitmap(northImage), new Bitmap(eastImage), new Bitmap(southImage), new Bitmap(westImage));
        }
    }
}
