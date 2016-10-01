using Civ4RFCMapApp.Implementation;

namespace Civ4RFCMapApp.ConsoleUI
{
    public class Program
    {
        private const string MapFilePath3000Bc = @"C:\Users\Zach\Desktop\Civ4 RFC\RFCMarathon Maps\RFC 3000 BC unlocked.CIVBEYONDSWORDWBSAVE";
        private const string MapFilePath600Ad = @"C:\Users\Zach\Desktop\Civ4 RFC\RFCMarathon Maps\RFC 600 AD unlocked.CIVBEYONDSWORDWBSAVE";

        public static void Main()
        {
            //var imageReader = new ImageReader();
            //var mapDataWriter = new MapDataWriter();
            //Dictionary<Civilization, List<Color>> civMapColors = imageReader.GetColorsForAllMaps();
            //Dictionary<Civilization, Dictionary<StabilityColor, int>> allStabilityColorCounts = civMapColors.ToDictionary(m => m.Key, m => imageReader.GetStabilityColorCounts(m.Value));
            //while (true)
            //{
            //    foreach (StabilityColor stabilityColor in Enum.GetValues(typeof(StabilityColor)).Cast<StabilityColor>().Where(m => m != StabilityColor.Mountain && m != StabilityColor.CannotSettle))
            //    {
            //        mapDataWriter.WriteCivOrderedByStabilityColor(allStabilityColorCounts, stabilityColor);
            //        ConsoleKeyInfo input = Console.ReadKey();
            //        if (input.KeyChar == 'q')
            //        {
            //            return;
            //        }
            //    }
            //}

            var mapFileReader = new MapFileReader();

            var map = mapFileReader.GetMap(MapFilePath3000Bc);

        }
    }
}
