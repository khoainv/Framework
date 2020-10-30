using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using SSD.Web.Models;

namespace SSD.Web.Identity.DataAccess
{
    public class IoTClientStore : BaseStore
    {
        public IoTClientStore(DbContext context):base(context)
        {
            DbEntitySet = context.Set<IoTClient>();
        }
        public DbSet<IoTClient> DbEntitySet
        {
            get;
            private set;
        }
        public virtual void Create(IoTClient per)
        {
            this.ThrowIfDisposed();
            if (per == null)
            {
                throw new ArgumentNullException("IdentityClient");
            }
            this.DbEntitySet.Add(per);
            this.Context.SaveChanges();
        }
        public virtual async Task CreateAsync(IoTClient per)
        {
            this.ThrowIfDisposed();
            if (per == null)
            {
                throw new ArgumentNullException("IdentityClient");
            }
            this.DbEntitySet.Add(per);
            await this.Context.SaveChangesAsync();
        }

        public virtual async Task DeleteAsync(IoTClient grp)
        {
            SupportDelete(grp);
            await this.Context.SaveChangesAsync();
        }
        public virtual void Delete(IoTClient grp)
        {
            SupportDelete(grp);
            this.Context.SaveChanges();
        }
        private void SupportDelete(IoTClient grp)
        {
            this.ThrowIfDisposed();
            if (grp == null)
            {
                throw new ArgumentNullException("IdentityClient");
            }

            this.DbEntitySet.Remove(grp);
        }

        public Task<IoTClient> FindByIdAsync(string id)
        {
            this.ThrowIfDisposed();
            return this.DbEntitySet.FindAsync(new object[] { id });
        }
        public IoTClient FindById(string id)
        {
            this.ThrowIfDisposed();
            return this.DbEntitySet.Find(new object[] { id });
        }
        public virtual void Detach(IoTClient per)
        {
            this.ThrowIfDisposed();
            if (per == null)
            {
                throw new ArgumentNullException("IdentityClient");
            }
            this.Context.Entry<IoTClient>(per).State = EntityState.Detached;
        }
        public virtual async Task UpdateAsync(IoTClient per)
        {
            this.ThrowIfDisposed();
            if (per == null)
            {
                throw new ArgumentNullException("IdentityClient");
            }
            if (per != null)
            {
                this.Context.Entry<IoTClient>(per).State = EntityState.Modified;
            }
            await this.Context.SaveChangesAsync();
        }
        public virtual void Update(IoTClient per)
        {
            this.ThrowIfDisposed();
            if (per == null)
            {
                throw new ArgumentNullException("IdentityClient");
            }
            if (per != null)
            {
                this.Context.Entry<IoTClient>(per).State = EntityState.Modified;
            }
            this.Context.SaveChanges();
        }

    }
}