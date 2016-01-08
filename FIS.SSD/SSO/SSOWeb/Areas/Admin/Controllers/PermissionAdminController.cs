using Microsoft.AspNet.Identity.Owin;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Data.Entity;
using System;
using SSD.SSO.Identity;
using SSOWeb.Models;
using SSOWeb.Base;

namespace SSOWeb.Areas.Admin.Controllers
{
    [Authorize]
    public class PermissionAdminController : Controller
    {
        //Ioc
        public PermissionAdminController()
        {
        }
        public PermissionAdminController(ApplicationPermissionManager permission, ApplicationRoleManager role)
        {
            PermissionManager = permission;
            RoleManager = role;
        }
        //Ioc Impl
        private ApplicationPermissionManager _perManager;
        public ApplicationPermissionManager PermissionManager
        {
            get
            {
                return _perManager ?? HttpContext.GetOwinContext().Get<ApplicationPermissionManager>();
            }
            private set
            {
                _perManager = value;
            }
        }
        private ApplicationRoleManager _roleManager;
        public ApplicationRoleManager RoleManager
        {

            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }
        public List<ApplicationPermission> Permissions
        {
            get
            {
                
                System.Reflection.Assembly mscorlib = System.Web.Compilation.BuildManager.GetGlobalAsaxType().BaseType.Assembly;
                List<ApplicationPermission> lst = new List<ApplicationPermission>();
                foreach (Type type in mscorlib.GetTypes())
                {
                    if (type.BaseType.Equals(typeof(BaseController)))
                    {
                        var lstObjAttr = type.GetCustomAttributes(typeof(UGAuthorizeAttribute), false);
                        //if (lstObjAttr.Length > 0)
                        //{
                            foreach (var md in type.GetMethods())
                            {
                                var lstAttAction = md.GetCustomAttributes(typeof(UGAuthorizeAttribute), false);
                                if (lstAttAction.Length > 0)
                                {
                                    lst.Add(
                                        new ApplicationPermission()
                                        {
                                            ControllerKey = lstObjAttr.Length > 0 ? (lstObjAttr.First() as UGAuthorizeAttribute).Key : type.Name,
                                            AcctionKey = (lstAttAction.First() as UGAuthorizeAttribute).Key,
                                            Description = (lstAttAction.First() as UGAuthorizeAttribute).Name
                                        }
                                        );
                                }
                            }
                        //}

                    }
                }
                return lst;
            }
        }
        public ActionResult Index()
        {
            
            ListPermissionViewModel model = new ListPermissionViewModel();
            model.PermissionList = PermissionManager.Permissions.OrderBy(x=>x.ControllerKey).ToList();//Permissions;
            return View(model);//this.PermissionManager.Permissions.ToList()
        }
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult UpdatePermission()
        {
            var lstDB  = PermissionManager.Permissions.ToList();
            var lst = Permissions;
            foreach (var per in lst)
            {
                var p = lstDB.Where(x=>x.AcctionKey==per.AcctionKey);
                if (p.Count() > 0)
                {
                    var first = p.First();
                    per.Id = first.Id;
                    foreach (var r in first.ApplicationRoles)
                        per.ApplicationRoles.Add(r);
                }
            }
            PermissionManager.DeleteAll();
            PermissionManager.SaveAll(lst);
            return RedirectToAction("Index");
        }
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ApplicationPermission applicationgroup =
                await this.PermissionManager.Permissions.FirstOrDefaultAsync(g => g.Id == id);
            if (applicationgroup == null)
            {
                return HttpNotFound();
            }
            var groupRoles = this.PermissionManager.GetRoles(applicationgroup.Id);

            string[] RoleNames = groupRoles.Select(p => p.Name).ToArray();
            ViewBag.RolesList = RoleNames;
            ViewBag.RolesCount = RoleNames.Count();
            return View(applicationgroup);
        }
        public ActionResult Create()
        {
            //Get a SelectList of Roles to choose from in the View:
            ViewBag.RolesList = new SelectList(
                this.RoleManager.Roles.ToList(), "Id", "Name");
            return View();
        }
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(
            [Bind(Include = "ControllerKey,AcctionKey,Description")] ApplicationPermission applicationgroup,
            params string[] selectedRoles)
        {
            if (ModelState.IsValid)
            {
                // Create the new Group:
                var result = await this.PermissionManager.CreatePermissionAsync(applicationgroup);
                if (result.Succeeded)
                {
                    selectedRoles = selectedRoles ?? new string[] { };
                    // Add the roles selected:
                    await this.PermissionManager
                        .SetPermissionRolesAsync(applicationgroup.Id, selectedRoles);
                }
                return RedirectToAction("Index");
            }
            // Otherwise, start over:
            ViewBag.RoleId = new SelectList(
                this.RoleManager.Roles.ToList(), "Id", "Name");
            return View(applicationgroup);
        }
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationPermission applicationgroup = await this.PermissionManager.FindByIdAsync(id);
            if (applicationgroup == null)
            {
                return HttpNotFound();
            }
            // Get a list, not a DbSet or queryable:
            var allRoles = await this.RoleManager.Roles.ToListAsync();
            var groupRoles = await this.PermissionManager.GetPermissionRolesAsync(id);
            var model = new PermissionViewModel()
            {
                Id = applicationgroup.Id,
                ControllerKey = applicationgroup.ControllerKey,
                AcctionKey = applicationgroup.AcctionKey,
                Description = applicationgroup.Description
            };
            // load the roles/Roles for selection in the form:
            foreach (var Role in allRoles)
            {
                var listItem = new SelectListItem()
                {
                    Text = Role.Name,
                    Value = Role.Id,
                    Selected = groupRoles.Any(g => g.ApplicationRoleId == Role.Id)
                };
                model.RolesList.Add(listItem);
            }
            return View(model);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(
            [Bind(Include = "Id,ControllerKey,AcctionKey,Description")] PermissionViewModel model,
            params string[] selectedRoles)
        {
            var group = await this.PermissionManager.FindByIdAsync(model.Id);
            if (group == null)
            {
                return HttpNotFound();
            }
            if (ModelState.IsValid)
            {
                group.ControllerKey = model.ControllerKey;
                group.AcctionKey = model.AcctionKey;
                group.Description = model.Description;
                await this.PermissionManager.UpdatePermissionAsync(group);
                selectedRoles = selectedRoles ?? new string[] { };
                await this.PermissionManager.SetPermissionRolesAsync(group.Id, selectedRoles);
                return RedirectToAction("Index");
            }
            return View(model);
        }
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationPermission applicationgroup = await this.PermissionManager.FindByIdAsync(id);
            if (applicationgroup == null)
            {
                return HttpNotFound();
            }
            return View(applicationgroup);
        }
        [HttpPost, ValidateAntiForgeryToken, ActionName("Delete")]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationPermission applicationgroup = await this.PermissionManager.FindByIdAsync(id);
            await this.PermissionManager.DeletePermissionAsync(id);
            return RedirectToAction("Index");
        }
    }
}
