using Karol;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Karol.Core.Exceptions;
using System.IO;
using System.Threading;
using System.Text;
using System.Windows.Forms;
using Karol.Core.Rendering;
using Karol.Core;
using System.Runtime.InteropServices;

namespace ComponentTest
{
    [TestClass]
    public class WorldTest
    {
        // .cskw Datei
        private string TestLoad2Path = @"E:\Files\Schule\Schule 12\TrFi\C_Gartenzaun_Karol\Brams\KarolMap - 6.cskw";

        // .kdw Datei
        private string TestLoad3Path = @"E:\Files\Schule\Schule 12\TrFi\C_Gartenzaun_Karol\Brams\labs\lab5.kdw";

        [TestMethod("Schlechter Constructor 1")]
        [ExpectedException(typeof(InvalidSizeException))]
        public void TestConstructor1()
        {
            World w1 = new World(0, 1, 1);
        }

        [TestMethod("Schlechter Constructor 2")]
        [ExpectedException(typeof(InvalidSizeException))]
        public void TestConstructor2()
        {
            World w1 = new World(0, 0, 0);
        }

        [TestMethod("Guter Constructor")]
        public void TestConstructor3()
        {
            World w1 = new World(5, 5, 5);
        }

        [TestMethod("Lade Welt mit Fehler")]
        [ExpectedException(typeof(IOException), AllowDerivedTypes = true)]
        public void TestLoad1()
        {
            World w1 = World.Load("AAAAAAAAAAAAAAAAAAAAAA");
        }

        [TestMethod("Lade cskw Welt aus Datei")]
        [Description("Funktioniert nur wenn die geforderte Datei auf dem PC vorhanden ist.")]
        public void TestLoad2()
        {
            // Datei muss existieren und darf keine Fehler enthalten damit der test richtig ist...
            World w1 = World.Load(TestLoad2Path);
        }

        [TestMethod("Lade Java Welt aus Datei")]
        [Description("Funktioniert nur wenn die geforderte Datei auf dem PC vorhanden ist.")]
        public void TestLoad3()
        {
            // Datei muss existieren und darf keine Fehler enthalten damit der test richtig ist...
            World w1 = World.Load(TestLoad3Path);
        }

        [TestMethod("Lade cskw Welt aus Stream")]
        public void TestLoad4()
        {
            string world = "C_Gartenzaun_Karol_World\n" +
                           "Size: 3,2,3\n" +
                           "---  \n" +
                           "_ _ R(2)\n" +
                           "Q Q _\n" +
                           "_ Q Q\n" +
                           "---  \n" +
                           "_ _ _\n" +
                           "D D _\n" +
                           "_ D D\n";

            MemoryStream stream = new MemoryStream(Encoding.ASCII.GetBytes(world));
            StreamReader reader = new StreamReader(stream);
            World w1 = World.Load(reader);
        }

        [TestMethod("Lade Java Welt aus Stream")]
        public void TestLoad5()
        {
            string str = "KarolVersion2Deutsch 3 3 3 0 0 0 n n n o n n n o n n n m q q n o n n n o z n n o n n n o z n n o n n n o";
            MemoryStream stream = new MemoryStream(Encoding.ASCII.GetBytes(str));
            World w1 = World.Load(stream);
        }

        [TestMethod("Welt Speichern")]
        public void TestSave1()
        {
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "TestSave.cskw");

            World w1 = new World(3, 3, 3);
            Robot r1 = new Robot(1, 1, w1);

            w1.Save(path);
        }

        [TestMethod("Welt Speichern ohne Berechtigungen")]
        [ExpectedException(typeof(UnauthorizedAccessException))]
        public void TestSave2()
        {
            string path = @"C:\System Volume Information\TestSave.cskw";

            World w1 = new World(3, 3, 3);
            Robot r1 = new Robot(1, 1, w1);

            w1.Save(path);
        }

        [TestMethod("Welt Speichern an nicht Existierenden Pfad")]
        [ExpectedException(typeof(DirectoryNotFoundException))]
        public void TestSave3()
        {
            string path = @"C:\AAAAAAAAAAAAAAAAAAAAAAAAAAA\TestSave.cskw";

            World w1 = new World(3, 3, 3);
            Robot r1 = new Robot(1, 1, w1);

            w1.Save(path);
        }

        [TestMethod("Render Methode ändern")]
        public void TestChangeRenderMode()
        {
            World w1 = new World(4, 4, 4);
            w1.RenderingMode = WorldRenderingMode.Render2D;
            w1.RenderingMode = WorldRenderingMode.Render3D;
        }

        [TestMethod("Platziere zufällige Ziegel")]
        public void TestPlaceRendomBricks1()
        {
            World w1 = new World(15, 7, 10);
            int cnt = w1.PlaceRandomBricks(28);

            Assert.AreEqual(28, cnt);
        }

        [TestMethod("Platziere zufällige Ziegel mit Max. höhe 2")]
        public void TestPlaceRendomBricks2()
        {
            World w1 = new World(15, 7, 10);
            int cnt = w1.PlaceRandomBricks(10000000, 2, false);

            Assert.AreEqual(15 * 2 * 10, cnt);
        }

        [TestMethod("Platziere zufällige Ziegel in zufälliger Farbe")]
        public void TestPlaceRendomBricks3()
        {
            World w1 = new World(15, 7, 10);
            int cnt = w1.PlaceRandomBricks(150, 5, true);

            Assert.AreEqual(150, cnt);
        }

        [TestMethod("Mehrere Welten auf einmal haben...")]
        public void TestMultipleWorlds()
        {
            World w1 = new World(2, 2, 2);
            World w2 = new World(3, 3, 3);
            World w3 = new World(7, 6, 4);
            World w4 = new World(8, 5, 5);
            World w5 = new World(9, 4, 6);
        }

        [TestMethod("Screenshot machen")]
        public void TestSaveScreenshot1()
        {
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "TestScreenshot.png");

            World w1 = new World(5, 2, 5);
            Robot r1 = new Robot(1, 1, w1, Direction.East);
            w1.SaveScreenshot(path);
        }

        [TestMethod("Screenshot machen in nicht Existierenden Ordner")]
        [ExpectedException(typeof(ExternalException))]
        public void TestSaveScreenshot2()
        {
            string path = @"C:\AAAAAAAAAAAAAAAAAAAAAAAAAAA\TestScreenshot.png";

            World w1 = new World(5, 2, 5);
            Robot r1 = new Robot(1, 1, w1, Direction.East);
            w1.SaveScreenshot(path);
        }

        [TestMethod("Welt als Bild Speichern")]
        public void TestSaveImage1()
        {
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "TestWorldImage.png");
            string world = "C_Gartenzaun_Karol_World\n" +
               "Size: 3,2,3\n" +
               "---  \n" +
               "_ _ R(2)\n" +
               "B B _\n" +
               "_ B B\n";

            MemoryStream stream = new MemoryStream(Encoding.ASCII.GetBytes(world));
            StreamReader reader = new StreamReader(stream);
            World w1 = World.Load(reader);

            w1.SaveImage(path);
        }

        [TestMethod("Welt als Bild Speichern in nicht Existierenden Ordner")]
        [ExpectedException(typeof(ExternalException))]
        public void TestSaveImage2()
        {
            string path = @"C:\AAAAAAAAAAAAAAAAAAAAAAAAAAAAA\TestWorldImage.png";
            string world = "C_Gartenzaun_Karol_World\n" +
               "Size: 3,2,3\n" +
               "---  \n" +
               "_ _ R(2)\n" +
               "B B _\n" +
               "_ B B\n";

            MemoryStream stream = new MemoryStream(Encoding.ASCII.GetBytes(world));
            StreamReader reader = new StreamReader(stream);
            World w1 = World.Load(reader);

            w1.SaveImage(path);
        }

        [TestMethod("Bild Laden")]
        public void TestLoadImage1()
        {
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "TestWorldImage.png");
            World w1 = World.LoadImage(path);
        }

        [TestMethod("Bild Laden das nicht existiert")]
        [ExpectedException(typeof(FileNotFoundException))]
        public void TestLoadImage2()
        {
            string path = @"C:\AAAAAAAAAAAAAAAAAAAAAAAAAAAAA\TestWorldImage.png";
            World w1 = World.LoadImage(path);
        }

        [TestMethod("Viele sachen in kombination")]
        public void TestAll()
        {
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "TestKomibWorld.cskw");
            string world = "C_Gartenzaun_Karol_World\n" +
               "Size: 3,4,3\n" +
               "---  \n" +
               "_ _ R(2)\n" +
               "B B _\n" +
               "_ B B\n";

            MemoryStream stream = new MemoryStream(Encoding.ASCII.GetBytes(world));
            StreamReader reader = new StreamReader(stream);
            World w1 = World.Load(reader);

            w1.Save(path);

            World w2 = World.Load(path);
            Robot r1 = new Robot(1, 0, w2);

            w2.RenderingMode = WorldRenderingMode.Render2D;
        }
    }
}


