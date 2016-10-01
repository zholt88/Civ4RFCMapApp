using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Civ4RFCMapApp.Core.Enums;
using Civ4RFCMapApp.Core.Models;
using Civ4RFCMapApp.Extensions;

namespace Civ4RFCMapApp.Implementation
{
    public class MapFileReader
    {
        public Map GetMap(string filePath)
        {
            return GetMap(File.ReadAllLines(filePath), filePath);
        }

        public Map GetMap(byte[] bytes, string filePath)
        {
            string[] mapFileLines = Encoding.Default.GetString(bytes, 0, bytes.Length - 1).Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            return GetMap(mapFileLines, filePath);
        }

        public Map GetMap(IReadOnlyList<string> mapFileLines, string filePath)
        {
            Tuple<int, int> mapDimensions = GetMapDimensions(mapFileLines);
            return mapDimensions.Item1 == 0 || mapDimensions.Item2 == 0
                ? null
                : new Map
                {
                    Name = GetMapName(filePath),
                    Width = mapDimensions.Item1,
                    Height = mapDimensions.Item2,
                    Plots = GetPlots(mapDimensions.Item1, mapDimensions.Item2, mapFileLines)
                };
        }

        private string GetMapName(string filePath)
        {
            string[] nameAndExtension = Path.GetFileName(filePath).Split('.');
            return string.Join(".", nameAndExtension.Take(nameAndExtension.Length - 1));
        }

        private Tuple<int, int> GetMapDimensions(IReadOnlyList<string> mapFileLines)
        {
            int beginMapIndex = mapFileLines.FindIndex(m => m == "BeginMap");
            int endMapIndex = mapFileLines.FindIndex(m => m == "EndMap");
            int mapWidth = GetMapWidth(beginMapIndex, endMapIndex, mapFileLines);
            int mapHeight = GetMapHeight(beginMapIndex, endMapIndex, mapFileLines);
            var mapDimensions = new Tuple<int, int>(mapWidth, mapHeight);
            return mapDimensions;
        }

        private int GetMapHeight(int beginMapIndex, int endMapIndex, IReadOnlyList<string> mapFileLines)
        {
            int mapHeight = 0;
            for (int i = beginMapIndex; i < endMapIndex; i++)
            {
                if (mapFileLines[i].Contains("grid height"))
                {
                    mapHeight = int.Parse(mapFileLines[i].Split('=')[1]);
                    break;
                }
            }
            return mapHeight;
        }

        private int GetMapWidth(int beginMapIndex, int endMapIndex, IReadOnlyList<string> mapFileLines)
        {
            int mapWidth = 0;
            for (int i = beginMapIndex; i < endMapIndex; i++)
            {
                if (mapFileLines[i].Contains("grid width"))
                {
                    mapWidth = int.Parse(mapFileLines[i].Split('=')[1]);
                    break;
                }
            }
            return mapWidth;
        }

        private Plot[,] GetPlots(int mapWidth, int mapHeight, IEnumerable<string> mapFileLines)
        {
            var plots = new Plot[mapWidth, mapHeight];
            bool inPlot = false;
            int x = 0;
            int y = 0;
            foreach (string line in mapFileLines)
            {
                inPlot = GetInPlot(line, inPlot);
                if (inPlot)
                {
                    if (line.StartsWith("\tx="))
                    {
                        x = int.Parse(line.Split('=')[1].Split(',')[0]);
                        y = int.Parse(line.Split('=')[2]);
                        plots[x, y] = new Plot();
                    }
                    else if (line.StartsWith("\tFeatureType="))
                    {
                        plots[x, y].Feature = GetFeature(line);
                    }
                    else if (line.StartsWith("\tTerrainType="))
                    {
                        plots[x, y].Terrain = GetTerrain(line);
                    }
                    else if (line.StartsWith("\tPlotType="))
                    {
                        plots[x, y].Type = (PlotType)int.Parse(line.Split('=')[1]);
                    }
                }
            }
            return plots;
        }

        private bool GetInPlot(string line, bool current)
        {
            switch (line)
            {
                case "BeginPlot":
                    return true;
                case "EndPlot":
                    return false;
                default:
                    return current;
            }
        }

        private Terrain GetTerrain(string line)
        {
            string terrainString = line.Split('=')[1].Split(',')[0];
            return Enum.GetValues(typeof(Terrain)).Cast<Terrain>().First(m => terrainString.Contains(m.ToString().ToUpper()));
        }

        private Feature GetFeature(string line)
        {
            string featureString = line.Split('=')[1].Split(',')[0];
            return Enum.GetValues(typeof(Terrain)).Cast<Feature>().FirstOrDefault(m => featureString.Replace("_", "").Contains(m.ToString().ToUpper()));
        }
    }
}