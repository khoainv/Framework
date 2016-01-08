using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IdentityServer3.EntityFramework.Entities;
using SSD.SSO.Identity;
using Microsoft.AspNet.Identity.Owin;
using SSOWeb.Base;

namespace SSOWeb.Areas.Admin.Controllers
{
    [UGAuthorize(Key = "Admin.ScopesController", Name = "Quản lý Scopes")]
    public class ScopesController : BaseController
    {
        private ApplicationDbContext _db;
        //Ioc
        public ScopesController()
        {
        }
        public ScopesController(ApplicationDbContext dbContext)
        {
            DbContext = dbContext;
        }
        //Ioc Impl
        public ApplicationDbContext DbContext
        {
            get
            {
                return _db ?? HttpContext.GetOwinContext().Get<ApplicationDbContext>();
            }
            private set
            {
                _db = value;
            }
        }
        // GET: Admin/Scopes
        [UGAuthorize(Key = "ScopesController.Index", Name = "Danh sách Scopes")]
        public ActionResult Index()
        {
            return View(DbContext.Scopes.ToList());
        }

        // GET: Admin/Scopes/Details/5
        [UGAuthorize(Key = "ScopesController.Details", Name = "Chi tiết Scopes")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Scope scope = DbContext.Scopes.Find(id);
            if (scope == null)
            {
                return HttpNotFound();
            }
            return View(scope);
        }

        // GET: Admin/Scopes/Create
        [UGAuthorize(Key = "ScopesController.Create", Name = "Tạo mới Scope")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Scopes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ValidateAntiForgeryToken]
        [UGFollowAuthorize(FollowKey = "ScopesController.Create")]
        public ActionResult Create([Bind(Include = "Id,Enabled,Name,DisplayName,Description,Required,Emphasize,Type,IncludeAllClaimsForUser,ClaimsRule,ShowInDiscoveryDocument")] Scope scope)
        {
            if (ModelState.IsValid)
            {
                DbContext.Scopes.Add(scope);
                DbContext.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(scope);
        }

        // GET: Admin/Scopes/Edit/5
        [UGAuthorize(Key = "ScopesController.Edit", Name = "Xửa Scope")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Scope scope = DbContext.Scopes.Find(id);
            if (scope == null)
            {
                return HttpNotFound();
            }
            return View(scope);
        }

        // POST: Admin/Scopes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ValidateAntiForgeryToken]
        [UGFollowAuthorize(FollowKey = "ScopesController.Edit")]
        public ActionResult Edit([Bind(Include = "Id,Enabled,Name,DisplayName,Description,Required,Emphasize,Type,IncludeAllClaimsForUser,ClaimsRule,ShowInDiscoveryDocument")] Scope scope)
        {
            if (scope.Name == IdentityServer3.Core.Constants.StandardScopes.OpenId
                || scope.Name == IdentityServer3.Core.Constants.StandardScopes.Email
                || scope.Name == IdentityServer3.Core.Constants.StandardScopes.Profile)
            {
                ModelState.AddModelError("Name", "Không được sửa Scopes này!");
            }
            if (ModelState.IsValid)
            {
                DbContext.Entry(scope).State = EntityState.Modified;
                DbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(scope);
        }

        // GET: Admin/Scopes/Delete/5
        [UGAuthorize(Key = "ScopesController.Delete", Name = "Xóa Scope")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Scope scope = DbContext.Scopes.Find(id);
            if (scope == null)
            {
                return HttpNotFound();
            }
            return View(scope);
        }

        // POST: Admin/Scopes/Delete/5
        [HttpPost, ValidateAntiForgeryToken, ActionName("Delete")]
        [UGFollowAuthorize(FollowKey = "ScopesController.Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Scope scope = DbContext.Scopes.Find(id);

            if (scope.Name == IdentityServer3.Core.Constants.StandardScopes.OpenId
                || scope.Name == IdentityServer3.Core.Constants.StandardScopes.Email
                || scope.Name == IdentityServer3.Core.Constants.StandardScopes.Profile)
            {
                ModelState.AddModelError("Name", "Không được xóa Scopes này!");
            }
            if (ModelState.IsValid)
            {
                DbContext.Scopes.Remove(scope);
                DbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(scope);
        }
    }
}
