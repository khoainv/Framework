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
    public class PermissionManager : PermissionManagerBase
    {
        public PermissionManager():base(HttpContext.Current.GetOwinContext().Get<IdentityDbContext>())
        {
        }
        public PermissionManager(IdentityDbContext context) : base(context)
        {
        }
        public static PermissionManager Create(IdentityFactoryOptions<PermissionManager> options, IOwinContext context)
        {
            return new PermissionManager(context.Get<IdentityDbContext>());
        }
    }
    public abstract class PermissionManagerBase : BaseManager,ICacheManager
    {
        private PermissionStore _permissionStore;
        private DbContext _db;

        public PermissionManagerBase(DbContext context)
        {
            _db = context;
            _permissionStore = new PermissionStore(_db);
        }
        public DbContext Context
        {
            get
            {
                return _db;
            }
        }
        #region Cache
        static object obj = new object();
        static List<Permission> lstPermission;
        public List<Permission> CacheLstAllPermission
        {
            get
            {
                if (lstPermission == null)
                {
                    lock (obj)
                    {
                        if (lstPermission == null)
                        {
                            lstPermission = Permissions.ToList();
                        }
                    }
                }
                return lstPermission;
            }
        }

        public async Task UpdateCacheAsync()
        {
            lstPermission = await Permissions.ToListAsync();
        }
        public void CleanCache()
        {
            lstPermission = null;
        }
        #endregion
        public IQueryable<Permission> Permissions
        {
            get
            {
                return _permissionStore.DbEntitySet;
            }
        }
        
        public IdentityResult UpdatePermission(List<Permission> lstNew)
        {
            var lstDB = Permissions.ToList();

            foreach (var per in lstDB)
            {
                var existDbNotNew = from p in lstNew
                            where p.CompareTo(per)
                            select p;
                if (existDbNotNew.Count() == 0)
                {
                    var map = _db.Set<PermissionGroup>().Where(x => x.PermissionId == per.Id);
                    _db.Set<PermissionGroup>().RemoveRange(map);
                    _permissionStore.DbEntitySet.Remove(per);
                }
            }
            foreach (var per in lstNew)
            {
                var notDb = from p in lstDB
                            where p.CompareTo(per)
                            select p;
                if (notDb.Count() == 0)
                    _permissionStore.DbEntitySet.Add(per);
            }
            _db.SaveChanges();
            return IdentityResult.Success;
        }

        public IQueryable<Permission> FindPermissionsByUserName(string userName)
        {
            var grps = _db.Set<UserGroup>().Where(x => x.UserName == userName).Select(x=>x.GroupId).ToList();

            return FindPermissionsByGroupId(grps).Distinct();
        }
        public IQueryable<Permission> FindPermissionsByOnlyGroupId(string grpId)
        {
            var grpPers = _db.Set<PermissionGroup>().Where(x => x.GroupId == grpId).Select(x => x.PermissionId);

            var perRoles = from p in this.Permissions
                           where grpPers.Contains(p.Id)
                           select p;

            return perRoles.Distinct();
        }
        public IQueryable<Permission> FindPermissionsByGroupId(List<string> grpId)
        {
            //Loop with parent Group
            var lstParentId = GetParentGroupIdById(grpId);

            var grpPers = _db.Set<PermissionGroup>().Where(x => lstParentId.Contains(x.GroupId)).Select(x => x.PermissionId);

            var perRoles = from p in this.Permissions
                           where grpPers.Contains(p.Id)
                           select p;

            return perRoles.Distinct();
        }
        public IQueryable<Permission> FindPermissionsByGroupId(string grpId)
        {
            //Loop with parent Group
            var lstParentId = GetParentGroupIdById(grpId);

            var grpPers = _db.Set<PermissionGroup>().Where(x => lstParentId.Contains(x.GroupId)).Select(x => x.PermissionId);

            var perRoles = from p in this.Permissions
                           where grpPers.Contains(p.Id)
                           select p;

            return perRoles.Distinct();
        }
        public List<string> GetParentGroupIdById(string parentId)
        {
            return GetParentGroupIdById(new List<string>() { parentId });
        }
        public List<string> GetParentGroupIdById(List<string> lstParent)
        {
            if (lstParent == null || lstParent.Count == 0)
                return new List<string>();
            var lst = _db.Set<Group>().ToList();

            List<string> lstGroup = new List<string>();
            foreach (string parent in lstParent)
                lstGroup.AddRange(GetParentGroupIdById(parent, lst));

            return lstGroup;
        }

        public async Task<Permission> FindByIdAsync(string id)
        {
            return await _permissionStore.FindByIdAsync(id);
        }
        public async Task<IQueryable<UserGroup>> FindUserGroupByIdAsync(string id)
        {
            var lstGrps = await _db.Set<PermissionGroup>().Where(x => x.PermissionId == id).Select(x => x.GroupId).Distinct().ToListAsync();

            return _db.Set<UserGroup>().Where(x => lstGrps.Contains(x.GroupId)).Distinct();
        }
        public async Task<IQueryable<Group>> FindGroupByIdAsync(string id)
        {
            var lstGrps = await _db.Set<PermissionGroup>().Where(x => x.PermissionId == id).Select(x => x.GroupId).Distinct().ToListAsync();
            return _db.Set<Group>().Where(x => lstGrps.Contains(x.Id)).Distinct();
        }
    }
}