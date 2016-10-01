using Civ4RFCMapApp.Core.Enums;

namespace Civ4RFCMapApp.Core.Models
{
    public class Plot
    {
        public PlotType Type { get; set; }
        public Terrain Terrain { get; set; }
        public Feature Feature { get; set; }
        public bool IsWestOfRiver { get; set; }
        public bool IsNorthOfRiver { get; set; }
    }
}