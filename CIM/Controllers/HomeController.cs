using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using CIM.PolicyAuthHelpers;

namespace CIM.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
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

        [Authorize]
        public ActionResult PrivacyPolicy()
        {
            ViewBag.Message = "Privacy Policy";

            return View();
        }

        //[PolicyAuthorize(Policy = "B2C_1_profileEditingTestPolicy")]
        public ActionResult Claims()
        {           
            Claim displayName = ClaimsPrincipal.Current.FindFirst(ClaimsPrincipal.Current.Identities.First().NameClaimType);
            var claims = HttpContext.GetOwinContext().Authentication.User.Claims;
            bool isAuthenticated = HttpContext.GetOwinContext().Authentication.User.Identity.IsAuthenticated;
            var authType = HttpContext.GetOwinContext().Authentication.User.Identity.AuthenticationType;
            var authName = HttpContext.GetOwinContext().Authentication.User.Identity.Name;
            ViewBag.Issuer = displayName.Issuer;
            ViewBag.Keys = displayName.Properties.Keys;
            ViewBag.Values = displayName.Properties.Values;
            ViewBag.DisplayName = displayName != null ? displayName.Value : string.Empty;
            return View();
        }
    }
}