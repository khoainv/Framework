using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using SSD.Web.Models;
using SSD.Framework.Extensions;

namespace SSD.Web.Identity
{
    public interface ICacheManager
    {
        void CleanCache();
        Task UpdateCacheAsync();
    }
    public class BaseManager : IDisposable
    {
        public void ClearAllCacheFrameworkByAssemblyName(string assemblyNameStart = "SSD.Web,")
        {
            var typeInterface = typeof(ICacheManager);
            var types = typeInterface.GetUGSubclassesOf(assemblyNameStart);
            foreach (Type type in types)
            {
                if (type.GetInterfaces().Length > 0)
                    if (type.GetInterfaces().Contains(typeof(ICacheManager)))
                    {
                        try
                        {
                            ICacheManager biz = (ICacheManager)Activator.CreateInstance(type);
                            biz.CleanCache();
                        }catch
                        { }
                    }
            }
        }
        public void ClearAllCacheFrameworkByCurrentAssembly()
        {
            var typeInterface = typeof(ICacheManager);
            var types = this.GetType().Assembly.GetTypes().Where(p => !p.IsAbstract && typeInterface.IsAssignableFrom(p));
            foreach (Type type in types)
            {
                if (type.GetInterfaces().Length > 0)
                    if (type.GetInterfaces().Contains(typeof(ICacheManager)))
                    {
                        try
                        {
                            ICacheManager biz = (ICacheManager)Activator.CreateInstance(type);
                            biz.CleanCache();
                        }
                        catch
                        { }
                    }
            }
        }
        //Get All Permission In Group
        protected List<string> GetParentGroupIdById(string parentId, List<Group> lst)
        {
            if (lst.Count == 0)
                return new List<string>();

            var grp = from g in lst where g.Id == parentId select g;

            List<string> lstId = new List<string>();

            if (grp.Count() > 0)
            {
                lstId.Add(parentId);
                GetParentDequy(grp.First(), lstId, lst);
            }

            return lstId;
        }
        private void GetParentDequy(Group grp, List<string> lstId, List<Group> lst)
        {
            if (string.IsNullOrWhiteSpace(grp.ParentId))
                return;

            var parent = from g in lst where g.Id == grp.ParentId select g;

            if (parent.Count() > 0)
            {
                var p = parent.First();
                lstId.Add(p.Id);
                GetParentDequy(p, lstId, lst);
            }
        }

        private bool _disposed;
       
        //public bool DisposeContext
        //{
        //    get;
        //    set;
        //}


        protected void ThrowIfDisposed()
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
            //if (this.DisposeContext && disposing && this.Context != null)
            //{
            //    this.Context.Dispose();
            //    this.Context = null;
            //}
            this._disposed = true;
        }
    }
}