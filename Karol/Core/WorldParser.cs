using Karol.Core.WorldElements;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Karol.Core
{
    internal class WorldParser
    {
        private const string FirstLine = "C_Gartenzaun_Karol_World";
        private const string LayerSeperator = "---";
        private const char EmptyCellID = '0';

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
                            layer.Append("0");
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
            using StreamReader reader = new StreamReader(filePath);
            if (reader.ReadLine() != FirstLine)
                Console.WriteLine("Header nicht gefunden. Welt kann möglicherweise nicht geladen werden.");

            string sizeLine = reader.ReadLine();
            var dimension = sizeLine.Substring(5).Split(',');

            int xSize = int.Parse(dimension[0]);
            int ySize = int.Parse(dimension[1]);
            int zSize = int.Parse(dimension[2]);

            World world = new World(xSize, ySize, zSize);

            while (!reader.EndOfStream)
            {            
                for (int y = 0; y < ySize; y++)
                {
                    reader.ReadLine();
                    for (int z = zSize - 1; z >= 0; z--)
                    {
                        string line = reader.ReadLine();
                        if (line == LayerSeperator)
                            line = reader.ReadLine();

                        if (line == null)
                            return world;

                        int xCount = Math.Min(xSize, line.Length);
                        for (int x = 0; x < xCount; x++)
                        {
                            if (line[x] == EmptyCellID)
                                continue;

                            WorldElement cell = line[x] == 'R' ? new Robot(x, z, world) : WorldElement.ForID(line[x]);
                            world.SetCell(x, y, z, cell, false);
                        }
                    }
                }
            }

            return world;
        }
    }
}
