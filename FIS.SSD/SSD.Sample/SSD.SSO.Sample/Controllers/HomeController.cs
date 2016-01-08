using System.Collections.Generic;
using System.Web.Mvc;
using System.Web;
using SSD.Web.Mvc.Controllers;
using SSD.Web.SSO;
using SSD.Web.SSO.Mvc.Controllers;
using Microsoft.AspNet.Identity.Owin;
using System.Security.Claims;
using SSD.Web.Security;
using SSD.Framework.Security;

namespace SSD.SSO.Sample.Controllers
{
    
    //[RequireHttps]
    //http://www.dotnet-stuff.com/tutorials/aspnet-mvc/understanding-url-rewriting-and-url-attribute-routing-in-asp-net-mvc-mvc5-with-examples
    [RoutePrefix("trang-chu")]
    //[UGAuthorize(Key = "HomeController", Name = "Danh sách chức năng trang chủ")]
    public class HomeController : SampleBaseController
    {
        public ActionResult ClearCache()
        {
            //UserExtensions IsGroup => IsGroup using cache
            var grpManager = HttpContext.GetOwinContext().Get<SSOGroupManager>();
            grpManager.ClearAllCacheFrameworkByCurrentAssembly();
            WorkContext.UpdateCurrentUser();

            return Json("0", JsonRequestBehavior.AllowGet);
        }
        // [UGAuthorize(Key = "HomeController.Index", Name = "Xem trang chủ")]
        public ActionResult Index()
        {
            //var str = WorkContext==null?new List<string>():WorkContext.CurrentUser.ListRoles;
            //var c= User.Claims;
            return View();
        }
        [Authorize]
        public ActionResult Claim()
        {
            return View((HttpContext.User as ClaimsPrincipal).Claims);
        }
        [Route("~/gioi-thieu")]
        [UGPermission(Key = "HomeController.About", Name = "Giới thiệu")]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        [Route("lien-he")]
        [UGPermission(Key = "HomeController.Contact", Name = "Liên hệ")]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        [Route("tu-cho-truy-cap/{message}")]
        public ActionResult AccessDenied(string message)
        {
            ViewBag.Message = message;
            return View(); 
        }
    }
}