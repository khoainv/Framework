using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SSD.Web.Models;
using SSD.Web.SSO;
using Microsoft.AspNet.Identity.Owin;
using SSD.Framework;
using System.Threading.Tasks;
using SSD.Web.Mvc.Models;

namespace SSD.Web.SSO.Mvc.Controllers
{
    [SSOAutshorize(Groups = "Admin")]
    public class SSOUserController : Controller
    {
        public SSOUserController()
        {
        }
        public SSOUserController(SSOGroupManager grpManager, SSOPermissionManager perManager)
        {
            _grpManager = grpManager;
            _perManager = perManager;
        }
        private SSOGroupManager _grpManager;
        public SSOGroupManager GroupManager
        {
            get
            {
                if (_grpManager == null)
                    _grpManager = HttpContext.GetOwinContext().Get<SSOGroupManager>();
                return _grpManager;
            }
        }
        private SSOPermissionManager _perManager;
        public SSOPermissionManager PermissionManager
        {
            get
            {
                if (_perManager == null)
                    _perManager = HttpContext.GetOwinContext().Get<SSOPermissionManager>();
                return _perManager;
            }
        }
        // GET: Admin/UserGroups
        public async Task<ActionResult> Index(string sortOrder, int? page, string SearchString = "")
        {
            var lst = GroupManager.UserGroups.Select(x => x.UserName).Distinct();
            //return View(lst.ToList());

            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = sortOrder == "name" ? "name_desc" : "name";
            ViewBag.CurrentSearchKey = SearchString;

            int pageno = 0;
            pageno = page == null ? 1 : page.Value;
            int pageSize = UGConstants.DefaultPageSize;

            if (!String.IsNullOrEmpty(SearchString))
            {
                lst = lst.Where(s => s.ToUpper().Contains(SearchString.ToUpper()));
            }
            long totalCount = lst.LongCount();
            switch (sortOrder)
            {
                case "name_desc":
                    lst = lst.OrderByDescending(s => s);
                    break;
                default:
                    lst = lst.OrderBy(s => s);
                    break;
            }

            var lstUser = await lst.Skip((pageno - 1) * pageSize).Take(pageSize).ToListAsync();
            Pager<string> pager = new Pager<string>(lstUser, pageno, pageSize, totalCount);

            pager.RouteValues = new System.Web.Routing.RouteValueDictionary();
            pager.RouteValues.Add("sortOrder", sortOrder);
            pager.RouteValues.Add("SearchString", SearchString);

            return View(pager);

            
        }

        //
        // GET: /Users/Details/5
        public async Task<ActionResult> Edit(string username)
        {
            if (username == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var ugrp = await GroupManager.FindByUserName(username).Select(x => x.Id).ToListAsync();
            ViewBag.UserName = username;
            ViewBag.Groups = GroupManager.Groups.Select(x => new SelectListItem()
            {
                Selected = ugrp.Contains(x.Id),
                Text = x.Name,
                Value = x.Id
            });
            ViewBag.Permissions = PermissionManager.FindPermissionsByUserName(username);
            return View();
        }

        //
        // POST: /Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(string username, params string[] SelectedGroup)
        {
            if (string.IsNullOrWhiteSpace(username)|| !GroupManager.IsExistUser(username))
            {
                return HttpNotFound();
            }
            
            if (ModelState.IsValid)
            {
                SelectedGroup = SelectedGroup ?? new string[] { };

                var lstSelectedGroup = SelectedGroup.ToList();

                if (username == UGConstants.AccountAdmin)
                {
                    var grp = GroupManager.FindByName(UGConstants.GroupAdmin);
                    lstSelectedGroup.Add(grp.Id);
                }

                var userRoles = GroupManager.FindByUserName(username).Select(x => x.Id);

                var result = await GroupManager.AddUserToGroupsAsync(username, lstSelectedGroup.Except(userRoles).ToArray<string>());

                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", result.Errors.First());
                    return View();
                }
                result = await GroupManager.RemoveUserFromGroupsAsync(username, userRoles.Except(lstSelectedGroup).ToArray<string>());

                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", result.Errors.First());
                    return View();
                }
                return RedirectToAction("Edit",new { username = username });
            }
            ModelState.AddModelError("", "Something failed.");
            return View();
        }
    }
}
