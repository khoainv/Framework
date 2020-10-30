using SSD.Web.Mvc.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using SSD.Web.Identity;
using SSD.Framework;
using SSD.Web.Models;

namespace SSD.Web.Mvc.Controllers
{
    [AdminAuthorize(Groups = "Admin")]
    public class UsersController : Controller
    {
        public UsersController()
        {
        }

        public UsersController(UserManager userManager, GroupManager grpManager, PermissionManager perManager)
        {
            UserManager = userManager;
            GroupManager = grpManager;
            PermissionManager = perManager;
        }

        private UserManager _userManager;
        public UserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<UserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        private GroupManager _grpManager;
        public GroupManager GroupManager
        {
            get
            {
                return _grpManager ?? HttpContext.GetOwinContext().Get<GroupManager>();
            }
            private set
            {
                _grpManager = value;
            }
        }
        private PermissionManager _perManager;
        public PermissionManager PermissionManager
        {
            get
            {
                return _perManager ?? HttpContext.GetOwinContext().Get<PermissionManager>();
            }
            private set
            {
                _perManager = value;
            }
        }
        //
        // GET: /Users/
        public async Task<ActionResult> Index(string sortOrder, int? page, string SearchString = "", bool isSystem = false)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = sortOrder == "name" ? "name_desc" : "name";
            ViewBag.CurrentSearchKey = SearchString;
            ViewBag.CurrentFilter = isSystem;

            int pageno = 0;
            pageno = page == null ? 1 : page.Value;
            int pageSize = UGConstants.DefaultPageSize;

            var query = UserManager.Users;
            if (!String.IsNullOrEmpty(SearchString))
            {
                query = query.Where(s => s.UserName.ToUpper().Contains(SearchString.ToUpper()));
            }
            query = query.Where(x=>x.IsSystemAccount == isSystem);
            long totalCount = query.LongCount();
            switch (sortOrder)
            {
                case "name_desc":
                    query = query.OrderByDescending(s => s.UserName);
                    break;
                case "name":
                    query = query.OrderBy(s => s.UserName);
                    break;
                default:  
                    query = query.OrderBy(x=>x.Id);
                    break;
            }

            var lst = await query.Skip((pageno - 1) * pageSize).Take(pageSize).ToListAsync();
            Pager<User> pager = new Pager<User>(lst, pageno, pageSize, totalCount);

            pager.RouteValues = new System.Web.Routing.RouteValueDictionary();
            pager.RouteValues.Add("sortOrder", sortOrder);
            pager.RouteValues.Add("SearchString", SearchString);
            pager.RouteValues.Add("isSystem", isSystem);

            return View(pager);
        }

        //
        // GET: /Users/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = await UserManager.FindByIdAsync(id);

            ViewBag.GroupNames = await GroupManager.FindByUserName(user.UserName).Select(x=>x.Name).ToListAsync();

            return View(user);
        }
        //
        // GET: /Users/Details/5
        public async Task<ActionResult> ViewUser(string username)
        {
            if (username == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = await UserManager.FindByNameAsync(username);

            ViewBag.GroupNames = await GroupManager.FindByUserName(user.UserName).Select(x => x.Name).ToListAsync();

            return View(user);
        }
        //
        // GET: /Users/Create
        public async Task<ActionResult> Create()
        {
            //Get the list of Roles
            ViewBag.Groups = new SelectList(await GroupManager.Groups.ToListAsync(), "Id", "Name");
            return View();
        }

        //
        // POST: /Users/Create
        [HttpPost]
        public async Task<ActionResult> Create(RegisterViewModel userViewModel, params string[] selectedGroups)
        {
            ViewBag.Groups = new SelectList(await GroupManager.Groups.ToListAsync(), "Id", "Name");

            if (ModelState.IsValid)
            {
                var user = new User { 
                    UserName = userViewModel.Email, 
                    Email = userViewModel.Email, 
                    IsSystemAccount = userViewModel.IsSystemAccount
                };
                var adminresult = await UserManager.CreateAsync(user, userViewModel.Password);

                //Add User to the selected Roles 
                if (adminresult.Succeeded)
                {
                    if (selectedGroups != null)
                    {
                        var result = await GroupManager.AddUserToGroupsAsync(user.UserName, selectedGroups);
                        if (!result.Succeeded)
                        {
                            ModelState.AddModelError("", result.Errors.First());
                            return View(userViewModel);
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("", adminresult.Errors.First());
                    return View(userViewModel);

                }
                return RedirectToAction("Index");
            }
            return View(userViewModel);
        }

        //
        // GET: /Users/Edit/1
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = await UserManager.FindByIdAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            var ugrp = await GroupManager.FindByUserName(user.UserName).Select(x=>x.Id).ToListAsync();

            return View(new EditUserViewModel()
            {
                Id = user.Id,
                Email = user.Email,
                // Include the Addresss info:
                Address = user.Address,
                City = user.City,
                Contry = user.Contry,
                IsSystemAccount = user.IsSystemAccount,

                GroupsList = GroupManager.Groups.Select(x => new SelectListItem()
                {
                    Selected = ugrp.Contains(x.Id),
                    Text = x.Name,
                    Value = x.Id
                }),
                PermissionsList = PermissionManager.FindPermissionsByUserName(user.UserName)
            });
        }

        //
        // POST: /Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Email,Id,Address,City,Contry,IsSystemAccount")] EditUserViewModel editUser
            , params string[] SelectedGroup)
        {
            var user = await UserManager.FindByIdAsync(editUser.Id);
            if (user == null)
            {
                return HttpNotFound();
            }
            if (ModelState.IsValid)
            {
                //user.UserName = editUser.Email; khong edit username
                user.Email = editUser.Email;
                user.Address = editUser.Address;
                user.City = editUser.City;
                user.Contry = editUser.Contry;
                user.IsSystemAccount = editUser.IsSystemAccount;

                await UserManager.UpdateAsync(user);

                SelectedGroup = SelectedGroup ?? new string[] { };

                var lstSelectedGroup = SelectedGroup.ToList();

                if (user.UserName == UGConstants.AccountAdmin)
                {
                    var grp = GroupManager.FindByName(UGConstants.GroupAdmin);
                    lstSelectedGroup.Add(grp.Id);
                }

                var userRoles = GroupManager.FindByUserName(user.UserName).Select(x=>x.Id);

                var result = await GroupManager.AddUserToGroupsAsync(user.UserName, lstSelectedGroup.Except(userRoles).ToArray<string>());

                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", result.Errors.First());
                    return View();
                }
                result = await GroupManager.RemoveUserFromGroupsAsync(user.UserName, userRoles.Except(lstSelectedGroup).ToArray<string>());

                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", result.Errors.First());
                    return View();
                }
                return RedirectToAction("Edit", new { id = editUser.Id });
            }
            ModelState.AddModelError("", "Something failed.");
            return View();
        }

        //
        // GET: /Users/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = await UserManager.FindByIdAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        //
        // POST: /Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            if (ModelState.IsValid)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                var user = await UserManager.FindByIdAsync(id);
                if (user == null)
                {
                    return HttpNotFound();
                }
                if(user.UserName == UGConstants.AccountAdmin)
                {
                    ViewBag.MsgError = "User Admin là không được xóa!";
                    return View(user);
                }
                var result = await UserManager.DeleteAsync(user);
                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", result.Errors.First());
                    return View(user);
                }
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}
