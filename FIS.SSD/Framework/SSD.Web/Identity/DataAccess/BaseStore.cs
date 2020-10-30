using System;
using System.Data.Entity;

namespace SSD.Web.Identity.DataAccess
{
    public class BaseStore : IDisposable
    {
        private bool _disposed;
        public DbContext Context
        {
            get;
            private set;
        }
        public BaseStore(DbContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            this.Context = context;
        }

        public bool DisposeContext
        {
            get;
            set;
        }


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
            if (this.DisposeContext && disposing && this.Context != null)
            {
                this.Context.Dispose();
                this.Context = null;
            }
            this._disposed = true;
        }
    }
}