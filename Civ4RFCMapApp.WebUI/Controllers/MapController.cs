using System.IO;
using System.Web;
using System.Web.Mvc;
using Civ4RFCMapApp.Core.Models;
using Civ4RFCMapApp.Implementation;
using Civ4RFCMapApp.WebUI.Models;

namespace Civ4RFCMapApp.WebUI.Controllers
{
    public class MapController : Controller
    {
        private readonly MapFileReader _mapFileReader = new MapFileReader();
        private readonly MapDrawer _mapDrawer = new MapDrawer();
        private readonly ImageResizer _imageResizer = new ImageResizer();

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(HttpPostedFileBase file)
        {
            MapViewModel viewModel;
            Map map = file == null ? null : _mapFileReader.GetMap(GetBytes(file), file.FileName);
            if (map == null)
            {
                viewModel = new MapViewModel
                {
                    Name = "Invalid Map"
                };
            }
            else
            {
                string path = Path.Combine(Server.MapPath("~/Content/Images/UploadedMaps"), $"{map.Name}.bmp");
                _mapDrawer.Draw(map, path);
                _imageResizer.Resize(path, 4, 4);
                viewModel = new MapViewModel
                {
                    Width = map.Width,
                    Height = map.Height,
                    Name = map.Name,
                    ImagePath = $"Content/Images/UploadedMaps/{map.Name}.bmp"
                };
            }
            return View(viewModel);
        }

        private static byte[] GetBytes(HttpPostedFileBase file)
        {
            byte[] bytes;
            using (var memoryStream = new MemoryStream())
            {
                file.InputStream.CopyTo(memoryStream);
                bytes = memoryStream.GetBuffer();
            }
            return bytes;
        }
    }
}