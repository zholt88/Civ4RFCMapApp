using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Civ4RFCMapApp.Core.Enums;
using Civ4RFCMapApp.Core.Models;

namespace Civ4RFCMapApp.Implementation
{
    public class ImageReader
    {
        private readonly Dictionary<Stability, List<Color>> _stabilityColors = new Dictionary<Stability, List<Color>>
        {
            { Stability.CannotSettle, new List<Color> { Color.FromArgb(0, 0, 200), Color.FromArgb(64, 32, 16) } },
            { Stability.Bad, new List<Color> { Color.FromArgb(255, 155, 0) } },
            { Stability.Worst, new List<Color> { Color.FromArgb(255, 0, 0) } },
            { Stability.Expansion, new List<Color> { Color.FromArgb(0, 255, 0) } },
            { Stability.Contested, new List<Color> { Color.FromArgb(255, 255, 0) } },
            { Stability.Home, new List<Color> { Color.FromArgb(0, 155, 0) } }
        };

        public void PopulateStabilities(Map map, Image stabilityMap)
        {
            var bitmap = stabilityMap as Bitmap;
            using (bitmap)
            {
                for (int i = 0; i < bitmap.Width; i++)
                {
                    for (int j = 0; j < bitmap.Height; j++)
                    {
                        map.Plots[i, bitmap.Height - j - 1].Stability = _stabilityColors.First(m => m.Value.Contains(bitmap.GetPixel(i, j))).Key;
                    }
                }
            }
        }

        public List<Color> GetSingleMapColors(string fileName)
        {
            var image = new Bitmap(fileName);
            var colors = new List<Color>();
            for (int i = 0; i < image.Width; i++)
            {
                for (int j = 0; j < image.Height; j++)
                {
                    colors.Add(image.GetPixel(i, j));
                }
            }
            return colors;
        }

        public Dictionary<Stability, int> GetStabilityColorCounts(List<Color> colors)
        {
            var allStabilityColors = new Dictionary<Stability, List<Color>>
            {
                { Stability.CannotSettle, new List<Color> { Color.FromArgb(0, 0, 200), Color.FromArgb(64, 32, 16) } },
                { Stability.Bad, new List<Color> { Color.FromArgb(255, 155, 0) } },
                { Stability.Worst, new List<Color> { Color.FromArgb(255, 0, 0) } },
                { Stability.Expansion, new List<Color> { Color.FromArgb(0, 255, 0) } },
                { Stability.Contested, new List<Color> { Color.FromArgb(255, 255, 0) } },
                { Stability.Home, new List<Color> { Color.FromArgb(0, 155, 0) } }
            };
            var stabilityColorCounts = new Dictionary<Stability, int>
            {
                { Stability.Home, 0 },
                { Stability.Expansion, 0 },
                { Stability.Contested, 0 },
                { Stability.Bad, 0 },
                { Stability.Worst, 0 },
                { Stability.CannotSettle, 0 },
            };
            foreach (Color color in colors)
            {
                foreach (KeyValuePair<Stability, List<Color>> stabilityColors in allStabilityColors)
                {
                    foreach (Color stabilityColor in stabilityColors.Value)
                    {
                        if (color == stabilityColor)
                        {
                            stabilityColorCounts[stabilityColors.Key]++;
                        }
                    }
                }
            }
            return stabilityColorCounts;
        }
    }
}