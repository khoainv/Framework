using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using SSD.Web.Models;

namespace SSD.Web.Identity.DataAccess
{
    public class GroupStore : BaseStore
    {
        public GroupStore(DbContext context):base(context)
        {
            DbEntitySet = context.Set<Group>();
        }
        public DbSet<Group> DbEntitySet
        {
            get;
            private set;
        }
        public virtual void Create(Group per)
        {
            this.ThrowIfDisposed();
            if (per == null)
            {
                throw new ArgumentNullException("Group");
            }
            this.DbEntitySet.Add(per);
            this.Context.SaveChanges();
        }
        public virtual async Task CreateAsync(Group per)
        {
            this.ThrowIfDisposed();
            if (per == null)
            {
                throw new ArgumentNullException("Group");
            }
            this.DbEntitySet.Add(per);
            await this.Context.SaveChangesAsync();
        }

        public virtual async Task DeleteAsync(Group grp)
        {
            SupportDelete(grp);
            await this.Context.SaveChangesAsync();
        }
        public virtual void Delete(Group grp)
        {
            SupportDelete(grp);
            this.Context.SaveChanges();
        }
        private void SupportDelete(Group grp)
        {
            this.ThrowIfDisposed();
            if (grp == null)
            {
                throw new ArgumentNullException("Group");
            }
            //Remove FK
            var dbSetPermissionGroup = this.Context.Set<PermissionGroup>();
            var lstMap = dbSetPermissionGroup.Where(x => x.GroupId == grp.Id);
            dbSetPermissionGroup.RemoveRange(lstMap);

            //Remove FK
            var dbSetUserGroup = this.Context.Set<UserGroup>();
            var lstMapUser = dbSetUserGroup.Where(x => x.GroupId == grp.Id);
            dbSetUserGroup.RemoveRange(lstMapUser);

            this.DbEntitySet.Remove(grp);
        }

        public Task<Group> FindByIdAsync(string id)
        {
            this.ThrowIfDisposed();
            return this.DbEntitySet.FindAsync(new object[] { id });
        }
        public Group FindById(string id)
        {
            this.ThrowIfDisposed();
            return this.DbEntitySet.Find(new object[] { id });
        }
        public virtual void Detach(Group per)
        {
            this.ThrowIfDisposed();
            if (per == null)
            {
                throw new ArgumentNullException("Group");
            }
            this.Context.Entry<Group>(per).State = EntityState.Detached;
        }
        public virtual async Task UpdateAsync(Group per)
        {
            this.ThrowIfDisposed();
            if (per == null)
            {
                throw new ArgumentNullException("Group");
            }
            if (per != null)
            {
                this.Context.Entry<Group>(per).State = EntityState.Modified;
            }
            await this.Context.SaveChangesAsync();
        }
        public virtual void Update(Group per)
        {
            this.ThrowIfDisposed();
            if (per == null)
            {
                throw new ArgumentNullException("Group");
            }
            if (per != null)
            {
                this.Context.Entry<Group>(per).State = EntityState.Modified;
            }
            this.Context.SaveChanges();
        }

    }
}