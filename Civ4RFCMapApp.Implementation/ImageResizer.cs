using System.Drawing;
using Civ4RFCMapApp.Core.Enums;

namespace Civ4RFCMapApp.Implementation
{
    public class ImageResizer
    {
        private readonly Common _common = new Common();

        public void ResizeImage(Civilization civilization)
        {
            string fileName = _common.GetStabilityMapFileName(civilization);
            var original = new Bitmap(fileName);
            var resized = new Bitmap(original, new Size(original.Width / 4, original.Height / 4));
            original.Dispose();
            resized.Save(fileName);
            resized.Dispose();
        }
    }
}
