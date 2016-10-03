using System;
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

        private const string UploadsFolder = "Content/Images/UploadedMaps";

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(IEnumerable<HttpPostedFileBase> files)
        {
            MapViewModel viewModel;

            List<HttpPostedFileBase> filesList = files?.ToList();
            Tuple<bool, bool> validationResult = ValidateFiles(filesList);
            if (!validationResult.Item1 || !validationResult.Item2)
            {
                viewModel = new MapViewModel
                {
                    Message = GetMessage(validationResult)
                };
            }
            else
            {
                HttpPostedFileBase file1 = filesList.First();
                Map map = _mapFileReader.GetMap(GetBytes(file1), file1.FileName);
                viewModel = map == null
                    ? new MapViewModel { Message = "Invalid Map Text File" }
                    : GetMapViewModel(map, filesList.Last());
            }

            return View(viewModel);
        }

        private MapViewModel GetMapViewModel(Map map, HttpPostedFileBase file2)
        {
            _imageReader.PopulateStabilities(map, Image.FromStream(file2.InputStream));
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

        private static string GetMessage(Tuple<bool, bool> validationResult)
        {
            string message = "";
            if (!validationResult.Item1)
            {
                message += " Invalid Map Text File";
            }
            if (!validationResult.Item2)
            {
                message += " Invalid Map Stability Image";
            }
            return message.Trim();
        }

        /// <summary>
        /// Validates the files required for the HttpPost method Index(IEnumerable&lt;HttpPostedFileBase&gt; files)
        /// </summary>
        /// <param name="files"></param>
        /// <returns>A Tuple&lt;bool, bool&gt; where Item1 is the validation result of the first file, and Item2 is the validation result of the second file</returns>
        private Tuple<bool, bool> ValidateFiles(List<HttpPostedFileBase> files)
        {
            bool file1IsValid;
            bool file2IsValid;
            if (files == null || files.Count != 2)
            {
                file1IsValid = false;
                file2IsValid = false;
            }
            else
            {
                file1IsValid = files.FirstOrDefault() != null;
                file2IsValid = files.LastOrDefault() != null;
            }
            return new Tuple<bool, bool>(file1IsValid, file2IsValid);
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