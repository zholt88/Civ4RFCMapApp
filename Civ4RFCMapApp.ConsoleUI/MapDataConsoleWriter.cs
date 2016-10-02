using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Civ4RFCMapApp.Core.Enums;
using Civ4RFCMapApp.Implementation;

namespace Civ4RFCMapApp.ConsoleUI
{
    public class MapDataConsoleWriter
    {
        private readonly ImageReader _imageReader = new ImageReader();

        public void WriteCivOrderedByStabilityColor(Dictionary<Civilization, Dictionary<Stability, int>> allStabilityColorCounts, Stability stabilityColor)
        {
            Console.Clear();
            Console.WriteLine($"***{stabilityColor}***");
            foreach (KeyValuePair<Civilization, Dictionary<Stability, int>> count in allStabilityColorCounts.OrderBy(m => m.Value[stabilityColor]))
            {
                string tabs = count.Key.ToString().Length > 7 ? "\t" : "\t\t";
                Console.WriteLine($"{count.Key}{tabs}{count.Value[stabilityColor]}");
            }
        }

        public void WriteImageColorCounts(string fileName)
        {
            List<Color> colors = _imageReader.GetSingleMapColors(fileName);
            Dictionary<Stability, int> stabilityColorCounts = _imageReader.GetStabilityColorCounts(colors);

            Console.WriteLine(fileName);
            foreach (KeyValuePair<Stability, int> stabilityColorCount in stabilityColorCounts.Where(m => m.Key != Stability.CannotSettle))
            {
                Console.WriteLine($"{stabilityColorCount.Key} Count: {stabilityColorCount.Value}");
            }
        }
    }
}
