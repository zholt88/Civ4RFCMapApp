using System;
using System.Drawing;
using Civ4RFCMapApp.Core.Enums;
using Civ4RFCMapApp.Core.Models;

namespace Civ4RFCMapApp.Implementation
{
    public class MapDrawer
    {
        public void Draw(Map map, string path)
        {
            using (var bitmap = new Bitmap(map.Width * 2, map.Height))
            {
                for (int i = 0; i < map.Width; i++)
                {
                    for (int j = 0; j < map.Height; j++)
                    {
                        bitmap.SetPixel(i, j, GetColor(map.Plots[i, map.Height - j - 1]));
                    }
                }

                for (int i = map.Width; i < map.Width * 2; i++)
                {
                    for (int j = 0; j < map.Height; j++)
                    {
                        bitmap.SetPixel(i, map.Height - j - 1, GetStabilityColor(map.Plots[i - map.Width, j]));
                    }
                }


                bitmap.Save(path);
            }
        }

        private Color GetStabilityColor(Plot mapPlot)
        {
            switch (mapPlot.Stability)
            {
                case Stability.CannotSettle:
                    return Color.FromArgb(0, 0, 0);
                case Stability.Bad:
                    return Color.FromArgb(255, 155, 0);
                case Stability.Worst:
                    return Color.FromArgb(255, 0, 0);
                case Stability.Contested:
                    return Color.FromArgb(255, 255, 0);
                case Stability.Expansion:
                    return Color.FromArgb(0, 255, 0);
                case Stability.Home:
                    return Color.FromArgb(0, 155, 0);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private Color GetColor(Plot plot)
        {
            if (plot.Feature == Feature.Ice)
            {
                return Color.FromArgb(255, 255, 255, 255);
            }
            if (plot.Type == PlotType.Mountain)
            {
                return Color.FromArgb(255, 255/8, 255/8, 255/8);
            }
            switch (plot.Terrain)
            {
                case Terrain.Ocean:
                    return Color.FromArgb(255, 0, 0, 255*3/4);
                case Terrain.Coast:
                    return Color.FromArgb(255, 255*1/8, 255*3/8, 255*7/8);
                case Terrain.Grass:
                    switch (plot.Feature)
                    {
                        case Feature.Forest:
                            return Color.FromArgb(255, 0, 255*5/8, 0);
                        case Feature.Jungle:
                            return Color.FromArgb(255, 255*3/8, 255*6/8, 0);
                        default:
                            return Color.FromArgb(255, 0, 255, 0);
                    }
                case Terrain.Plains:
                    switch (plot.Feature)
                    {
                        case Feature.Forest:
                            return Color.FromArgb(255, 255*9/16, 255*11/16, 255*0/8);
                        case Feature.Jungle:
                            return Color.FromArgb(255, 255*6/8, 255*13/16, 255*1/8);
                        default:
                            return Color.FromArgb(255, 255*7/8, 255*3/4, 255/2);
                    }
                case Terrain.Desert:
                    switch (plot.Feature)
                    {
                        case Feature.FloodPlains:
                            return Color.FromArgb(255, 255*14/16, 255, 255*7/16);
                        default:
                            return Color.FromArgb(255, 255, 255, 255/2);
                    }
                case Terrain.Tundra:
                    return plot.Feature == Feature.Forest 
                        ? Color.FromArgb(255, 255*3/8, 255*4/8, 255*3/8) 
                        : Color.FromArgb(255, 255*3/8, 255*3/8, 255*3/8);
                case Terrain.Snow:
                    return plot.Feature == Feature.Forest 
                        ? Color.FromArgb(255, 255*7/8, 255, 255*7/8) 
                        : Color.FromArgb(255, 255*7/8, 255*7/8, 255*7/8);
                case Terrain.Marsh:
                    return Color.FromArgb(255, 0, 255/2, 255/2);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}