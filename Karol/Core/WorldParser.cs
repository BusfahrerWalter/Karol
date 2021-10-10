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

    internal class WorldParser
    {
        private const string FirstLine = "C_Gartenzaun_Karol_World";
        private const string FileExtension = ".cskw";
        private const string LayerSeperator = "---";
        private const char EmptyCellID = '_';

        private int CurrentLine { get; set; }

        #region Public
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
                    for (int x = 0; x < world.SizeX; x++)
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

                            char id = arr[x][0];
                            WorldElement cell = id == 'R' ? 
                                WorldElement.ForID(id, x, z, world) : 
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
            int ySize = int.Parse(arr[2]);
            int zSize = int.Parse(arr[3]);

            World world = new World(xSize, ySize, zSize);
            Dictionary<char, char> IDMap = new Dictionary<char, char>()
            {
                { 'n', EmptyCellID },
                { 'o', 'B' }
            };

            int x = xSize - 1;
            int z = 0;

            for(int i = 6; i < arr.Length; i++)
            {
                char c = Translate(arr[i][0]);
                WorldElement cell = WorldElement.ForID(c);

                world.SetCell(x, z, cell, false);

                z++;
                if (z == zSize - 1)
                {
                    x--;
                    z = 0;
                }
                    
            }

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

        #region Privat
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
