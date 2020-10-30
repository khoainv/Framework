using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using SSOWeb.Areas.Admin.Controllers;
using SSOWeb.Base;
using SSD.SSO;
using System.Security.Claims;

namespace SSOWeb.Controllers
{
    
    //[RequireHttps]
    //http://www.dotnet-stuff.com/tutorials/aspnet-mvc/understanding-url-rewriting-and-url-attribute-routing-in-asp-net-mvc-mvc5-with-examples
    [RoutePrefix("trang-chu")]
    //[UGAuthorize(Key = "HomeController", Name = "Danh sách chức năng trang chủ")]
    public class HomeController : BaseController
    {
        public ActionResult ClearCache()
        {
            SSD.SSO.Config.AppConfigBiz.Instance.CleanCacheWithDecrypted();
            return Json("0", JsonRequestBehavior.AllowGet);
        }
       // [UGAuthorize(Key = "HomeController.Index", Name = "Xem trang chủ")]
        public ActionResult Index()
        {
            return View();
        }
        [Authorize]
        public ActionResult Claim()
        {
            return View((HttpContext.User as ClaimsPrincipal).Claims);
        }
        [Route("~/gioi-thieu")]
        //[UGAuthorize(Key = "HomeController.About", Name = "Giới thiệu")]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        [Route("lien-he")]
        //[UGAuthorize(Key = "HomeController.Contact", Name = "Liên hệ")]
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