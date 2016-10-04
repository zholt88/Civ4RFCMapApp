using System.Web.Mvc;

namespace Civ4RFCMapApp.WebUI.Controllers
{
    public class RfcStabilityController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
    }
}