using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using SSD.Framework;
using SSD.Framework.Collections;
using SSD.Framework.Cryptography;
using SSD.Framework.Email;
using SSD.Framework.Extensions;
using SSD.Web.Identity.DataAccess;
using SSD.Web.Models;

namespace SSD.Web.Identity
{
    public class AppConfigManager : AppConfigManagerBase
    {
        public AppConfigManager() : base(HttpContext.Current.GetOwinContext().Get<IdentityDbContext>())
        {
        }
        public AppConfigManager(IdentityDbContext context) : base(context)
        {
        }
        public static AppConfigManager Create(IdentityFactoryOptions<AppConfigManager> options, IOwinContext context)
        {
            return new AppConfigManager(context.Get<IdentityDbContext>());
        }
    }
    public abstract class AppConfigManagerBase : BaseManager, ICacheManager
    {

        #region Decrypt and Encrypt
        #region Cache
        public void CleanCache()
        {
            CleanCacheWithDecrypted();
        }

        public Task UpdateCacheAsync()
        {
            return Task.Run(() =>
            {
                var lst = CacheAppConfigWithDecrypted;
            });
        }
        static SortableBindingList<AppConfig> appConfigWithDecrypted;
        public SortableBindingList<AppConfig> CacheAppConfigWithDecrypted
        {
            get
            {
                if (appConfigWithDecrypted == null)
                    appConfigWithDecrypted = ListAllObjectsWithDecrypted();
                return appConfigWithDecrypted;
            }
        }

        private SortableBindingList<AppConfig> ListAllObjectsWithDecrypted()
        {
            var lst = AppConfigs;
            foreach (var obj in lst)
            {
                if (obj.IsEncryption)
                    obj.ConfigData = RSAEngine.Password.DecryptPassword(obj.ConfigData);
            }
            return lst.ToSortableBindingList();
        }
        public void CleanCacheWithDecrypted()
        {
            appConfigWithDecrypted = null;
            extAuthentication = null;
            appSettings = null;
            smsServerData = null;
            smtpData = null;
        }
        #endregion
        public bool InsertOrUpdateWithEncrypt(AppConfig obj)
        {
            if ((obj.ID != 0))
            {
                UpdateWithEncrypt(obj);
                return true;
            }
            else return InsertWithEncrypt(obj);
        }

        //public void DeleteByID(int id)
        //{
        //    _configStore.DeleteConfig(new AppConfig() { ID = id });
        //}

        public bool InsertWithEncrypt(AppConfig obj)
        {
            if (obj.IsEncryption)
                obj.ConfigData = RSAEngine.Password.EncryptPassword(obj.ConfigData);
            CleanCacheWithDecrypted();
            return Create(obj).Succeeded;
        }

        public void UpdateWithEncrypt(AppConfig obj)
        {
            if (obj.IsEncryption)
                obj.ConfigData = RSAEngine.Password.EncryptPassword(obj.ConfigData);
            Update(obj);
            CleanCacheWithDecrypted();
        }
        #endregion
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key">Constants in AppConfig Entity</param>
        /// <param name="values"></param>
        /// <param name="isEncryption"></param>
        /// <returns></returns>
        public bool SaveAppSettings(string key, string values, bool isEncryption = false)
        {
            var query = from au in CacheAppConfigWithDecrypted where au.ConfigKey == key select au;

            AppConfig obj;
            if (query.Count() > 0)
            {
                obj = query.First();
                obj.ConfigData = values;
                obj.IsEncryption = isEncryption;
            }
            else
            {
                obj = new AppConfig()
                {
                    ConfigKey = key,
                    ConfigData = values,
                    IsEncryption = isEncryption
                };
            }
            return InsertOrUpdateWithEncrypt(obj);
        }
        public bool IsExistKey(string key)
        {
            var sql = from au in CacheAppConfigWithDecrypted where au.ConfigKey == key select au;
            return sql.Count() > 0;
        }

        public AppConfig ReadObjectByIDWithDecrypted(int iD)
        {
            AppConfig obj = FindById(iD);
            if (obj.IsEncryption)
                obj.ConfigData = RSAEngine.Password.DecryptPassword(obj.ConfigData);
            return obj;
        }

        #region AppSetting
        public IQueryable<AppConfig> GetAllAppSetting
        {
            get
            {
                return (from au in CacheAppConfigWithDecrypted where string.IsNullOrWhiteSpace(au.DataType) select au).AsQueryable();
            }
        }
        static DictionaryWithDefault<string, string> appSettings;
        public DictionaryWithDefault<string, string> AppSettings
        {
            get
            {
                if (appSettings == null)
                {
                    appSettings = new DictionaryWithDefault<string, string>();

                    var query = from au in CacheAppConfigWithDecrypted where string.IsNullOrWhiteSpace(au.DataType) select au;

                    foreach (AppConfig app in query)
                    {
                        appSettings.Add(app.ConfigKey, app.ConfigData);
                    }
                }
                return appSettings;
            }
        }
        #endregion

        #region Authentication
        public IQueryable<AppConfig> GetAllExtAuthenticationOpenIdConnect
        {
            get
            {
                return (from au in CacheAppConfigWithDecrypted where au.DataType == typeof(ExtAuthenticationOpenIdConnect).FullName select au).AsQueryable();
            }
        }
        static DictionaryWithDefault<string, ExtAuthenticationOpenIdConnect> extAuthenticationOpenIdConnect;
        public DictionaryWithDefault<string, ExtAuthenticationOpenIdConnect> ExtAuthenticationOpenIdConnect
        {
            get
            {
                if (extAuthenticationOpenIdConnect == null)
                {
                    extAuthenticationOpenIdConnect = new DictionaryWithDefault<string, ExtAuthenticationOpenIdConnect>();

                    var query = from au in CacheAppConfigWithDecrypted where au.DataType == typeof(ExtAuthenticationOpenIdConnect).FullName select au;

                    foreach (AppConfig app in query)
                    {
                        extAuthenticationOpenIdConnect.Add(app.ConfigKey, app.ConfigData.DeserializeXML<ExtAuthenticationOpenIdConnect>());
                    }
                }
                return extAuthenticationOpenIdConnect;
            }
        }
        public IQueryable<AppConfig> GetAllExtAuthentication
        {
            get
            {
                return (from au in CacheAppConfigWithDecrypted where au.DataType == typeof(ExtAuthentication).FullName select au).AsQueryable();
            }
        }
        static DictionaryWithDefault<string, ExtAuthentication> extAuthentication;
        public DictionaryWithDefault<string, ExtAuthentication> ExtAuthentication
        {
            get
            {
                if (extAuthentication == null)
                {
                    extAuthentication = new DictionaryWithDefault<string, ExtAuthentication>();

                    var query = from au in CacheAppConfigWithDecrypted where au.DataType == typeof(ExtAuthentication).FullName select au;

                    foreach (AppConfig app in query)
                    {
                        extAuthentication.Add(app.ConfigKey, app.ConfigData.DeserializeXML<ExtAuthentication>());
                    }
                }
                return extAuthentication;
            }
        }
        #endregion

        #region SMTP
        public IQueryable<AppConfig> GetAllSmtpData
        {
            get
            {
                return (from au in CacheAppConfigWithDecrypted where au.DataType == typeof(SmtpData).FullName select au).AsQueryable();
            }
        }
        static DictionaryWithDefault<string, SmtpData> smtpData;
        public DictionaryWithDefault<string, SmtpData> SmtpData
        {
            get
            {
                if (smtpData == null)
                {
                    smtpData = new DictionaryWithDefault<string, SmtpData>();

                    var query = from au in CacheAppConfigWithDecrypted where au.DataType == typeof(SmtpData).FullName select au;

                    foreach (AppConfig app in query)
                    {
                        smtpData.Add(app.ConfigKey, app.ConfigData.DeserializeXML<SmtpData>());
                    }
                }
                return smtpData;
            }
        }
        #endregion

        #region SMS
        public IQueryable<AppConfig> GetAllSMSServerData
        {
            get
            {
                return (from au in CacheAppConfigWithDecrypted where au.DataType == typeof(SMSServerData).FullName select au).AsQueryable();
            }
        }
        static DictionaryWithDefault<string, SMSServerData> smsServerData;
        public DictionaryWithDefault<string, SMSServerData> SMSServerData
        {
            get
            {
                if (smsServerData == null)
                {
                    smsServerData = new DictionaryWithDefault<string, SMSServerData>();

                    var query = from au in CacheAppConfigWithDecrypted where au.DataType == typeof(SMSServerData).FullName select au;

                    foreach (AppConfig app in query)
                    {
                        smsServerData.Add(app.ConfigKey, app.ConfigData.DeserializeXML<SMSServerData>());
                    }
                }
                return smsServerData;
            }
        }
        #endregion

        #region Base
        private AppConfigStore _store;
        private DbContext _db;
        public AppConfigManagerBase(DbContext context)
        {
            _db = context;
            _store = new AppConfigStore(_db);
        }
        public DbContext Context
        {
            get
            {
                return _db;
            }
        }
        public IQueryable<AppConfig> AppConfigs
        {
            get
            {
                return _store.DbEntitySet;
            }
        }

        #region CRUD
        public async Task<IdentityResult> CreateAsync(AppConfig per)
        {
            await _store.CreateAsync(per);
            return IdentityResult.Success;
        }
        public IdentityResult Create(AppConfig per)
        {
            _store.Create(per);
            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(int grpId)
        {
            var grp = await this.FindByIdAsync(grpId);
            if (grp == null)
            {
                throw new ArgumentNullException("AppConfig");
            }
            await _store.DeleteAsync(grp);
            return IdentityResult.Success;
        }
        public IdentityResult Delete(int grpId)
        {
            var grp = this.FindById(grpId);
            if (grp == null)
            {
                throw new ArgumentNullException("AppConfig");
            }
            _store.Delete(grp);
            return IdentityResult.Success;
        }

        public async Task<IdentityResult> UpdateAsync(AppConfig per)
        {
            await _store.UpdateAsync(per);
            return IdentityResult.Success;
        }
        public IdentityResult Update(AppConfig per)
        {
            _store.Update(per);
            return IdentityResult.Success;
        }

        public async Task<AppConfig> FindByIdAsync(int id)
        {
            return await _store.FindByIdAsync(id);
        }
        public AppConfig FindById(int id)
        {
            return _store.FindById(id);
        }
        #endregion
        #endregion
    }
    public class DictionaryWithDefault<TKey, TValue> : Dictionary<TKey, TValue>
    {
        TValue _default;
        public TValue DefaultValue
        {
            get { return _default; }
            set { _default = value; }
        }
        public DictionaryWithDefault() : base() { }
        public DictionaryWithDefault(TValue defaultValue)
            : base()
        {
            _default = defaultValue;
        }
        public new TValue this[TKey key]
        {
            get
            {
                TValue t = _default;
                base.TryGetValue(key, out t);
                return t;
            }
            set { base[key] = value; }
        }
    }
}