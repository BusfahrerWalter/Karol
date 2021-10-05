using Karol.Core.WorldElements;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Karol.Core
{
    public enum KarolWorldFormat
    {
        CSKW,
        KDW
    }

    internal class WorldParser
    {
        private const string FirstLine = "C_Gartenzaun_Karol_World";
        private const string LayerSeperator = "---";
        private const char EmptyCellID = '_';

        public void Save(World world, string filePath)
        {
            using StreamWriter writer = new StreamWriter(filePath);
            writer.WriteLine(FirstLine);
            writer.WriteLine($"Size: {world.SizeX},{world.SizeY},{world.SizeZ}");

            for (int y = 0; y < world.SizeY; y++)
            {
                StringBuilder layer = new StringBuilder();
                for (int z = world.SizeZ - 1; z >= 0; z--)
                {
                    for (int x = 0; x < world.SizeX; x++)
                    {
                        if (!world.HasCellAt(x, y, z, out WorldElement cell))
                        {
                            layer.Append(EmptyCellID);
                            continue;
                        }

                        layer.Append(cell.ID);
                    }

                    layer.AppendLine();
                }

                writer.WriteLine(LayerSeperator);
                writer.Write(layer);
            }
        }

        public World Load(string filePath)
        {
            return Load(filePath, KarolWorldFormat.CSKW);
        }

        public World Load(string filePath, KarolWorldFormat format)
        {
            int currentLine = 0;

            using StreamReader reader = new StreamReader(filePath);
            if (NextLine(reader, ref currentLine) != FirstLine)
                Console.WriteLine("Header nicht gefunden. Welt kann möglicherweise nicht geladen werden.");

            string sizeLine = NextLine(reader, ref currentLine);
            var dimension = sizeLine.Trim().Replace(" ", "")[5..].Split(',');

            int xSize = int.Parse(dimension[0]);
            int ySize = int.Parse(dimension[1]);
            int zSize = int.Parse(dimension[2]);

            World world = new World(xSize, ySize, zSize);

            while (!reader.EndOfStream)
            {            
                for (int y = 0; y < ySize; y++)
                {
                    NextLine(reader, ref currentLine);
                    for (int z = zSize - 1; z >= 0; z--)
                    {
                        string line = NextLine(reader, ref currentLine);
                        if (line != null && line.StartsWith(LayerSeperator))
                            line = NextLine(reader, ref currentLine);

                        if (line == null)
                            return world;

                        int xCount = Math.Min(xSize, line.Length);
                        for (int x = 0; x < xCount; x++)
                        {
                            if (line[x] == EmptyCellID || line[x] == 'D')
                                continue;

                            if (world.HasCellAt(x, y, z, out WorldElement e))
                            {
                                reader.Close();
                                throw new InvalidDataException($"Map Datei ist fehlerhaft! Fehler in Zeile: {currentLine}");
                            }

                            WorldElement cell = line[x] == 'R' ? WorldElement.ForID(line[x], x, z, world) : WorldElement.ForID(line[x]);
                            world.SetCell(x, y, z, cell, false);
                        }
                    }
                }
            }

            return world;
        }

        private string NextLine(StreamReader reader, ref int currentLine)
        {
            currentLine++;
            return reader.ReadLine();
        }
    }
}
