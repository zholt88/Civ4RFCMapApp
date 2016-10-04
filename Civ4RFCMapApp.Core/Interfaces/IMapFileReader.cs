using System.Collections.Generic;
using Civ4RFCMapApp.Core.Models;

namespace Civ4RFCMapApp.Core.Interfaces
{
    public interface IMapFileReader
    {
        Map GetMap(string filePath);
        Map GetMap(byte[] bytes, string filePath);
        Map GetMap(IReadOnlyList<string> mapFileLines, string filePath);
    }
}