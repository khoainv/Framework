using SSOWeb.Models;
using System;
using System.Data;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using SSD.Framework;
using SSD.SSO.Config;
using SSD.SSO.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Web;
using SSOWeb.Base;
using SSD.SSO;
using SSD.Framework.Email;

namespace SSOWeb.Areas.Admin.Controllers
{
    [UGAuthorize(Key = "Admin.AppConfigsController", Name = "Quản lý cấu hình")]
    public class AppConfigsController : BaseController
    {
        [UGFollowAuthorize(FollowKey = "AppConfigsController.Index")]
        public ActionResult ClearLogBD()
        {
            return Json("0", JsonRequestBehavior.AllowGet);
        }
        [UGFollowAuthorize(FollowKey = "AppConfigsController.Index")]
        public ActionResult RestartApplication()
        {
            var helper = new WebHelper();
            helper.RestartAppDomain(HttpContext);
            return Json("0", JsonRequestBehavior.AllowGet);
        }

        private AppConfigBiz _appConfig;
        //Ioc
        public AppConfigsController()
        {
        }
        public AppConfigsController(AppConfigBiz config)
        {
            AppConfigManager = config;//new AppConfigBiz( dbContext);
        }
        //Ioc Impl
        public AppConfigBiz AppConfigManager
        {
            get
            {
                return _appConfig ?? new AppConfigBiz(HttpContext.GetOwinContext().Get<ApplicationDbContext>());
            }
            private set
            {
                _appConfig = value;
            }
        }
        [UGFollowAuthorize(FollowKey = "AppConfigsController.ConfigSMSEdit")]
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult ConfigSMSEdit([Bind(Include = "ID,ConfigKey,IsEncryption")] AppConfig appConfig
            , [Bind(Include = "SID,Token,FromPhone")] SMSServerData sms)
        {
            ModelState.Clear();
            if (string.IsNullOrWhiteSpace(appConfig.ConfigKey))
                ModelState.AddModelError("ConfigKey", "ConfigKey: Yêu cầu nhập");
            if (string.IsNullOrWhiteSpace(sms.SID))
                ModelState.AddModelError("SID", "SID: Yêu cầu nhập");
            if (string.IsNullOrWhiteSpace(sms.Token))
                ModelState.AddModelError("Token", "Token: Yêu cầu nhập");
            if (string.IsNullOrWhiteSpace(sms.FromPhone))
                ModelState.AddModelError("FromPhone", "FromPhone: Yêu cầu nhập");

            if (appConfig.ID == 0 && AppConfigManager.IsExistKey(appConfig.ConfigKey))
            {
                ModelState.AddModelError("CustomConfigKey", appConfig.ConfigKey + ": Đã có trong cơ sở dữ liệu rồi, đề nghị sử dụng Key khác");
            }
            appConfig.DataType = typeof(SMSServerData).FullName;
            if (sms != null)
                appConfig.ConfigData = sms.ToXML();
            if (ModelState.IsValid)
            {
                AppConfigManager.InsertOrUpdateWithEncrypt(appConfig);
                return RedirectToAction("ConfigExt", new { typeName = typeof(SMSServerData).FullName });
            }
            return View(appConfig);
        }
        [UGAuthorize(Key = "AppConfigsController.ConfigSMSEdit", Name = "Cấu hình SMS")]
        public ActionResult ConfigSMSEdit(int? id)
        {
            if (id == null)
            {
                return View();
            }
            AppConfig config = AppConfigManager.ReadObjectByIDWithDecrypted(id.Value);
            if (config == null)
            {
                return HttpNotFound();
            }
            return View(config);
        }

        #region SMTP
        [UGFollowAuthorize(FollowKey = "AppConfigsController.ConfigSMTPEdit")]
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult ConfigSMTPEdit([Bind(Include = "ID,ConfigKey,IsEncryption")] AppConfig appConfig
            , [Bind(Include = "Host,Port,UserName,Password")] SmtpData smtp)
        {
            ModelState.Clear();
            if (string.IsNullOrWhiteSpace(appConfig.ConfigKey))
                ModelState.AddModelError("ConfigKey", "ConfigKey: Yêu cầu nhập");
            if (string.IsNullOrWhiteSpace(smtp.Host))
                ModelState.AddModelError("Host", "Host: Yêu cầu nhập");
            if (smtp.Port == 0)
                ModelState.AddModelError("Port", "Port: Yêu cầu nhập");
            if (string.IsNullOrWhiteSpace(smtp.UserName))
                ModelState.AddModelError("UserName", "UserName: Yêu cầu nhập");

            if (appConfig.ID == 0 && AppConfigManager.IsExistKey(appConfig.ConfigKey))
            {
                ModelState.AddModelError("CustomConfigKey", appConfig.ConfigKey + ": Đã có trong cơ sở dữ liệu rồi, đề nghị sử dụng Key khác");
            }
            appConfig.DataType = typeof(SmtpData).FullName;
            if (smtp != null)
                appConfig.ConfigData = smtp.ToXML();
            if (ModelState.IsValid)
            {
                AppConfigManager.InsertOrUpdateWithEncrypt(appConfig);
                return RedirectToAction("ConfigExt", new { typeName = typeof(SmtpData).FullName });
            }
            return View(appConfig);
        }
        [UGAuthorize(Key = "AppConfigsController.ConfigSMTPEdit", Name = "Cấu hình SMTP")]
        public ActionResult ConfigSMTPEdit(int? id)
        {
            if (id == null)
            {
                return View();
            }
            AppConfig config = AppConfigManager.ReadObjectByIDWithDecrypted(id.Value);
            if (config == null)
            {
                return HttpNotFound();
            }
            return View(config);
        }
        #endregion
        #region Authen OpenId
        [UGFollowAuthorize(FollowKey = "AppConfigsController.ConfigAuthenOpenIdEdit")]
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult ConfigAuthenOpenIdEdit([Bind(Include = "ID,ConfigKey,IsEncryption")] AppConfig appConfig
            , [Bind(Include = "Caption,Authority,RedirectUri,PostLogoutRedirectUri,ClientId,ClientSecret")] ExtAuthenticationOpenIdConnect authen)
        {
            ModelState.Clear();
            if (string.IsNullOrWhiteSpace(appConfig.ConfigKey))
                ModelState.AddModelError("ConfigKey", "ConfigKey: Yêu cầu nhập");
            if (string.IsNullOrWhiteSpace(authen.Authority))
                ModelState.AddModelError("Authority", "Authority: Yêu cầu nhập");
            if (string.IsNullOrWhiteSpace(authen.RedirectUri))
                ModelState.AddModelError("RedirectUri", "RedirectUri: Yêu cầu nhập");
            if (string.IsNullOrWhiteSpace(authen.PostLogoutRedirectUri))
                ModelState.AddModelError("PostLogoutRedirectUri", "PostLogoutRedirectUri: Yêu cầu nhập");
            if (string.IsNullOrWhiteSpace(authen.ClientId))
                ModelState.AddModelError("ClientId", "ClientId: Yêu cầu nhập");
            if (string.IsNullOrWhiteSpace(authen.ClientSecret))
                ModelState.AddModelError("ClientSecret", "ClientSecret: Yêu cầu nhập");

            if (appConfig.ID == 0 && AppConfigManager.IsExistKey(appConfig.ConfigKey))
            {
                ModelState.AddModelError("CustomConfigKey", appConfig.ConfigKey + ": Đã có trong cơ sở dữ liệu rồi, đề nghị sử dụng Key khác");
            }
            appConfig.DataType = typeof(ExtAuthenticationOpenIdConnect).FullName;
            if (authen != null)
                appConfig.ConfigData = authen.ToXML();
            if (ModelState.IsValid)
            {
                AppConfigManager.InsertOrUpdateWithEncrypt(appConfig);
                return RedirectToAction("ConfigExt", new { typeName = typeof(ExtAuthenticationOpenIdConnect).FullName });
            }
            return View(appConfig);
        }

        [UGAuthorize(Key = "AppConfigsController.ConfigAuthenOpenIdEdit", Name = "Cấu hình Authentication OpenId")]
        public ActionResult ConfigAuthenOpenIdEdit(int? id)
        {
            if (id == null)
            {
                return View();
            }
            AppConfig config = AppConfigManager.ReadObjectByIDWithDecrypted(id.Value);
            if (config == null)
            {
                return HttpNotFound();
            }
            return View(config);
        }
        #endregion

        #region Authen
        [UGFollowAuthorize(FollowKey = "AppConfigsController.ConfigAuthenEdit")]
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult ConfigAuthenEdit([Bind(Include = "ID,ConfigKey,IsEncryption")] AppConfig appConfig
            , [Bind(Include = "ClientId,ClientSecret")] ExtAuthentication authen)
        {
            ModelState.Clear();
            if (string.IsNullOrWhiteSpace(appConfig.ConfigKey))
                ModelState.AddModelError("ConfigKey", "ConfigKey: Yêu cầu nhập");
            if (string.IsNullOrWhiteSpace(authen.ClientId))
                ModelState.AddModelError("ClientId", "ClientId: Yêu cầu nhập");
            if (string.IsNullOrWhiteSpace(authen.ClientSecret))
                ModelState.AddModelError("ClientSecret", "ClientSecret: Yêu cầu nhập");

            if (appConfig.ID == 0 && AppConfigManager.IsExistKey(appConfig.ConfigKey))
            {
                ModelState.AddModelError("CustomConfigKey", appConfig.ConfigKey + ": Đã có trong cơ sở dữ liệu rồi, đề nghị sử dụng Key khác");
            }
            appConfig.DataType = typeof(ExtAuthentication).FullName;
            if (authen!=null)
                appConfig.ConfigData = authen.ToXML();
            if (ModelState.IsValid)
            {
                AppConfigManager.InsertOrUpdateWithEncrypt(appConfig);
                return RedirectToAction("ConfigExt", new { typeName = typeof(ExtAuthentication).FullName });
            }
            return View(appConfig);
        }

        [UGAuthorize(Key = "AppConfigsController.ConfigAuthenEdit", Name = "Cấu hình Authentication")]
        public ActionResult ConfigAuthenEdit(int? id)
        {
            if (id == null)
            {
                return View();
            }
            AppConfig config = AppConfigManager.ReadObjectByIDWithDecrypted(id.Value);
            if (config == null)
            {
                return HttpNotFound();
            }
            return View(config);
        }
        #endregion

        [UGAuthorize(Key = "AppConfigsController.ConfigExt", Name = "Danh sách cấu hình mở rộng")]
        public ActionResult ConfigExt(string typeName)
        {
            ViewBag.TypeName = typeName;
            if (typeName == typeof(ExtAuthentication).FullName)
            {
                ViewBag.EditAction = "ConfigAuthenEdit";
                return View(AppConfigManager.GetAllExtAuthentication);
            }else 
            if (typeName == typeof(ExtAuthenticationOpenIdConnect).FullName)
            {
                ViewBag.EditAction = "ConfigAuthenOpenIdEdit";
                return View(AppConfigManager.GetAllExtAuthenticationOpenIdConnect);
            }
            else if (typeName == typeof(SmtpData).FullName)
            {
                ViewBag.EditAction = "ConfigSMTPEdit";
                return View(AppConfigManager.GetAllSmtpData);
            }
            else if (typeName == typeof(SMSServerData).FullName)
            {
                ViewBag.EditAction = "ConfigSMSEdit";
                return View(AppConfigManager.GetAllSMSServerData);
            }
            return RedirectToAction("Index");
        }
        // GET: Admin/AppConfigs
        [UGAuthorize(Key = "AppConfigsController.Index", Name = "Danh sách cấu hình")]
        public ActionResult Index(string sortOrder, int? page, string SearchString = "")
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = string.IsNullOrWhiteSpace(sortOrder) ? "key_desc" : "";
            ViewBag.CurrentSearchKey = SearchString;

            int pageno = 0;
            pageno = page == null ? 1 : page.Value;
            int pageSize = UGConstants.DefaultPageSize;
            var query = AppConfigManager.GetAllAppSetting;
            if (!String.IsNullOrEmpty(SearchString))
            {

                query = query.Where(s => s.ConfigKey.ToUpper().Contains(SearchString.ToUpper()) || s.ConfigData.ToUpper().Contains(SearchString.ToUpper()));
            }
            long totalCount = query.LongCount();
            switch (sortOrder)
            {
                case "key_desc":
                    query = query.OrderByDescending(s => s.ConfigKey);
                    break;
                default:
                    query = query.OrderBy(s => s.ConfigKey);
                    break;
            }

            //var lst = query.Skip((pageno - 1) * pageSize).Take(pageSize).ToList();
            Pager<AppConfig> pager = new Pager<AppConfig>(query.AsQueryable<AppConfig>(), pageno, pageSize, totalCount);
            pager.RouteValues = new System.Web.Routing.RouteValueDictionary();
            pager.RouteValues.Add("sortOrder", sortOrder);
            pager.RouteValues.Add("SearchString", SearchString);

            return View(pager);
        }
        [UGAuthorize(Key = "AppConfigsController.Details", Name = "Xem chi tiết cấu hình")]
        // GET: Admin/AppConfigs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AppConfig appConfig = AppConfigManager.ReadObjectByIDWithDecrypted(id.Value);
            if (appConfig == null)
            {
                return HttpNotFound();
            }
            return View(appConfig);
        }
        [UGAuthorize(Key = "AppConfigsController.Create", Name = "Tạo mới cấu hình")]
        // GET: Admin/AppConfigs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/AppConfigs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ValidateAntiForgeryToken]
        [UGFollowAuthorize(FollowKey = "AppConfigsController.Create")]
        public ActionResult Create([Bind(Include = "ID,ConfigKey,ConfigData,DataType,IsEncryption")] AppConfig appConfig)
        {
            if(AppConfigManager.IsExistKey(appConfig.ConfigKey))
            {
                ModelState.AddModelError("CustomConfigKey", appConfig.ConfigKey + ": Đã có trong cơ sở dữ liệu rồi, đề nghị sử dụng Key khác");
            }
            if (ModelState.IsValid)
            {
                AppConfigManager.InsertWithEncrypt(appConfig);
                return RedirectToAction("Index");
            }

            return View(appConfig);
        }
        [UGAuthorize(Key = "AppConfigsController.Edit", Name = "Sửa cấu hình")]
        // GET: Admin/AppConfigs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AppConfig appConfig = AppConfigManager.ReadObjectByIDWithDecrypted(id.Value);
            if (appConfig == null)
            {
                return HttpNotFound();
            }
            return View(appConfig);
        }

        // POST: Admin/AppConfigs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [UGFollowAuthorize(FollowKey = "AppConfigsController.Edit")]
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,ConfigKey,ConfigData,DataType,IsEncryption")] AppConfig appConfig)
        {
            if (ModelState.IsValid)
            {
                AppConfigManager.UpdateWithEncrypt(appConfig);
                return RedirectToAction("Index");
            }
            return View(appConfig);
        }

        [UGAuthorize(Key = "AppConfigsController.Delete", Name = "Xóa cấu hình")]
        // GET: Admin/AppConfigs/Delete/5
        public ActionResult Delete(int? id, string actionName, string typeName)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AppConfig appConfig = AppConfigManager.ReadObjectByIDWithDecrypted(id.Value);
            if (appConfig == null)
            {
                return HttpNotFound();
            }
            ViewBag.ActionName = "Index";
            if (!string.IsNullOrWhiteSpace(actionName))
            {
                appConfig.ConfigData = string.Empty;
                ViewBag.TypeName = typeName;
                ViewBag.ActionName = actionName;
            }

            return View(appConfig);
        }
        [UGFollowAuthorize(FollowKey = "AppConfigsController.Delete")]
        // POST: Admin/AppConfigs/Delete/5
        [HttpPost, ValidateAntiForgeryToken, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id,string actionName,string typeName)
        {
            AppConfigManager.DeleteByID(id);
            AppConfigManager.CleanCacheWithDecrypted();
            if (!string.IsNullOrWhiteSpace(actionName))
                return RedirectToAction(actionName, new { typeName = typeName });
            return RedirectToAction("Index");
        }
    }
}
