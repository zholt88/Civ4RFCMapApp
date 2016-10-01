namespace Civ4RFCMapApp.Core.Models
{
    public class Map
    {
        public string Name { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public Plot[,] Plots { get; set; }
    }
}