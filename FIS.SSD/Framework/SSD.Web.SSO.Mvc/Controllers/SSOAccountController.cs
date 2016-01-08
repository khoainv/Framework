using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using SSD.Web.Mvc.Models;
using SSD.Web.Identity;

namespace SSD.Web.SSO.Mvc.Controllers
{
    [Authorize]
    public class SSOAccountController : Controller
    {
        [Authorize]
        public ActionResult Login()
        {
            //return View();
            return Redirect("/");
        }
        [Authorize]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut();
            return Redirect("/");
        }
        // GET: /Account/Register
        // GET: /Account/ForgotPassword
        // GET: /Account/ResetPassword
        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }
    }
}