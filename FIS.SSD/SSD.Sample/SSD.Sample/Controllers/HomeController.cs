using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using SSD.Framework.Security;
using SSD.Web.Caching;
using SSD.Web.Identity;
using SSD.Web.Models;
using SSD.Web.Mvc.Controllers;
using SSD.Web.Security;

namespace SSD.Sample.Controllers
{
    
    //[RequireHttps]
    //http://www.dotnet-stuff.com/tutorials/aspnet-mvc/understanding-url-rewriting-and-url-attribute-routing-in-asp-net-mvc-mvc5-with-examples
    [RoutePrefix("trang-chu")]
    //[UGAuthorize(Key = "HomeController", Name = "Danh sách chức năng trang chủ")]
    public class HomeController : SampleBaseController
    {
        private static string GetGuidHash()
        {
            return Guid.NewGuid().ToString().GetHashCode().ToString("x");
        }
        //private void ClearAllCache()
        //{
        //    Assembly mscorlib = Assembly.GetAssembly(typeof(ICacheManager));
        //    foreach (Type type in mscorlib.GetTypes())
        //    {
        //        if (type.GetInterfaces().Length > 0)
        //            if (type.GetInterfaces().Contains(typeof(ICacheManager)))
        //            {
        //                ICacheManager biz = (ICacheManager)Activator.CreateInstance(type);
        //                biz.CleanCache();
        //            }
        //    }
        //    CacheUser.Flush();
        //    //KQKiemKhoBiz.Instance.ClearCacheTheoKyHienTay();
        //    //ChienDichCTBiz.Instance.ClearCacheRun();

        //    //Load lai Permission
        //    //UsersBiz.CurrentUser.Permission = DataAccessRepository.Provider.UsersProvider.GetPermissionByUserID(ScopeConnection.Offline.Instance.ConnectDB, UsersBiz.CurrentUser.Id, UsersBiz.CurrentUser.IsSystemAccount);
        //}
        public ActionResult ClearCache()
        {
            //UserExtensions IsGroup => IsGroup using cache
            var grpManager = HttpContext.GetOwinContext().Get<GroupManager>();
            grpManager.ClearAllCacheFrameworkByCurrentAssembly();
            WorkContext.UpdateCurrentUser();

            return Json("0", JsonRequestBehavior.AllowGet);
        }
        // [UGAuthorize(Key = "HomeController.Index", Name = "Xem trang chủ")]
        public ActionResult Index()
        {
            Dictionary<string, object> cacheItems = new Dictionary<string, object>();
            const string perfix = "com.vncorekhoainv@gmail.com.vn";
            string strVal = perfix;
            for (int i = 0; i < 5; i++)
                strVal = strVal + strVal;

            //User user = new UserInMemory();
            //user.UserName = "khoainv@fpt.com.vn";
            //for (int i = 0; i < 100; i++)
            //    user.Groups.Add(strVal + i);
            //for (int i = 0; i < 1000; i++)
            //    user.Permissions.Add(strVal + i);

            //user.Address = strVal + strVal + strVal + strVal;

            //Cache c = new Cache(3024);
            //for (int i = 0; i < 2900000; i++)
            //    c.AddItem(perfix + i, strVal + i);

            //for (int i = 0; i < 29000000; i++)
            //    MemoryCacher.Instance.Add(perfix + i, user, new DateTimeOffset(DateTime.Now.AddDays(5)));

            //for (int i = 0; i < 2900000; i++)
            //    cacheItems.Add(perfix + i, strVal + i);



            //for (int i = 0; i < 6000000; i++)
            //{
            //    CacheUser.Set<UserInMemory>(user.UserName + i, user);
            //} 

            //var startTime = DateTime.Now;
            //var str = cacheItems[perfix + 4908408];
            //var timeSpan = DateTime.Now - startTime;
            //var sec = timeSpan.Seconds;

            //var c= User.Claims;
            return View();
        }
        [Route("~/gioi-thieu")]
        [UGPermission(Key = "HomeController.About", Name = "Giới thiệu")]
        public ActionResult About()
        {
            var context = WorkContext;
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