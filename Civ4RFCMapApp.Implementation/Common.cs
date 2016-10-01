using Civ4RFCMapApp.Core.Enums;

namespace Civ4RFCMapApp.Implementation
{
    public class Common
    {
        public string GetStabilityMapFileName(Civilization civilization)
        {
            const string path = @"C:\Users\Zach\Desktop\Civ4 RFC\RFCMarathon Maps\Stabililty Maps\";
            const string extension = ".gif";
            return $"{path}{civilization}{extension}";
        }
    }
}