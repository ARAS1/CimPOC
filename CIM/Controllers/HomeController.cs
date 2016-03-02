using System.Web.Mvc;

namespace CIM.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult RandomView()
        {
            ViewBag.Message = "Random View Page 123456";

            return View();
        }

        public ActionResult PrivacyPolicy()
        {
            ViewBag.Message = "Privacy Policy";

            return View();
        }
    }
}