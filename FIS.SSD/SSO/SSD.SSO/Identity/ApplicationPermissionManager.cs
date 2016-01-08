using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;


namespace SSD.SSO.Identity
{

    public class ApplicationPermissionManager:IDisposable
    {
        private ApplicationPermissionStore _permissionStore;
        private ApplicationDbContext _db;
        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;
        private bool _disposed;

        public ApplicationPermissionManager()
        {
            _db = HttpContext.Current
                .GetOwinContext().Get<ApplicationDbContext>();
            _userManager = HttpContext.Current
               .GetOwinContext().GetUserManager<ApplicationUserManager>();
            _roleManager = HttpContext.Current
                .GetOwinContext().Get<ApplicationRoleManager>();
            _permissionStore = new ApplicationPermissionStore(_db);
        }

        public ApplicationPermissionManager(ApplicationDbContext context)
        {
            _db = context;
            _userManager = new ApplicationUserManager(_db);
            _roleManager = new ApplicationRoleManager(_db);
            _permissionStore = new ApplicationPermissionStore(_db);
        }
        public static ApplicationPermissionManager Create(IdentityFactoryOptions<ApplicationPermissionManager> options, IOwinContext context)
        {
            return new ApplicationPermissionManager(context.Get<ApplicationDbContext>());
        }

        public IQueryable<ApplicationPermission> Permissions
        {
            get
            {
                return _permissionStore.Permissions;
            }
        }


        public async Task<IdentityResult> CreatePermissionAsync(ApplicationPermission per)
        {
            await _permissionStore.CreateAsync(per);
            return IdentityResult.Success;
        }


        public IdentityResult CreatePermission(ApplicationPermission per)
        {
            _permissionStore.Create(per);
            return IdentityResult.Success;
        }


        public IdentityResult SetPermissionRoles(string perId, params string[] roleNames)
        {
            // Clear all the roles associated with this Permission:
            var thisPermission = this.FindById(perId);
            thisPermission.ApplicationRoles.Clear();
            _db.SaveChanges();

            // Add the new roles passed in:
            var newRoles = _roleManager.Roles.Where(r => roleNames.Any(n => n == r.Name));
            foreach(var role in newRoles)
            {
                thisPermission.ApplicationRoles.Add(new ApplicationPermissionRole { ApplicationPermissionId = perId, ApplicationRoleId = role.Id });
            }
            _db.SaveChanges();

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> SetPermissionRolesAsync(string perId, params string[] roleNames)
        {
            // Clear all the roles associated with this Permission:
            var thisPermission = await this.FindByIdAsync(perId);
            thisPermission.ApplicationRoles.Clear();
            await _db.SaveChangesAsync();

            // Add the new roles passed in:
            var newRoles = _roleManager.Roles.Where(r => roleNames.Any(n => n == r.Name));
            foreach (var role in newRoles)
            {
                thisPermission.ApplicationRoles.Add(new ApplicationPermissionRole { ApplicationPermissionId = perId, ApplicationRoleId = role.Id });
            }
            await _db.SaveChangesAsync();
            return IdentityResult.Success;
        }

        public IdentityResult SetRolePermissions(string rolId, params string[] perIds)
        {
            // Clear all the roles associated with this Permission:
            var rol = _roleManager.FindById(rolId);
            rol.ApplicationPermissions.Clear();
            _db.SaveChanges();

            // Add the new roles passed in:
            var perIdsObj = this.Permissions.Where(r => perIds.Any(n => n == r.Id));
            foreach (var perId in perIdsObj)
            {
                rol.ApplicationPermissions.Add(new ApplicationPermissionRole { ApplicationPermissionId = rolId, ApplicationRoleId = perId.Id });
            }
            _db.SaveChanges();

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> SetRolePermissionsAsync(string rolId, params string[] perIds)
        {
            // Clear all the roles associated with this Permission:
            var rol = await _roleManager.FindByIdAsync(rolId);
            rol.ApplicationPermissions.Clear();
            await _db.SaveChangesAsync();

            // Add the new roles passed in:
            var perIdsObj = this.Permissions.Where(r => perIds.Any(n => n == r.Id));
            foreach (var perId in perIdsObj)
            {
                rol.ApplicationPermissions.Add(new ApplicationPermissionRole { ApplicationPermissionId = rolId, ApplicationRoleId = perId.Id });
            }
            await _db.SaveChangesAsync();

            return IdentityResult.Success;
        }
        public async Task<IdentityResult> DeletePermissionAsync(string perId)
        {
            var per = await this.FindByIdAsync(perId);
            if (per == null)
            {
                throw new ArgumentNullException("Permission");
            }

            // remove the roles from the Permission:
            per.ApplicationRoles.Clear();

            // Remove the Permission itself:
            _db.ApplicationPermissions.Remove(per);

            await _db.SaveChangesAsync();

            return IdentityResult.Success;
        }
        public IdentityResult SaveAll(List<ApplicationPermission> pers)
        {
            foreach (var per in pers)
            {
                _db.ApplicationPermissions.Add(per);
            }
            _db.SaveChanges();
            return IdentityResult.Success;
        }
        public IdentityResult DeleteAll()
        {
            foreach (var per in Permissions)
            {
                // Remove the Permission itself:
                _db.ApplicationPermissions.Remove(per);
            }
            _db.SaveChanges();
            return IdentityResult.Success;
        }

        public IdentityResult DeletePermission(string perId)
        {
            var per = this.FindById(perId);
            if(per == null)
            {
                throw new ArgumentNullException("User");
            }

            // remove the roles from the Permission:
            per.ApplicationRoles.Clear();

            // Remove the Permission itself:
            _db.ApplicationPermissions.Remove(per);

            _db.SaveChanges();

            return IdentityResult.Success;
        }


        public async Task<IdentityResult> UpdatePermissionAsync(ApplicationPermission per)
        {
            await _permissionStore.UpdateAsync(per);
            return IdentityResult.Success;
        }


        public IdentityResult UpdatePermission(ApplicationPermission per)
        {
            _permissionStore.Update(per);
            return IdentityResult.Success;
        }

        public async Task<IEnumerable<ApplicationPermission>> GetPermissionsAsync(string rolId)
        {
            var result = new List<ApplicationPermission>();
            var userPermissions = (from g in this.Permissions
                                   where g.ApplicationRoles.Any(u => u.ApplicationRoleId == rolId)
                              select g).ToListAsync();
            return await userPermissions;
        }


        public IEnumerable<ApplicationPermission> GetPermissions(string rolId)
        {
            var result = new List<ApplicationPermission>();
            var userPermissions = (from g in this.Permissions
                              where g.ApplicationRoles.Any(u => u.ApplicationRoleId == rolId)
                              select g).ToList();
            return userPermissions;
        }


        public async Task<IEnumerable<ApplicationRole>> GetRolesAsync(string perId)
        {
            var grp = await _db.ApplicationPermissions.FirstOrDefaultAsync(g => g.Id == perId);
            var roles = await _roleManager.Roles.ToListAsync();
            var PermissionRoles = (from r in roles
                              where grp.ApplicationRoles.Any(ap => ap.ApplicationRoleId == r.Id)
                              select r).ToList();
            return PermissionRoles;
        }


        public IEnumerable<ApplicationRole> GetRoles(string perId)
        {
            var grp = _db.ApplicationPermissions.FirstOrDefault(g => g.Id == perId);
            var roles = _roleManager.Roles.ToList();
            var perRoles = from r in roles
                             where grp.ApplicationRoles.Any(ap => ap.ApplicationRoleId == r.Id)
                             select r;
            return perRoles;
        }

        public IEnumerable<ApplicationPermission> GetUserPermissions(string userId)
        {
            var rols = _userManager.FindById(userId).Roles.Select(x => x.RoleId);
            var perRoles = from p in this.Permissions
                           where p.ApplicationRoles.Any(ap => rols.Contains(ap.ApplicationRoleId))
                           select p;
            
            return perRoles.Distinct();
        }


        public async Task<IEnumerable<ApplicationPermission>> GetUserPermissionsAsync(string userId)
        {
            var rols = await _userManager.FindByIdAsync(userId);
            var perRoles = from p in this.Permissions
                           where p.ApplicationRoles.Any(ap => rols.Roles.Select(x => x.RoleId).Contains(ap.ApplicationRoleId))
                           select p;

            return perRoles.Distinct();
        }

        public IEnumerable<ApplicationPermissionRole> GetPermissionRoles(string rolId)
        {
            var userPermissions = this.GetUserPermissions(rolId);
            var userPermissionRoles = new List<ApplicationPermissionRole>();
            foreach (var Permission in userPermissions)
            {
                userPermissionRoles.AddRange(Permission.ApplicationRoles.ToArray());
            }
            return userPermissionRoles;
        }


        public async Task<IEnumerable<ApplicationPermissionRole>> GetPermissionRolesAsync(string userId)
        {
            var userPermissions = await this.GetUserPermissionsAsync(userId);
            var userPermissionRoles = new List<ApplicationPermissionRole>();
            foreach (var Permission in userPermissions)
            {
                userPermissionRoles.AddRange(Permission.ApplicationRoles.ToArray());
            }
            return userPermissionRoles;
        }

        public async Task<ApplicationPermission> FindByIdAsync(string id)
        {
            return await _permissionStore.FindByIdAsync(id);
        }


        public ApplicationPermission FindById(string id)
        {
            return _permissionStore.FindById(id);
        }

        // DISPOSE STUFF: ===============================================

        public bool DisposeContext
        {
            get;
            set;
        }


        private void ThrowIfDisposed()
        {
            if (this._disposed)
            {
                throw new ObjectDisposedException(this.GetType().Name);
            }
        }


        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }


        protected virtual void Dispose(bool disposing)
        {
            if (this.DisposeContext && disposing && this._db != null)
            {
                this._db.Dispose();
                this._userManager.Dispose();
                this._roleManager.Dispose();
            }
            this._disposed = true;
            this._db = null;
            this._userManager = null;
            this._roleManager = null;
            this._permissionStore = null;
        }


        //public IdentityResult RefreshUserPermissionRoles(string userId)
        //{
        //    var user = _userManager.FindById(userId);
        //    if(user == null)
        //    {
        //        throw new ArgumentNullException("User");
        //    }
        //    // Remove user from previous roles:
        //    var oldUserRoles = _userManager.GetRoles(userId);
        //    if(oldUserRoles.Count > 0)
        //    {
        //        _userManager.RemoveFromRoles(userId, oldUserRoles.ToArray());
        //    }

        //    // Find teh roles this user is entitled to from Permission membership:
        //    var newPermissionRoles = this.GetUserPermissionRoles(userId);

        //    // Get the damn role names:
        //    var allRoles = _roleManager.Roles.ToList();
        //    var addTheseRoles = allRoles.Where(r => newPermissionRoles.Any(gr => gr.ApplicationRoleId == r.Id));
        //    var roleNames = addTheseRoles.Select(n => n.Name).ToArray();

        //    // Add the user to the proper roles
        //    _userManager.AddToRoles(userId, roleNames);

        //    return IdentityResult.Success;
        //}


        //public async Task<IdentityResult> RefreshUserPermissionRolesAsync(string userId)
        //{
        //    var user = await _userManager.FindByIdAsync(userId);
        //    if (user == null)
        //    {
        //        throw new ArgumentNullException("User");
        //    }
        //    // Remove user from previous roles:
        //    var oldUserRoles = await _userManager.GetRolesAsync(userId);
        //    if (oldUserRoles.Count > 0)
        //    {
        //        await _userManager.RemoveFromRolesAsync(userId, oldUserRoles.ToArray());
        //    }

        //    // Find the roles this user is entitled to from Permission membership:
        //    var newPermissionRoles = await this.GetUserPermissionRolesAsync(userId);

        //    // Get the damn role names:
        //    var allRoles = await _roleManager.Roles.ToListAsync();
        //    var addTheseRoles = allRoles.Where(r => newPermissionRoles.Any(gr => gr.ApplicationRoleId == r.Id));
        //    var roleNames = addTheseRoles.Select(n => n.Name).ToArray();

        //    // Add the user to the proper roles
        //    await _userManager.AddToRolesAsync(userId, roleNames);

        //    return IdentityResult.Success;
        //}

    }
}