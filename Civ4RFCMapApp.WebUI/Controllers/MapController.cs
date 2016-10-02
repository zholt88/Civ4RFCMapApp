using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
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
        private readonly ImageReader _imageReader = new ImageReader();

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(IEnumerable<HttpPostedFileBase> files)
        {
            MapViewModel viewModel;

            files = files?.ToList();
            HttpPostedFileBase file1 = files?.FirstOrDefault();
            HttpPostedFileBase file2 = files?.LastOrDefault();

            Map map = file1 == null ? null : _mapFileReader.GetMap(GetBytes(file1), file1.FileName);
            if (file1 == null || map == null)
            {
                viewModel = new MapViewModel
                {
                    Name = "Invalid Map"
                };
            }
            else if (file2 == null)
            {
                viewModel = new MapViewModel
                {
                    Name = "Invalid Stability Image"
                };
            }
            else
            {
                _imageReader.PopulateStabilities(map, Image.FromStream(file2.InputStream));
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
            using (var memoryStream = new MemoryStream())
            {
                file.InputStream.CopyTo(memoryStream);
                return memoryStream.GetBuffer();
            }
        }
    }
}