using ADIdentityServer.IdSvr;
using ADIdentityServer.Models;
using System;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace ADIdentityServer.Controllers
{
    public class HomeController : Controller
    {
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public ActionResult About()
        {
            return View((User as ClaimsPrincipal).Claims);
        }

        [Authorize]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        [Authorize]
        public ActionResult Logout()
        {
            Request.GetOwinContext().Authentication.SignOut();
            return Redirect("/");
        }
        [Authorize]
        public ActionResult ChangePassword()
        {
            var lstClaim = (User as ClaimsPrincipal).Claims;
            string userName = string.Empty;
            foreach (var c in lstClaim)
            {
                if (c.Type == "sub")
                {
                    userName = c.Value;
                    break;
                }
            }
            ChangePasswordModel user = new ChangePasswordModel()
            {
                Account = userName
            };
            return View(user);
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword([Bind(Include = "Account,OldPassword,NewPassword,ConfirmNewPassword")] ChangePasswordModel user)
        {
            if (string.IsNullOrWhiteSpace(user.Account))
            {
                ViewBag.Success = false ;
                ViewBag.Msg = "Không tồn tại UserName, Đề nghị đăng nhập trước khi đổi mật khẩu.";
                return View(user);
            }

            if (user.NewPassword != user.ConfirmNewPassword)
            {
                ModelState.AddModelError("ConfirmNewPassword", "'Nhập lại mật khẩu' phải trùng 'Mật khẩu mới'.");
            }

            if (ModelState.IsValid)
            {
                var success = false;
                try
                {
                    success = ADUserService.ChangePassword(user.Account, user.OldPassword, user.NewPassword);
                }
                catch(Exception ex)
                {
                    ViewBag.ErrorDescription = ex.Message;
                }
                ViewBag.Success = success;
                if (success)
                    ViewBag.Msg = "Đổi mật khẩu thành công!";
                else ViewBag.Msg = "Đổi mật khẩu không thành công, Đề nghị kiểm tra lại hoặc liên hệ quản trị!";
            }

            return View(user);
        }
    }
}