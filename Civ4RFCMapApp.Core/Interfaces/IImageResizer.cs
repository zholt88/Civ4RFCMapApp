namespace Civ4RFCMapApp.Core.Interfaces
{
    public interface IImageResizer
    {
        void Resize(string imagePath, double widthRatio, double heightRatio);
    }
}