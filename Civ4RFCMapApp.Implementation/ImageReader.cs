using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Civ4RFCMapApp.Core.Enums;

namespace Civ4RFCMapApp.Implementation
{
    public class ImageReader
    {
        private readonly Common _common = new Common();

        public Dictionary<Civilization, List<Color>> GetColorsForAllMaps()
        {
            List<Civilization> civilizations = Enum.GetValues(typeof(Civilization)).Cast<Civilization>().ToList();
            return civilizations.ToDictionary(civilization => civilization, GetSingleMapColors);
        }

        public List<Color> GetSingleMapColors(Civilization civilization)
        {
            var image = new Bitmap(_common.GetStabilityMapFileName(civilization));
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