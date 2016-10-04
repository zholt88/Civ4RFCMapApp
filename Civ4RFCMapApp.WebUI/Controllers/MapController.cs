using System.IO;
using System.Web;
using System.Web.Mvc;
using Civ4RFCMapApp.Core.Interfaces;
using Civ4RFCMapApp.Core.Models;
using Civ4RFCMapApp.WebUI.Models;

namespace Civ4RFCMapApp.WebUI.Controllers
{
    public class MapController : Controller
    {
        private readonly IMapFileReader _mapFileReader;
        private readonly IMapDrawer _mapDrawer;
        private readonly IImageResizer _imageResizer;

        public MapController(IMapFileReader mapFileReader, IMapDrawer mapDrawer, IImageResizer imageResizer)
        {
            _mapFileReader = mapFileReader;
            _mapDrawer = mapDrawer;
            _imageResizer = imageResizer;
        }

        private const string UploadsFolder = "Content/Images/UploadedMaps";

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(HttpPostedFileBase file)
        {
            Map map = file == null ? null : _mapFileReader.GetMap(GetBytes(file), file.FileName);
            MapViewModel viewModel = map == null
                ? new MapViewModel { Message = "Invalid Map Text File" }
                : GetMapViewModel(map);

            return View(viewModel);
        }

        private MapViewModel GetMapViewModel(Map map)
        {
            string path = Path.Combine(Server.MapPath($"~/{UploadsFolder}"), $"{map.Name}.bmp");
            _mapDrawer.Draw(map, path);
            _imageResizer.Resize(path, 4, 4);
            return new MapViewModel
            {
                Width = map.Width,
                Height = map.Height,
                Name = map.Name,
                ImagePath = $"{UploadsFolder}/{map.Name}.bmp"
            };
        }

        private static byte[] GetBytes(HttpPostedFileBase file)
        {
            using (var memoryStream = new MemoryStream())
            {
                file.InputStream.CopyTo(memoryStream);
                return memoryStream.GetBuffer();
            }
        }
    }
}