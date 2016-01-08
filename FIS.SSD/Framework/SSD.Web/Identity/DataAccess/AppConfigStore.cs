using System;
using System.Data.Entity;
using System.Threading.Tasks;
using SSD.Web.Models;

namespace SSD.Web.Identity.DataAccess
{
    public class AppConfigStore : BaseStore
    {
        public AppConfigStore(DbContext context):base(context)
        {
            DbEntitySet = context.Set<AppConfig>();
        }
        public DbSet<AppConfig> DbEntitySet
        {
            get;
            private set;
        }
        public virtual void Create(AppConfig per)
        {
            this.ThrowIfDisposed();
            if (per == null)
            {
                throw new ArgumentNullException("AppConfig");
            }
            this.DbEntitySet.Add(per);
            this.Context.SaveChanges();
        }
        public virtual async Task CreateAsync(AppConfig per)
        {
            this.ThrowIfDisposed();
            if (per == null)
            {
                throw new ArgumentNullException("AppConfig");
            }
            this.DbEntitySet.Add(per);
            await this.Context.SaveChangesAsync();
        }

        public virtual async Task DeleteAsync(AppConfig per)
        {
            this.ThrowIfDisposed();
            if (per == null)
            {
                throw new ArgumentNullException("AppConfig");
            }
            this.DbEntitySet.Remove(per);
            await this.Context.SaveChangesAsync();
        }
        public virtual void Delete(AppConfig per)
        {
            this.ThrowIfDisposed();
            if (per == null)
            {
                throw new ArgumentNullException("AppConfig");
            }
            this.DbEntitySet.Remove(per);
            this.Context.SaveChanges();
        }

        public Task<AppConfig> FindByIdAsync(int id)
        {
            this.ThrowIfDisposed();
            return this.DbEntitySet.FindAsync(new object[] { id });
        }
        public AppConfig FindById(int id)
        {
            this.ThrowIfDisposed();
            return this.DbEntitySet.Find(new object[] { id });
        }

        public virtual async Task UpdateAsync(AppConfig per)
        {
            this.ThrowIfDisposed();
            if (per == null)
            {
                throw new ArgumentNullException("AppConfig");
            }
            if (per != null)
            {
                this.Context.Entry<AppConfig>(per).State = EntityState.Modified;
            }
            await this.Context.SaveChangesAsync();
        }
        public virtual void Update(AppConfig per)
        {
            this.ThrowIfDisposed();
            if (per == null)
            {
                throw new ArgumentNullException("AppConfig");
            }
            if (per != null)
            {
                this.Context.Entry<AppConfig>(per).State = EntityState.Modified;
            }
            this.Context.SaveChanges();
        }
    }
}