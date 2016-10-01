using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Civ4RFCMapApp.Core.Enums;
using Civ4RFCMapApp.Core.Models;

namespace Civ4RFCMapApp.Implementation
{
    public class MapFileReader
    {
        public Map GetMap(string filePath)
        {
            var mapFileLines = File.ReadAllLines(filePath).ToList();
            int beginMapIndex = mapFileLines.FindIndex(m => m == "BeginMap");
            int endMapIndex = mapFileLines.FindIndex(m => m == "EndMap");
            int mapWidth = GetMapWidth(beginMapIndex, endMapIndex, mapFileLines);
            int mapHeight = GetMapHeight(beginMapIndex, endMapIndex, mapFileLines);

            var nameAndExtension = Path.GetFileName(filePath).Split('.');
            var map = new Map
            {
                Name = string.Join(".", nameAndExtension.Take(nameAndExtension.Length - 1)),
                Width = mapWidth,
                Height = mapHeight,
                Plots = new Plot[mapWidth, mapHeight]
            };

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
                        map.Plots[x, y] = new Plot();
                    }
                    else if (line.StartsWith("\tFeatureType="))
                    {
                        map.Plots[x, y].Feature = GetFeature(line);
                    }
                    else if (line.StartsWith("\tTerrainType="))
                    {
                        map.Plots[x, y].Terrain = GetTerrain(line);
                    }
                    else if (line.StartsWith("\tPlotType="))
                    {
                        map.Plots[x, y].Type = (PlotType)int.Parse(line.Split('=')[1]);
                    }
                }
            }

            return map;
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

        private static bool GetInPlot(string line, bool current)
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

        private int GetMapHeight(int beginMapIndex, int endMapIndex, List<string> mapFileLines)
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

        private int GetMapWidth(int beginMapIndex, int endMapIndex, List<string> mapFileLines)
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
    }
}