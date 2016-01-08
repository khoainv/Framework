using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace SSD.SSO.Identity
{
    public class PermissionStoreBase
    {
        public DbContext Context
        {
            get;
            private set;
        }


        public DbSet<ApplicationPermission> DbEntitySet
        {
            get;
            private set;
        }


        public IQueryable<ApplicationPermission> EntitySet
        {
            get
            {
                return this.DbEntitySet;
            }
        }


        public PermissionStoreBase(DbContext context)
        {
            this.Context = context;
            this.DbEntitySet = context.Set<ApplicationPermission>();
        }


        public void Create(ApplicationPermission entity)
        {
            this.DbEntitySet.Add(entity);
        }


        public void Delete(ApplicationPermission entity)
        {
            this.DbEntitySet.Remove(entity);
        }


        public virtual Task<ApplicationPermission> GetByIdAsync(object id)
        {
            return this.DbEntitySet.FindAsync(new object[] { id });
        }


        public virtual ApplicationPermission GetById(object id)
        {
            return this.DbEntitySet.Find(new object[] { id });
        }


        public virtual void Update(ApplicationPermission entity)
        {
            if (entity != null)
            {
                this.Context.Entry<ApplicationPermission>(entity).State = EntityState.Modified;
            }
        }
    }
    public class ApplicationPermissionStore : IDisposable
    {
        private bool _disposed;
        private PermissionStoreBase _permissionStore;


        public ApplicationPermissionStore(ApplicationDbContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            this.Context = context;
            this._permissionStore = new PermissionStoreBase(context);
        }

        public IQueryable<ApplicationPermission> Permissions
        {
            get
            {
                return this._permissionStore.EntitySet;
            }
        }
        
        public DbContext Context
        {
            get;
            private set;
        }


        public virtual void Create(ApplicationPermission per)
        {
            this.ThrowIfDisposed();
            if (per == null)
            {
                throw new ArgumentNullException("Permission");
            }
            this._permissionStore.Create(per);
            this.Context.SaveChanges();
        }


        public virtual async Task CreateAsync(ApplicationPermission per)
        {
            this.ThrowIfDisposed();
            if (per == null)
            {
                throw new ArgumentNullException("Permission");
            }
            this._permissionStore.Create(per);
            await this.Context.SaveChangesAsync();
        }


        public virtual async Task DeleteAsync(ApplicationPermission per)
        {
            this.ThrowIfDisposed();
            if (per == null)
            {
                throw new ArgumentNullException("Permission");
            }
            this._permissionStore.Delete(per);
            await this.Context.SaveChangesAsync();
        }


        public virtual void Delete(ApplicationPermission per)
        {
            this.ThrowIfDisposed();
            if (per == null)
            {
                throw new ArgumentNullException("Permission");
            }
            this._permissionStore.Delete(per);
            this.Context.SaveChanges();
        }


        public Task<ApplicationPermission> FindByIdAsync(string roleId)
        {
            this.ThrowIfDisposed();
            return this._permissionStore.GetByIdAsync(roleId);
        }


        public ApplicationPermission FindById(string roleId)
        {
            this.ThrowIfDisposed();
            return this._permissionStore.GetById(roleId);
        }

        //public Task<ApplicationPermission> FindByNameAsync(string groupName)
        //{
        //    this.ThrowIfDisposed();
        //    return QueryableExtensions
        //        .FirstOrDefaultAsync<ApplicationPermission>(this._groupStore.EntitySet, 
        //            (ApplicationPermission u) => u.Name.ToUpper() == groupName.ToUpper());
        //}


        public virtual async Task UpdateAsync(ApplicationPermission per)
        {
            this.ThrowIfDisposed();
            if (per == null)
            {
                throw new ArgumentNullException("Permission");
            }
            this._permissionStore.Update(per);
            await this.Context.SaveChangesAsync();
        }


        public virtual void Update(ApplicationPermission per)
        {
            this.ThrowIfDisposed();
            if (per == null)
            {
                throw new ArgumentNullException("Permission");
            }
            this._permissionStore.Update(per);
            this.Context.SaveChanges();
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
            if (this.DisposeContext && disposing && this.Context != null)
            {
                this.Context.Dispose();
            }
            this._disposed = true;
            this.Context = null;
            this._permissionStore = null;
        }
    }
}