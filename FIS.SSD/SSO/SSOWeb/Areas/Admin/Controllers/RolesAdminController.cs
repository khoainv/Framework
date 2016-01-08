using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Collections.Generic;
using System;
using SSD.Framework;
using SSOWeb.Models;
using SSD.SSO.Identity;
using SSOWeb.Base;

namespace SSOWeb.Areas.Admin.Controllers
{
    [Authorize]
    public class RolesAdminController : Controller
    {
        public RolesAdminController()
        {
        }
        public RolesAdminController(ApplicationUserManager userManager,
            ApplicationRoleManager roleManager,ApplicationPermissionManager perManager)
        {
            UserManager = userManager;
            RoleManager = roleManager;
            PermissionManager = perManager;
        }
        private ApplicationPermissionManager _perManager;
        public ApplicationPermissionManager PermissionManager
        {
            get
            {
                //return _perManager ?? new ApplicationPermissionManager();
                return _perManager ?? HttpContext.GetOwinContext().Get<ApplicationPermissionManager>();
            }
            private set
            {
                _perManager = value;
            }
        }
        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            set
            {
                _userManager = value;
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
        public IDictionary<string,string> ControllerPermissions
        {
            get
            {
                System.Reflection.Assembly mscorlib = System.Web.Compilation.BuildManager.GetGlobalAsaxType().BaseType.Assembly;
                IDictionary<string, string> lst = new Dictionary<string, string>();
                foreach (Type type in mscorlib.GetTypes())
                {
                    if (type.BaseType.Equals(typeof(BaseController)))
                    {
                        var lstObjAttr = type.GetCustomAttributes(typeof(UGAuthorizeAttribute), false);
                        if (lstObjAttr.Length > 0)
                        {
                             lst.Add((lstObjAttr.First() as UGAuthorizeAttribute).Key,(lstObjAttr.First() as UGAuthorizeAttribute).Name);
                        }
                    }
                }
                return lst;
            }
        }
        //
        // GET: /Roles/
        public ActionResult Index(string sortOrder, int? page, string SearchString = "")
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm =string.IsNullOrWhiteSpace(sortOrder)? "name_desc" : "";
            ViewBag.CurrentSearchKey = SearchString;

            int pageno = 0;
            pageno = page == null ? 1 : page.Value;
            int pageSize = UGConstants.DefaultPageSize;

            var query = RoleManager.Roles;
            if (!String.IsNullOrEmpty(SearchString))
            {
                query = query.Where(s => s.Name.ToUpper().Contains(SearchString.ToUpper()));
            }
            long totalCount = query.LongCount();
            switch (sortOrder)
            {
                case "name_desc":
                    query = query.OrderByDescending(s => s.Name);
                    break;
                default:
                    query = query.OrderBy(s => s.Name);
                    break;
            }

            var lst = query.Skip((pageno - 1) * pageSize).Take(pageSize).ToList();
            Pager<ApplicationRole> pager = new Pager<ApplicationRole>(lst, pageno, pageSize, totalCount);

            pager.RouteValues = new System.Web.Routing.RouteValueDictionary();
            pager.RouteValues.Add("sortOrder", sortOrder);
            pager.RouteValues.Add("SearchString", SearchString);

            return View(pager);
        }

        //
        // GET: /Roles/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var role = await RoleManager.FindByIdAsync(id);
            // Get the list of Users in this Role
            var users = new List<ApplicationUser>();

            // Get the list of Users in this Role
            foreach (var user in UserManager.Users.ToList())
            {
                if (await UserManager.IsInRoleAsync(user.Id, role.Name))
                {
                    users.Add(user);
                }
            }

            ViewBag.Users = users;
            ViewBag.UserCount = users.Count();
            return View(role);
        }

        //
        // GET: /Roles/Create
        public ActionResult Create()
        {

            return View();
        }

        //
        // POST: /Roles/Create
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(RoleViewModel roleViewModel)
        {
            if (ModelState.IsValid)
            {
                var role = new ApplicationRole(roleViewModel.Name);
                // Save the new Description property:
                role.Description = roleViewModel.Description;

                var roleresult = await RoleManager.CreateAsync(role);
                if (!roleresult.Succeeded)
                {
                    ModelState.AddModelError("", roleresult.Errors.First());
                    return View();
                }
                return RedirectToAction("Index");
            }
            return View();
        }

        //
        // GET: /Roles/Edit/Admin
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var role = await RoleManager.FindByIdAsync(id);
            if (role == null)
            {
                return HttpNotFound();
            }
            RoleViewModel roleModel = new RoleViewModel { Id = role.Id, Name = role.Name, Description = role.Description };

            var rolPers = await PermissionManager.GetPermissionsAsync(id);
            var pers = PermissionManager.Permissions.OrderBy(x=>x.ControllerKey).ToList();
            roleModel.PerrmissionList = pers.Select(x => new SelectListItem()
                {
                    Selected = rolPers.Contains(x),
                    Text = x.Description,
                    Value = x.Id
                });
            var ctrs = ControllerPermissions;
            foreach (var c in ctrs)
            {
                var perCtr = pers.Where(x=>x.ControllerKey == c.Key).Select(x=>x.Id).ToList();
                roleModel.GroupPerrmissionList.Add(c.Value, roleModel.PerrmissionList.Where(x=>perCtr.Contains(x.Value)));
            }

            var perCtr1 = pers.Where(x =>!ctrs.Keys.Contains(x.ControllerKey)).Select(x => x.Id).ToList();
            roleModel.GroupPerrmissionList.Add("Các chức năng khác", roleModel.PerrmissionList.Where(x => perCtr1.Contains(x.Value)));

            return View(roleModel);
        }

        //
        // POST: /Roles/Edit/5
        [HttpPost, ValidateAntiForgeryToken]

        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Name,Id,Description")] RoleViewModel roleModel, params string[] selectedPermission)
        {
            if (ModelState.IsValid)
            {
                var role = await RoleManager.FindByIdAsync(roleModel.Id);
                role.Name = roleModel.Name;
                // Update the new Description property:
                role.Description = roleModel.Description;
                role.ApplicationPermissions.Clear();
                if (selectedPermission!=null)
                foreach (string idPer in selectedPermission)
                    role.ApplicationPermissions.Add(new ApplicationPermissionRole() { ApplicationPermissionId = idPer, ApplicationRoleId = role.Id });

                await RoleManager.UpdateAsync(role);
                return RedirectToAction("Index");
            }
            return View();
        }

        //
        // GET: /Roles/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var role = await RoleManager.FindByIdAsync(id);
            if (role == null)
            {
                return HttpNotFound();
            }
            return View(role);
        }

        //
        // POST: /Roles/Delete/5
        [HttpPost, ValidateAntiForgeryToken, ActionName("Delete")]
        public async Task<ActionResult> DeleteConfirmed(string id, string deleteUser)
        {
            if (ModelState.IsValid)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                var role = await RoleManager.FindByIdAsync(id);
                if (role == null)
                {
                    return HttpNotFound();
                }
                IdentityResult result;
                if (deleteUser != null)
                {
                    result = await RoleManager.DeleteAsync(role);
                }
                else
                {
                    result = await RoleManager.DeleteAsync(role);
                }
                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", result.Errors.First());
                    return View();
                }
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}
