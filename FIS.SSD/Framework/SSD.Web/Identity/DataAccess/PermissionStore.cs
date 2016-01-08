using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using SSD.Web.Models;

namespace SSD.Web.Identity.DataAccess
{
    public class PermissionStore : BaseStore
    {
        public PermissionStore(DbContext context):base(context)
        {
            DbEntitySet = context.Set<Permission>();
        }
        public DbSet<Permission> DbEntitySet
        {
            get;
            private set;
        }
        public virtual void Create(Permission per)
        {
            this.ThrowIfDisposed();
            if (per == null)
            {
                throw new ArgumentNullException("Permission");
            }
            this.DbEntitySet.Add(per);
            this.Context.SaveChanges();
        }
        public virtual async Task CreateAsync(Permission per)
        {
            this.ThrowIfDisposed();
            if (per == null)
            {
                throw new ArgumentNullException("Permission");
            }
            this.DbEntitySet.Add(per);
            await this.Context.SaveChangesAsync();
        }

        public virtual async Task DeleteAsync(Permission per)
        {
            this.ThrowIfDisposed();
            if (per == null)
            {
                throw new ArgumentNullException("Permission");
            }
            //Remove FK
            var dbSetPermissionGroup = this.Context.Set<PermissionGroup>();
            var lstMap = dbSetPermissionGroup.Where(x => x.PermissionId == per.Id);
            dbSetPermissionGroup.RemoveRange(lstMap);

            this.DbEntitySet.Remove(per);
            await this.Context.SaveChangesAsync();
        }
        public virtual void Delete(Permission per)
        {
            this.ThrowIfDisposed();
            if (per == null)
            {
                throw new ArgumentNullException("Permission");
            }
            //Remove FK
            var dbSetPermissionGroup = this.Context.Set<PermissionGroup>();
            var lstMap = dbSetPermissionGroup.Where(x => x.PermissionId == per.Id);
            dbSetPermissionGroup.RemoveRange(lstMap);

            this.DbEntitySet.Remove(per);
            this.Context.SaveChanges();
        }

        public Task<Permission> FindByIdAsync(string id)
        {
            this.ThrowIfDisposed();
            return this.DbEntitySet.FindAsync(new object[] { id });
        }
        public Permission FindById(string id)
        {
            this.ThrowIfDisposed();
            return this.DbEntitySet.Find(new object[] { id });
        }

        public virtual async Task UpdateAsync(Permission per)
        {
            this.ThrowIfDisposed();
            if (per == null)
            {
                throw new ArgumentNullException("Permission");
            }
            if (per != null)
            {
                this.Context.Entry<Permission>(per).State = EntityState.Modified;
            }
            await this.Context.SaveChangesAsync();
        }
        public virtual void Update(Permission per)
        {
            this.ThrowIfDisposed();
            if (per == null)
            {
                throw new ArgumentNullException("Permission");
            }
            if (per != null)
            {
                this.Context.Entry<Permission>(per).State = EntityState.Modified;
            }
            this.Context.SaveChanges();
        }
    }
}