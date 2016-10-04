using System.Collections.Generic;
using System.Drawing;
using Civ4RFCMapApp.Core.Enums;
using Civ4RFCMapApp.Core.Models;

namespace Civ4RFCMapApp.Core.Interfaces
{
    public interface IImageReader
    {
        void PopulateStabilities(Map map, Image stabilityMap);
        List<Color> GetSingleMapColors(string fileName);
        Dictionary<Stability, int> GetStabilityColorCounts(List<Color> colors);
    }
}