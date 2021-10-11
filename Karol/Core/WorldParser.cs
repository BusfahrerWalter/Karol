using Karol.Core.WorldElements;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Karol.Core
{
    /// <summary>
    /// Dateiformat in dem die Karol Welt gespeichert ist.
    /// </summary>
    public enum KarolWorldFormat
    {
        CSharp,
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

        private World LoadCSharp(string filePath)
        {
            using StreamReader reader = new StreamReader(filePath);
            if (NextLine(reader) != FirstLine)
                Console.WriteLine("Header nicht gefunden. Welt kann möglicherweise nicht geladen werden.");

            string sizeLine = NextLine(reader);
            var dimension = sizeLine.Trim().Replace(" ", "")[5..].Split(',');

            int xSize = int.Parse(dimension[0]);
            int ySize = int.Parse(dimension[1]);
            int zSize = int.Parse(dimension[2]);

            World world = new World(xSize, ySize, zSize);

            while (!reader.EndOfStream)
            {            
                for (int y = 0; y < ySize; y++)
                {
                    NextLine(reader);
                    for (int z = zSize - 1; z >= 0; z--)
                    {
                        string line = NextLine(reader);
                        if (line != null && line.StartsWith(LayerSeperator))
                            line = NextLine(reader);

                        if (line == null)
                            return world;

                        string[] arr = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                        int xCount = Math.Min(xSize, arr.Length);
                        for (int x = 0; x < xCount; x++)
                        {
                            if (IsPlaceholder(arr[x]))
                                continue;

                            if (world.HasCellAt(x, y, z, out WorldElement e))
                            {
                                reader.Close();
                                Kill();
                            }

                            char id = char.ToUpper(arr[x][0]);
                            WorldElement cell = id == 'R' ? 
                                new Robot(x, z, world, Direction.North, false) : 
                                WorldElement.ForID(id);

                            if(cell == null)
                            {
                                reader.Close();
                                Kill();
                            }

                            if (GetMetaData(arr[x], out string metadata))
                                cell.Metadata = metadata;

                            world.SetCell(x, y, z, cell, false);
                        }
                    }
                }
            }

            world.Redraw();
            reader.Close();
            return world;
        }

        private World LoadJava(string filePath)
        {
            using StreamReader reader = new StreamReader(filePath);
            string[] arr = NextLine(reader).Split(" ");

            int xSize = int.Parse(arr[1]);
            int zSize = int.Parse(arr[2]);
            int ySize = int.Parse(arr[3]);

            int rXpos = int.Parse(arr[4]);
            int rZpos = zSize - 1 - int.Parse(arr[5]);
            int rYpos = int.Parse(arr[6]);

            World world = new World(xSize, ySize, zSize);
            Dictionary<char, char> IDMap = new Dictionary<char, char>()
            {
                { 'n', EmptyCellID },
                { 'z', 'B' },
                { 'm', 'M' }
            };
            
            int x = 0;
            int z = zSize - 1;
            bool hasPlacedCube = false;

            for(int i = 7; i < arr.Length; i++)
            {
                for (int y = 0; y < world.SizeY; y++)
                {
                    char c = Translate(arr[i][0]);
                    if (!hasPlacedCube)
                    {
                        world.SetCell(x, y, z, WorldElement.ForID(c), false);
                    }

                    hasPlacedCube = !hasPlacedCube && c == 'Q';
                    i++;
                }

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

            world.Redraw();
            reader.Close();
            return world;

            char Translate(char c)
            {
                if(IDMap.ContainsKey(c))
                    return IDMap[c];

                return char.ToUpper(c);
            }
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
    }
}
