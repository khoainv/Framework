using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using SSD.Framework;
using SSD.Framework.Collections;

namespace SSD.SSO.Config
{
    public class AppConfigStoreBase
    {
        public DbContext Context
        {
            get;
            private set;
        }


        public DbSet<AppConfig> DbEntitySet
        {
            get;
            private set;
        }


        public IQueryable<AppConfig> EntitySet
        {
            get
            {
                return this.DbEntitySet;
            }
        }


        public AppConfigStoreBase(DbContext context)
        {
            this.Context = context;
            this.DbEntitySet = context.Set<AppConfig>();
        }


        public void Create(AppConfig entity)
        {
            this.DbEntitySet.Add(entity);
        }


        public void Delete(AppConfig entity)
        {
            this.DbEntitySet.Remove(entity);
        }


        public virtual Task<AppConfig> GetByIdAsync(object id)
        {
            return this.DbEntitySet.FindAsync(new object[] { id });
        }


        public virtual AppConfig GetById(object id)
        {
            return this.DbEntitySet.Find(new object[] { id });
        }


        public virtual void Update(AppConfig entity)
        {
            if (entity != null)
            {
                this.Context.Entry<AppConfig>(entity).State = EntityState.Modified;
            }
        }
    }
    public class AppConfigStore : IDisposable, IAppConfigStore
    {
        private bool _disposed;
        private AppConfigStoreBase _configStore;


        public AppConfigStore(DbContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            this.Context = context;
            this._configStore = new AppConfigStoreBase(context);
        }
        public AppConfigStore()
        {
        }
        public void SetDbContext(DbContext context)
        {
            this.Context = context;
            this._configStore = new AppConfigStoreBase(context);
        }
        public SortableBindingList<AppConfig> AppConfigs
        {
            get
            {
                return this._configStore.EntitySet.ToList().ToSortableBindingList();
            }
        }
        
        public DbContext Context
        {
            get;
            private set;
        }
        public virtual AppConfig ReadObjectByIDConfig(int id)
        {
            return this._configStore.GetById(id);
        }
        public virtual int InsertConfig(AppConfig per)
        {
            this.ThrowIfDisposed();
            if (per == null)
            {
                throw new ArgumentNullException("AppConfig");
            }
            this._configStore.Create(per);
            return this.Context.SaveChanges();
        }
        public virtual async Task CreateAsync(AppConfig per)
        {
            this.ThrowIfDisposed();
            if (per == null)
            {
                throw new ArgumentNullException("AppConfig");
            }
            this._configStore.Create(per);
            await this.Context.SaveChangesAsync();
        }


        public virtual async Task DeleteAsync(AppConfig per)
        {
            this.ThrowIfDisposed();
            if (per == null)
            {
                throw new ArgumentNullException("AppConfig");
            }
            this._configStore.Delete(per);
            await this.Context.SaveChangesAsync();
        }


        public virtual void DeleteConfig(AppConfig per)
        {
            this.ThrowIfDisposed();
            if (per == null)
            {
                throw new ArgumentNullException("AppConfig");
            }
            this._configStore.Delete(per);
            this.Context.SaveChanges();
        }
        public virtual async Task UpdateAsync(AppConfig per)
        {
            this.ThrowIfDisposed();
            if (per == null)
            {
                throw new ArgumentNullException("AppConfig");
            }
            this._configStore.Update(per);
            await this.Context.SaveChangesAsync();
        }


        public virtual void UpdateConfig(AppConfig per)
        {
            this.ThrowIfDisposed();
            if (per == null)
            {
                throw new ArgumentNullException("AppConfig");
            }
            this._configStore.Update(per);
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
            this._configStore = null;
        }
    }
}