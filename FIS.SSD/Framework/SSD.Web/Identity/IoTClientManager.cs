using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using SSD.Web.Identity.DataAccess;
using SSD.Web.Models;

namespace SSD.Web.Identity
{
    public class IoTClientManager : IoTClientManagerBase
    {
        public IoTClientManager() : base(HttpContext.Current.GetOwinContext().Get<IdentityDbContext>())
        {
        }
        public IoTClientManager(IdentityDbContext context) : base(context)
        {
        }
        public static IoTClientManager Create(IdentityFactoryOptions<IoTClientManager> options, IOwinContext context)
        {
            return new IoTClientManager(context.Get<IdentityDbContext>());
        }
    }
    public abstract class IoTClientManagerBase : BaseManager, ICacheManager
    {
        private IoTClientStore _store;
        private DbContext _db;

        public IoTClientManagerBase(DbContext context)
        {
            _db = context;
            _store = new IoTClientStore(_db);
        }
        #region Cache
        public string NewSecret
        {
            get
            {
                string str = Guid.NewGuid().ToString();
                using (var sha = SHA256.Create())
                {
                    var bytes = Encoding.UTF8.GetBytes(str);
                    var hash = sha.ComputeHash(bytes);

                    str = Convert.ToBase64String(hash);
                }
                return str.Substring(0, 25);
            }
        }
        public bool IsValidClient(string clientId, string clientSecret)
        {
            if (string.IsNullOrWhiteSpace(clientId) || string.IsNullOrWhiteSpace(clientSecret))
                return false;
            return CacheLstAllIoTClient.Where(x => x.ClientId.ToLower() == clientId.ToLower() && x.ClientSecret.ToLower() == clientSecret.ToLower()).Count() > 0;
        }
        static object obj = new object();
        static List<IoTClient> lstIoTClient;
        public List<IoTClient> CacheLstAllIoTClient
        {
            get
            {
                if (lstIoTClient == null)
                {
                    lock (obj)
                    {
                        if (lstIoTClient == null)
                        {
                            lstIoTClient = IoTClients.Where(x=>x.Enable && !x.Deleted).ToList();
                        }
                    }
                }
                return lstIoTClient;
            }
        }
        public async Task UpdateCacheAsync()
        {
            lstIoTClient = await IoTClients.ToListAsync();
        }
        public void CleanCache()
        {
            lstIoTClient = null;
        }
      
        #endregion
        public DbContext Context
        {
            get
            {
                return _db;
            }
        }
        public IQueryable<IoTClient> IoTClients
        {
            get
            {
                return _store.DbEntitySet;
            }
        }
     
        #region CRUD
        public async Task<IdentityResult> CreateAsync(IoTClient per)
        {
            await _store.CreateAsync(per);
            return IdentityResult.Success;
        }
        public IdentityResult Create(IoTClient per)
        {
            _store.Create(per);
            return IdentityResult.Success;
        }
        
        public async Task<IdentityResult> DeleteAsync(string grpId)
        {
            var grp = await this.FindByIdAsync(grpId);
            if (grp == null)
            {
                throw new ArgumentNullException("IdentityClient");
            }
            await _store.DeleteAsync(grp);
            return IdentityResult.Success;
        }
        public IdentityResult Delete(string grpId)
        {
            var grp = this.FindById(grpId);
            if(grp == null)
            {
                throw new ArgumentNullException("IdentityClient");
            }
            _store.Delete(grp);
            return IdentityResult.Success;
        }
        public IdentityResult Detach(IoTClient per)
        {
            _store.Detach(per);
            return IdentityResult.Success;
        }
        public async Task<IdentityResult> UpdateAsync(IoTClient per)
        {
            await _store.UpdateAsync(per);
            return IdentityResult.Success;
        }
        public IdentityResult Update(IoTClient per)
        {
            _store.Update(per);
            return IdentityResult.Success;
        }

        public async Task<IoTClient> FindByIdAsync(string id)
        {
            return await _store.FindByIdAsync(id);
        }
        public IoTClient FindById(string id)
        {
            return _store.FindById(id);
        }

        #endregion
    }
}