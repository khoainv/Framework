using SSD.Web.Mvc.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Data.Entity;
using System;
using SSD.Web.Identity;
using SSD.Web.Models;
using SSD.Framework;

namespace SSD.Web.Mvc.Controllers
{
    [AdminAuthorize(Groups = "Admin")]
    public class GroupsController : GroupsControllerBase
    {
        protected override void SetManager()
        {
            _grpManager = HttpContext.GetOwinContext().Get<GroupManager>();
            _perManager = HttpContext.GetOwinContext().Get<PermissionManager>();
        }
        public GroupsController() : base(null,null)
        {
        }
        public GroupsController(GroupManager grpManager, PermissionManager perManager) : base(grpManager, perManager)//, grpManager)
        {
        }
    }
    public abstract class GroupsControllerBase : Controller
    {
        protected abstract void SetManager();
        protected GroupManagerBase _grpManager;
        protected PermissionManagerBase _perManager;
        public GroupsControllerBase(GroupManagerBase grpManager, PermissionManagerBase perManager)
        {
            _grpManager = grpManager;
            _perManager = perManager;
        }
        public GroupManagerBase GroupManager
        {
            get
            {
                if (_grpManager == null)
                    SetManager();
                return _grpManager;
            }
        }
        public PermissionManagerBase PermissionManager
        {
            get
            {
                if (_perManager == null)
                    SetManager();
                return _perManager;
            }
        }
        public async Task<ActionResult> AddUsers(string id, string sortOrder, int? page, string UserName, string SearchString = "")
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            int pageno = 0;
            ViewBag.GroupId = id;
            ViewBag.CurrentSort = sortOrder;
            string orderby = string.IsNullOrWhiteSpace(sortOrder) ? "name_desc" : "";
            ViewBag.NameSortParm = orderby;
            ViewBag.CurrentSearchKey = SearchString;
            pageno = page == null ? 1 : page.Value;
            ViewBag.Page = pageno;
            int pageSize = UGConstants.DefaultPageSize;

            var grp = GroupManager.FindById(id);
            if (grp == null)
            {
                return HttpNotFound();
            }

            if (!string.IsNullOrWhiteSpace(UserName))
            {
                if (GroupManager.IsExistUser(UserName))
                {

                    if (!GroupManager.IsExistInUserGroup(UserName, id))
                        GroupManager.AddUserToGroup(UserName, id);
                    else
                        ViewBag.NotExistUser = "'Người dùng' đã được thêm vào hệ thống rồi, đề nghị kiểm tra lại";
                }
                else
                {
                    ViewBag.NotExistUser = "Hiện tại trong hệ thống không tồn tại 'Người dùng' đề nghị bạn kiểm tra lại";
                }
            }
            var lst = GroupManager.FindUserGroupByOnlyId(id).Distinct();
            if (!String.IsNullOrEmpty(SearchString))
            {
                lst = lst.Where(s => s.UserName.ToUpper().Contains(SearchString.ToUpper()));
            }

            if (lst.Count() > 0)
            {
                long totalCount = lst.LongCount();
                switch (sortOrder)
                {
                    case "name_desc":
                        lst = lst.OrderByDescending(s => s.UserName);
                        break;
                    default:
                        lst = lst.OrderBy(s => s.UserName);
                        break;
                }

                var lstUser = await lst.Skip((pageno - 1) * pageSize).Take(pageSize).Select(x=>x.UserName).ToListAsync();
                Pager<string> pager = new Pager<string>(lstUser, pageno, pageSize, totalCount);

                pager.RouteValues = new System.Web.Routing.RouteValueDictionary();
                pager.RouteValues.Add("sortOrder", sortOrder);
                pager.RouteValues.Add("SearchString", SearchString);
                ViewBag.UserPaging = pager;
            }
            return View(grp);
        }
        public async Task<ActionResult> DeleteInGroup(string GroupId, string userName, string sortOrder, int? page, string SearchString = "")
        {
            if (userName != UGConstants.AccountAdmin)
            {
                await GroupManager.RemoveUserFromGroupsAsync(userName, new string[] { GroupId });
                WorkContextBase.DeleteUser(userName);
            }
            return RedirectToAction("AddUsers", new { id = GroupId, SearchString = SearchString, sortOrder = sortOrder, page = page });
        }
        // GET: Admin/Groups
        public ActionResult Index()
        {
            return View(GroupManager.Groups);
        }

        // GET: Admin/Groups/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Group group = await GroupManager.FindByIdAsync(id);
            if (group == null)
            {
                return HttpNotFound();
            }
            return View(group);
        }

        // GET: Admin/Groups/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Groups/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Name,Description,ParentId")] Group group)
        {
            if (ModelState.IsValid)
            {
                await GroupManager.CreateAsync(group);
                return RedirectToAction("Index");
            }

            return View(group);
        }
        private async Task<GroupViewModel> AttachEditAsync(GroupViewModel model, string[] SelectedPermision)
        {
            var grpSelected = await PermissionManager.FindPermissionsByOnlyGroupId(model.Id).Select(x => x.Id).ToListAsync();

            if (SelectedPermision != null)
                grpSelected = SelectedPermision.ToList();
            model.AllPermissionsInGroupList = await PermissionManager.FindPermissionsByGroupId(model.Id).ToListAsync();
            model.PermissionsList = await PermissionManager.Permissions
                .Select(x => new PermissionViewModel() { Permission = x, Selected = grpSelected.Contains(x.Id) })
                .ToListAsync();
            model.AllUsersInGroupList = await GroupManager.FindUserGroupById(model.Id).ToListAsync();
            return model;
        }
        // GET: Admin/Groups/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Group group = await GroupManager.FindByIdAsync(id);
            if (group == null)
            {
                return HttpNotFound();
            }
            var model = new GroupViewModel(group);
            model = await AttachEditAsync(model, null);
            return View(model);
        }

        // POST: Admin/Groups/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,Description,ParentId")] Group group
            ,params string[] SelectedPermision)
        {
            Group oldgrp = await GroupManager.FindByIdAsync(group.Id);
            if (oldgrp == null)
            {
                return HttpNotFound();
            }
            if(oldgrp.Name == UGConstants.GroupAdmin && group.Name!=UGConstants.GroupAdmin)
            {
                ModelState.AddModelError("Name","Không được sửa tên của Group Admin");
            }
            if (oldgrp.Name == UGConstants.GroupAnonymous && group.Name != UGConstants.GroupAnonymous)
            {
                ModelState.AddModelError("Name", "Không được sửa tên của Group Anonymous");
            }
            if (ModelState.IsValid)
            {
                GroupManager.Detach(oldgrp);
                var result = await GroupManager.UpdateAsync(group);
                if (result.Succeeded)
                {
                    var perInGrp = PermissionManager.FindPermissionsByOnlyGroupId(group.Id).Select(x => x.Id);
                    SelectedPermision = SelectedPermision ?? new string[] { };
                    result = await GroupManager.AddPermissionToGroupsAsync(group.Id, SelectedPermision.Except(perInGrp).ToArray<string>());
                    if (result.Succeeded)
                    {
                        result = await GroupManager.RemovePermissionFromGroupsAsync(group.Id, perInGrp.Except(SelectedPermision).ToArray<string>());
                        if (result.Succeeded)
                        {
                            WorkContextBase.ClearCache();
                            return RedirectToAction("Index");
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("", result.Errors.First());
                }

            }
            var model = new GroupViewModel(group);
            model = await AttachEditAsync(model, SelectedPermision);
            return View(model);
        }

        // GET: Admin/Groups/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Group group = await GroupManager.FindByIdAsync(id);
            if (group == null)
            {
                return HttpNotFound();
            }
            return View(group);
        }

        // POST: Admin/Groups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            Group group = await GroupManager.FindByIdAsync(id);
            if (group == null)
            {
                return HttpNotFound();
            }
            if (UGConstants.GroupAdmin == group.Name)
            {
                ModelState.AddModelError("Name", "Group Admin là không được xóa!");
                ViewBag.MsgError = "Group Admin là không được xóa!";
            }
            if (UGConstants.GroupAnonymous == group.Name)
            {
                ModelState.AddModelError("Name", "Group Anonymous là không được xóa!");
                ViewBag.MsgError = "Group Anonymous là không được xóa!";
            }
            if (ModelState.IsValid)
            {
                await GroupManager.DeleteAsync(id);
                WorkContextBase.ClearCache();
                return RedirectToAction("Index");
            }
            return View(group);
        }
    }
}
