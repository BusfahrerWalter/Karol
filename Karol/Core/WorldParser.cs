using Karol.Core.WorldElements;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using System.Runtime.CompilerServices;
using Karol.Core.Annotations;
using System.Reflection;

[assembly: InternalsVisibleTo("ComponentTest")]
namespace Karol.Core
{
    /// <summary>
    /// Dateiformat in dem die Karol Welt gespeichert ist.
    /// </summary>
    public enum KarolWorldFormat
    {
        /// <summary>
        /// C# Welten
        /// </summary>
        CSharp,
        /// <summary>
        /// Java Welten
        /// </summary>
        Java,
        /// <summary>
        /// Automatisches erkennen des Formats. (Kann unter umständen nicht bei jeder Welt funktionieren)
        /// </summary>
        Auto
    }

    /// <summary>
    /// Klasse zum Speichern und Laden von Karol welten. <br></br>
    /// Kann C# und Java Welten Laden.
    /// </summary>
    internal class WorldParser
    {
        private const string FirstLine = "C_Gartenzaun_Karol_World";
        private const string FileExtension = ".cskw";
        private const string LayerSeperator = "---";
        private const char EmptyCellID = '_';

        private int CurrentLine { get; set; }

        #region Save / Load
        public void Save(World world, string filePath)
        {
            using StreamWriter writer = new StreamWriter(filePath);
            writer.WriteLine(FirstLine);
            writer.WriteLine($"Size: {world.SizeX},{world.SizeY},{world.SizeZ}");

            for (int y = 0; y < world.SizeY; y++)
            {
                StringBuilder layer = new StringBuilder();
                bool isLayerEmpty = true;

                for (int z = world.SizeZ - 1; z >= 0; z--)
                {
                    int xCount = 1;
                    for(int i = world.SizeX - 1; i >= 0; i--)
                    {
                        if(world.HasCellAt(i, y, z, out WorldElement _))
                        {
                            xCount = i + 1;
                            break;
                        }
                    }
                    
                    for (int x = 0; x < xCount; x++)
                    {
                        if (!world.HasCellAt(x, y, z, out WorldElement cell))
                        {
                            layer.Append($"{EmptyCellID} ");
                            continue;
                        }

                        string metaData = $"({cell.Metadata})";
                        layer.Append($"{cell.ID}{(cell.Metadata != string.Empty ? metaData : string.Empty)} ");
                        isLayerEmpty = false;
                    }

                    layer.AppendLine();
                }

                if (isLayerEmpty)
                    break;

                writer.WriteLine(LayerSeperator);
                writer.Write(layer);
            }
        }

        public World Load(string filePath)
        {
            return Load(filePath, KarolWorldFormat.Auto);
        }

        public World Load(string filePath, KarolWorldFormat format)
        {
            return format switch
            {
                KarolWorldFormat.CSharp => LoadCSharp(filePath),
                KarolWorldFormat.Java => LoadJava(filePath),
                KarolWorldFormat.Auto => filePath.EndsWith(FileExtension) ? LoadCSharp(filePath) : LoadJava(filePath),
                _ => throw new NotImplementedException(),
            };
        }

        public World Load(StreamReader reader)
        {
            return Load(reader, KarolWorldFormat.Auto);
        }

        public World Load(StreamReader reader, KarolWorldFormat format)
        {
            byte[] buffer = new byte[FirstLine.Length];
            reader.BaseStream.Read(buffer, 0, FirstLine.Length);
            reader.BaseStream.Position = 0;
            string head = Encoding.ASCII.GetString(buffer);

            return format switch
            {
                KarolWorldFormat.CSharp => LoadCSharp(reader),
                KarolWorldFormat.Java => LoadJava(reader),
                KarolWorldFormat.Auto => head.StartsWith(FirstLine) ? LoadCSharp(reader) : LoadJava(reader),
                _ => throw new NotImplementedException(),
            };
        }

        public World LoadCSharp(string filePath)
        {
            using StreamReader reader = new StreamReader(filePath);
            return LoadCSharp(reader);
        }

        public World LoadCSharp(StreamReader reader)
        {
            if (NextLine(reader) != FirstLine)
                Console.WriteLine("Header nicht gefunden. Welt kann möglicherweise nicht geladen werden.");

            string sizeLine = NextLine(reader);
            var dimension = sizeLine.Trim().Replace(" ", "")[5..].Split(',');
            double iteration = 0;

            int xSize = int.Parse(dimension[0]);
            int ySize = int.Parse(dimension[1]);
            int zSize = int.Parse(dimension[2]);
            double count = xSize * ySize * zSize;

            World world = new World(xSize, ySize, zSize);

            EnableProgressBar(world, true);

            for (int y = 0; y < ySize; y++)
            {
                NextLine(reader);
                for (int z = zSize - 1; z >= 0; z--)
                {
                    string line = NextLine(reader);
                    if (line != null && line.StartsWith(LayerSeperator))
                        line = NextLine(reader);

                    if (line == null)
                    {
                        End();
                        return world;
                    }

                    string[] arr = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    int xCount = Math.Min(xSize, arr.Length);
                    for (int x = 0; x < xCount; x++)
                    {
                        iteration++;
                        if (IsPlaceholder(arr[x]))
                            continue;

                        if (world.HasCellAt(x, y, z, out WorldElement e))
                        {
                            reader.Close();
                            Kill();
                        }

                        char id = char.ToUpper(arr[x][0]);
                        GetMetaData(arr[x], out string metadata);
                        WorldElement cell = WorldElement.ForID(id);
                        
                        if (cell == null)
                        {
                            Console.WriteLine($"Ungültiges Zeichen in zeile {CurrentLine}");
                        }
                        else
                        {
                            world.SetCell(x, y, z, cell, false);
                            if (metadata != string.Empty)
                                cell.Metadata = metadata;
                        }
                    }

                    SetProgress(world, iteration / count);
                }
            }

            End();
            return world;

            void End()
            {
                EnableProgressBar(world, false);
                world.Redraw();
                reader.Close();
            }
        }

        public World LoadJava(string filePath)
        {
            using StreamReader reader = new StreamReader(filePath);
            return LoadJava(reader);
        }

        public World LoadJava(StreamReader reader)
        {
            string line = reader.ReadLine();
            int startIndex = StartIndex(line);
            string[] arr = Regex.Split(line[startIndex..], @"(?<=[om])");
            string[] posArr = line[..startIndex].Split(' ');

            int xSize = int.Parse(posArr[1]);
            int zSize = int.Parse(posArr[2]);
            int ySize = int.Parse(posArr[3]);

            int rXpos = int.Parse(posArr[4]);
            int rZpos = zSize - 1 - int.Parse(posArr[5]);
            int rYpos = int.Parse(posArr[6]);

            World world = new World(xSize, ySize, zSize);
            Dictionary<char, char> IDMap = new Dictionary<char, char>()
            {
                { 'n', EmptyCellID },
                { 'z', 'B' }
            };
            
            int x = 0;
            int z = zSize - 1;
            bool hasPlacedCube = false;

            EnableProgressBar(world, true);

            for (int i = 0; i < arr.Length; i++)
            {
                string[] blocks = arr[i].Split(' ', StringSplitOptions.RemoveEmptyEntries);
                foreach(var block in blocks)
                {
                    if (block[0] == 'o')
                        break;

                    char c = Translate(block[0]);
                    if (!hasPlacedCube)
                    {
                        var cell = WorldElement.ForID(c);
                        world.SetCell(x, z, cell, false);
                    }

                    hasPlacedCube = !hasPlacedCube && c == 'Q';
                }

                SetProgress(world, (double)(i + 1) / arr.Length);

                z--;
                if (z == -1)
                {
                    x++;
                    z = zSize - 1;

                    if (x == xSize)
                        break;
                }          
            }

            Robot r = new Robot(rXpos, rZpos, world, Direction.South, false);

            EnableProgressBar(world, false);
            world.Redraw();
            reader.Close();
            return world;

            char Translate(char c)
            {
                if(IDMap.ContainsKey(c))
                    return IDMap[c];

                return char.ToUpper(c);
            }

            int StartIndex(string str)
            {
                int remainingBlanks = 7;
                for(int i = 0; i < str.Length; i++)
                {
                    if(str[i] == ' ')
                    {
                        remainingBlanks--;
                        if (remainingBlanks == 0)
                            return i;
                    }
                }

                return 32;
            }
        }

        public World LoadImage(string filePath, int worldHeight = 5)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException("Datei nicht gefunden", filePath);

            var map = new Bitmap(filePath);
            World world = new World(map.Width, worldHeight, map.Height);

            EnableProgressBar(world, true);

            for (int x = 0; x < map.Width; x++)
            {
                for (int y = 0; y < map.Height; y++)
                {
                    Color color = map.GetPixel(x, y);
                    if (color.A == 0)
                        continue;

                    world.SetGridElement(x, 0, map.Height - y - 1, new Brick(color));
                }

                double val = (double)(x + 1) / map.Width;
                SetProgress(world, val);
            }

            EnableProgressBar(world, false);
            world.Redraw();
            map.Dispose();
            return world;
        }
        #endregion

        #region Generate World
        public static World Generate(int xSize, int ySize, int zSize, Dictionary<Position, WorldElement> elements)
        {
            World w = new World(xSize, ySize, zSize);

            foreach(var e in elements)
            {
                w.SetCell(e.Key, e.Value);
            }

            return w;
        }
        #endregion

        #region Private
        private void Kill()
        {
            throw new InvalidDataException($"Map Datei ist fehlerhaft! Fehler in Zeile: {CurrentLine}");
        }

        private string NextLine(StreamReader reader)
        {
            CurrentLine++;
            return reader.ReadLine();
        }

        private bool IsPlaceholder(string str)
        {
            return str.StartsWith(EmptyCellID) || str.StartsWith("D");
        }

        private bool HasMetadata(string str)
        {
            return str.Length != 1;
        }

        private bool GetMetaData(string str, out string metadata)
        {
            if(str.Length == 1)
            {
                metadata = string.Empty;
                return false;
            }

            metadata = str[2..^1];
            return true;
        }
        #endregion

        #region ProgressBar
        private void EnableProgressBar(World targetWorld, bool enabled)
        {
            targetWorld.InvokeFormMethod(() =>
            {
                targetWorld.WorldForm.ProgressBar.Visible = enabled;
                targetWorld.WorldForm.ProgressBar.Enabled = enabled;
                targetWorld.WorldForm.ProgressBar.Value = 0;
            });
        }

        private void SetProgress(World targetWorld, double value)
        {
            targetWorld.InvokeFormMethod(() =>
            {
                targetWorld.WorldForm.ProgressBar.Value = (int)Math.Round(value * 100);
            });
        }
        #endregion
    }
}
