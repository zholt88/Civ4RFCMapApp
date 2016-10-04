using Civ4RFCMapApp.Core.Models;

namespace Civ4RFCMapApp.Core.Interfaces
{
    public interface IMapDrawer
    {
        void Draw(Map map, string path);
    }
}