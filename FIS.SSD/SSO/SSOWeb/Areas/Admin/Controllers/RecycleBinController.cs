using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using UG.BusinessEntities;
using UG.BusinessLogics;
using UG.Framework;
using SSOWeb.Areas.Admin.Controllers;
using SSOWeb.Areas.Admin.Models;

namespace SSOWeb.Areas.Admin.Controllers
{
    [UGAuthorize(Key = "Admin.RecycleBinController", Name = "Quản lý thùng rác")]
    public class RecycleBinController : BaseController
    {
        // GET: Admin/RecycleBin
        [UGAuthorize(Key = "RecycleBinController.Index", Name = "Xem danh sách")]
        public ActionResult Index(string BizID,int? page)
        {
            var lstAss = RecycleBinMapBiz.Instance.ListAllObjectsWithAssembly();
            
            if (string.IsNullOrWhiteSpace(BizID)&&lstAss.Count>0)
            {
                BizID = lstAss.First().BizID;
                return RedirectToAction("Index", new { BizID = BizID, page = page });
            }
            int pageno = 0;
            pageno = page == null ? 1 : page.Value;

            int pageSize = UGConstants.DefaultPageSize;
            long totalCount = 0;

            ViewBag.BizID = new SelectList(lstAss, RecycleBinMap.FIELD_BizID, RecycleBinMap.FIELD_BizName, BizID);
            ViewBag.TypeName = BizID;

            if (BizID == typeof(CompaniesBiz).FullName)
            {
                var lst = CompaniesBiz.Instance.ListAllObjectsIsBin(pageno, pageSize, out totalCount);
                Pager<Companies> pager = new Pager<Companies>(lst, pageno, pageSize, totalCount);
                ViewBag.ModelObj = pager;
            }
            else
                if (BizID == typeof(ComLocationStoreBiz).FullName)
                {
                    var lst = ComLocationStoreBiz.Instance.ListAllObjectsIsBin(pageno, pageSize, out totalCount);
                    Pager<ComLocationStore> pager = new Pager<ComLocationStore>(lst, pageno, pageSize, totalCount);
                    ViewBag.ModelObj = pager;
                }
            return View();
        }
        // GET: Admin/RecycleBin/Map
        [UGAuthorize(Key = "RecycleBinController.Map", Name = "Map")]
        public ActionResult Map()
        {
            var lstAss = RecycleBinMapBiz.Instance.ListAllObjectsWithAssembly();

            return View(lstAss);
        }
        [UGFollowAuthorize(FollowKey = "RecycleBinController.Map")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Map(ICollection<RecycleBinMap> maps)
        {
            RecycleBinMapBiz.Instance.SaveAll(maps);
            return RedirectToAction("Map");
        }
        [UGAuthorize(Key = "RecycleBinController.DeleteAll", Name = "Xóa thùng rác")]
        public ActionResult DeleteAll(int id, string BizID, int? page)
        {
            if (BizID == typeof(CompaniesBiz).FullName)
            {
                CompaniesBiz.Instance.DeleteRealByID(id);
            }
            else
            if (BizID == typeof(ComLocationStoreBiz).FullName)
            {
                ComLocationStoreBiz.Instance.DeleteRealByID(id);
            }
            return RedirectToAction("Index", new { BizID = BizID, page = page });
        }
        [UGAuthorize(Key = "RecycleBinController.Restore", Name = "Khôi phục")]
        public ActionResult Restore(int id, string BizID, int? page)
        {
            if (BizID == typeof(CompaniesBiz).FullName)
            {
                CompaniesBiz.Instance.RestoreByIDBin(id, WorkContext);
            }
            if (BizID == typeof(ComLocationStoreBiz).FullName)
            {
                ComLocationStoreBiz.Instance.RestoreByIDBin(id, WorkContext);
            }
            return RedirectToAction("Index", new { BizID = BizID, page = page });
        }
    }
}
