using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using SSD.Web.Identity.DataAccess;
using SSD.Web.Models;

namespace SSD.Web.Identity
{
    public class GroupManager : GroupManagerBase
    {
        public GroupManager() : base(HttpContext.Current.GetOwinContext().Get<IdentityDbContext>())
        {
        }
        public GroupManager(IdentityDbContext context) : base(context)
        {
        }
        public static GroupManager Create(IdentityFactoryOptions<GroupManager> options, IOwinContext context)
        {
            return new GroupManager(context.Get<IdentityDbContext>());
        }
        public override bool IsExistUser(string userName)
        {
            var grpInfo = from ug in Context.Set<User>()
                          where ug.UserName == userName
                          select ug;
            return grpInfo.Count() > 0;
        }
    }
    public abstract class GroupManagerBase : BaseManager, ICacheManager
    {
        private GroupStore _store;
        private DbContext _db;

        public GroupManagerBase(DbContext context)
        {
            _db = context;
            _store = new GroupStore(_db);
        }
        #region Cache
        static object obj = new object();
        static List<Group> lstGroup;
        public List<Group> CacheLstAllGroup
        {
            get
            {
                if (lstGroup == null)
                {
                    lock (obj)
                    {
                        if (lstGroup == null)
                        {
                            lstGroup = Groups.ToList();
                        }
                    }
                }
                return lstGroup;
            }
        }
        public async Task UpdateCacheAsync()
        {
            lstGroup = await Groups.ToListAsync();
            lstUserGroup = await _db.Set<UserGroup>().ToListAsync();
        }
        public void CleanCache()
        {
            lstGroup = null;
        }
        static List<UserGroup> lstUserGroup;
        public List<UserGroup> CacheLstAllUserGroup
        {
            get
            {
                if (lstUserGroup == null)
                {
                    lock (obj)
                    {
                        if (lstUserGroup == null)
                        {
                            lstUserGroup = _db.Set<UserGroup>().ToList();
                        }
                    }
                }
                return lstUserGroup;
            }
        }
        #endregion
        public DbContext Context
        {
            get
            {
                return _db;
            }
        }
        public IQueryable<Group> Groups
        {
            get
            {
                return _store.DbEntitySet;
            }
        }
        public IQueryable<UserGroup> UserGroups
        {
            get
            {
                return Context.Set<UserGroup>();
            }
        }
        #region Searching
        public bool IsGroupCache(string userName, string grpName)
        {
            var grpInfo = from g in CacheLstAllGroup
                          join ug in CacheLstAllUserGroup on g.Id equals ug.GroupId
                          where ug.UserName == userName && g.Name == grpName
                          select g;
            return grpInfo.Count()>0;
        }
        public virtual bool IsExistUser(string userName)
        {
            var grpInfo = from ug in _db.Set<UserGroup>()
                          where ug.UserName == userName
                          select ug;
            return grpInfo.Count()>0;
        }
        public bool IsExistInUserGroup(string userName,string grpId)
        {
            var grpInfo = from ug in _db.Set<UserGroup>()
                          where ug.UserName == userName && ug.GroupId == grpId
                          select ug;
            return grpInfo.Count() > 0;
        }
        public Group FindByName(string name)
        {
            var grpInfo = from g in Groups where g.Name == name select g;
            if (grpInfo.Count() > 0)
                return grpInfo.First();
            return null;
        }
        
        public IQueryable<Group> FindByUserName(string userName)
        {
            var grpInfo = from g in Groups
                          join ug in _db.Set<UserGroup>() on g.Id equals ug.GroupId
                          where ug.UserName == userName select g;
            return grpInfo.Distinct();
        }
        public IQueryable<UserGroup> FindUserGroupByOnlyId(string grpId)
        {
            var grpInfo = from ug in _db.Set<UserGroup>()
                          where ug.GroupId == grpId
                          select ug;
            return grpInfo.Distinct();
        }
        public IQueryable<UserGroup> FindUserGroupById(string grpId)
        {
            //Loop with children Group
            var lstGrpId = GetChildrenGroupIdById(grpId);
            var grpInfo = from ug in _db.Set<UserGroup>()
                          where lstGrpId.Contains(ug.GroupId)// == grpId
                          select ug;
            return grpInfo.Distinct();
        }
        public UserGroup FindUserGroupById(string userName, string grpId)
        {
            var grpInfo = from g in _db.Set<UserGroup>() where g.UserName == userName && g.GroupId == grpId select g;
            if (grpInfo.Count() > 0)
                return grpInfo.First();
            return null;
        }
        #endregion

        #region Tree
        static List<Group> cacheGroupTree;
        public List<Group> CacheGroupTree
        {
            get
            {
                if (cacheGroupTree == null)
                {
                    cacheGroupTree = BuildTree();
                }
                return cacheGroupTree;
            }
        }
        public List<Group> BuildTree()
        {
            var items = Groups.ToList();
            if (items.Count() > 0)
            {
                items.ToList().ForEach(item => item.Children = items.Where(child => child.ParentId == item.Id).ToList());
                List<Group> topItems = items.Where(item => string.IsNullOrWhiteSpace(item.ParentId)).ToList();
                return topItems;
            }
            return new List<Group>();
        }
        public Group BuildTreeChildren(string grpId)
        {
            var items = Groups.ToList();
            if (items.Count() > 0)
            {
                items.ToList().ForEach(item => item.Children = items.Where(child => child.ParentId == item.Id).ToList());
                var topItems = items.Where(item => item.Id == grpId);
                if (topItems.Count() > 0)
                    return topItems.First();
            }
            return null;
        }
        //Get All User In Group
        public List<string> GetChildrenGroupIdById(string parentId)
        {
            List<string> lst = new List<string>();
            var cat = BuildTreeChildren(parentId);
            if (cat != null)
            {
                lst.Add(parentId);
                GetChildrenDequy(cat, lst);
            }
            if (lst.Count == 0)
                return null;
            return lst;
        }
        private void GetChildrenDequy(Group cat, List<string> lst)
        {
            if (cat.Children == null)
                return;
            foreach (var t1 in cat.Children)
            {
                lst.Add(t1.Id);
                if (!(t1.Children == null || t1.Children.Count <= 0))
                {
                    GetChildrenDequy(t1, lst);
                }
            }
        }

        public List<string> GetParentGroupIdById(string parentId)
        {
            var lst = Groups.ToList();
            return GetParentGroupIdById(parentId, lst);
        }

        #endregion

        #region Foreignkey CRUD
        public IdentityResult AddUserToGroup(string userName, string grpId)
        {
            _db.Set<UserGroup>().Add(new UserGroup() { GroupId = grpId, UserName = userName });
            _db.SaveChanges();
            return IdentityResult.Success;
        }

        public IdentityResult AddUserToGroups(string userName, string[] grpId)
        {
            foreach(string grp in grpId)
                _db.Set<UserGroup>().Add(new UserGroup() { GroupId = grp, UserName = userName });
            _db.SaveChanges();
            return IdentityResult.Success;
        }
        
        public async Task<IdentityResult> AddUserToGroupsAsync(string userName, string[] grpId)
        {
            foreach (string grp in grpId)
                _db.Set<UserGroup>().Add(new UserGroup() { GroupId = grp, UserName = userName });
            await _db.SaveChangesAsync();
            return IdentityResult.Success;
        }
        public async Task<IdentityResult> RemoveUserFromGroupsAsync(string userName, string[] grpId)
        {
            var lst = from ug in _db.Set<UserGroup>() where ug.UserName == userName && grpId.Contains(ug.GroupId) select ug;
            _db.Set<UserGroup>().RemoveRange(lst);
            await _db.SaveChangesAsync();
            return IdentityResult.Success;
        }
        public async Task<IdentityResult> AddPermissionToGroupsAsync(string grpId, string[] perId)
        {
            foreach (string per in perId)
                _db.Set<PermissionGroup>().Add(new PermissionGroup() { GroupId = grpId, PermissionId = per });
            await _db.SaveChangesAsync();
            return IdentityResult.Success;
        }
        public async Task<IdentityResult> RemovePermissionFromGroupsAsync(string grpId, string[] perId)
        {
            var lst = from ug in _db.Set<PermissionGroup>() where ug.GroupId == grpId && perId.Contains(ug.PermissionId) select ug;
            _db.Set<PermissionGroup>().RemoveRange(lst);
            await _db.SaveChangesAsync();
            return IdentityResult.Success;
        }
        #endregion

        #region CRUD
        public async Task<IdentityResult> CreateAsync(Group per)
        {
            await _store.CreateAsync(per);
            return IdentityResult.Success;
        }
        public IdentityResult Create(Group per)
        {
            _store.Create(per);
            return IdentityResult.Success;
        }
        
        public async Task<IdentityResult> DeleteAsync(string grpId)
        {
            var grp = await this.FindByIdAsync(grpId);
            if (grp == null)
            {
                throw new ArgumentNullException("Group");
            }
            await _store.DeleteAsync(grp);
            return IdentityResult.Success;
        }
        public IdentityResult Delete(string grpId)
        {
            var grp = this.FindById(grpId);
            if(grp == null)
            {
                throw new ArgumentNullException("Group");
            }
            _store.Delete(grp);
            return IdentityResult.Success;
        }
        public IdentityResult Detach(Group per)
        {
            _store.Detach(per);
            return IdentityResult.Success;
        }
        public async Task<IdentityResult> UpdateAsync(Group per)
        {
            await _store.UpdateAsync(per);
            return IdentityResult.Success;
        }
        public IdentityResult Update(Group per)
        {
            _store.Update(per);
            return IdentityResult.Success;
        }

        public async Task<Group> FindByIdAsync(string id)
        {
            return await _store.FindByIdAsync(id);
        }
        public Group FindById(string id)
        {
            return _store.FindById(id);
        }

        #endregion
    }
}