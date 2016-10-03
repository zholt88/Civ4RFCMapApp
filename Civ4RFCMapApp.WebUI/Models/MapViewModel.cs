using Civ4RFCMapApp.Core.Models;

namespace Civ4RFCMapApp.WebUI.Models
{
    public class MapViewModel
    {
        public string Message { get; set; }
        public string Name { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string Dimensions => $"{Width}x{Height}";
        public string ImagePath { get; set; }
    }
}