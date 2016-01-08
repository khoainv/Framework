/*
Product Name		: UG	
Product Version 	: 1.0                                               	                     
Product Owner   	: UG-Trad
Developed By    	: 7i.com.vn

Description:  Sync7i is project intergarte client SmartNet and web 7i.com.vn

File Name	   		: AppConfigBiz.cs 				   	     
File Description 	: AppConfigBiz provides business processing on tblAppConfig data

Copyright(C) 2013 by UG-Trad. All Rights Reserved 	
*/

using System.Linq;
using System.Data;
using System.Collections.Generic;
using SSD.Framework.Extensions;
using SSD.Framework;
using SSD.SSO.Identity;
using System;
using System.Reflection;
using System.Configuration;
using SSD.Framework.Collections;
using SSD.Framework.Email;
using SSD.Framework.Cryptography;

namespace SSD.SSO.Config
{
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
	public partial class AppConfigBiz: Singleton<AppConfigBiz>
    {
        private IAppConfigStore _configStore;
        private ApplicationDbContext _db;
        public readonly static string AppConfigStr = ConfigurationManager.AppSettings["AppConfig"];

        public AppConfigBiz()
        {
            _db = new ApplicationDbContext();
            Init();
        }
        public AppConfigBiz(ApplicationDbContext db)
        {
            this._db = db;
            Init();
        }
        private void Init()
        {
            if (string.IsNullOrWhiteSpace(AppConfigStr) || AppConfigStr.Split(new char[] { ',' }).Count() < 2)
            {
                //Becouse run in Startup owin => not init HTTPContext
                _configStore = new AppConfigStore(_db);
            }
            else
            {
                var arr = AppConfigStr.Split(new char[] { ',' });
                Assembly assembly = Assembly.Load(arr[0]);
                Type type = assembly.GetType(arr[1]);
                _configStore = Activator.CreateInstance(type) as IAppConfigStore;
                _configStore.SetDbContext(_db);
            }
        }
        #region Decrypt and Encrypt
        #region Cache
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
            var lst = _configStore.AppConfigs;
            foreach (var obj in lst)
            {
                if (obj.IsEncryption)
                    obj.ConfigData = RSAEngine.Password.DecryptPassword(obj.ConfigData);
            }
            return lst;
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
        public int InsertOrUpdateWithEncrypt(AppConfig obj)
        {
            if ((obj.ID != 0))
            {
                UpdateWithEncrypt(obj);
                return 0;
            }
            else return InsertWithEncrypt(obj);
        }

        public void DeleteByID(int id)
        {
            _configStore.DeleteConfig(new AppConfig() { ID = id });
        }
        
        public int InsertWithEncrypt(AppConfig obj)
		{
            if (obj.IsEncryption)
                obj.ConfigData = RSAEngine.Password.EncryptPassword(obj.ConfigData);
            CleanCacheWithDecrypted();
            return _configStore.InsertConfig(obj);
		}
		
        public void UpdateWithEncrypt(AppConfig obj)
		{
            if (obj.IsEncryption)
                obj.ConfigData = RSAEngine.Password.EncryptPassword(obj.ConfigData);
            _configStore.UpdateConfig(obj);
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
        public int SaveAppSettings(string key, string values, bool isEncryption=false)
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
            return sql.Count()>0;
        }
        
        public AppConfig ReadObjectByIDWithDecrypted(int iD)
        {
            AppConfig obj = _configStore.ReadObjectByIDConfig(iD);
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

                    foreach(AppConfig app in query)
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
    }
}
