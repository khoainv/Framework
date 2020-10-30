using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Text;
using SSD.Mobile.Entities;
using SSD.Mobile.Agents;
using System.Collections.Generic;
using SSD.Framework.Extensions;
using SSD.Framework.Helpers;
using SSD.Framework.Security;

namespace SSD.Mobile.BusinessLogics
{
    public class BaseBiz<T>
        where T : class, new()
    {
        static object obj = new object();
        private static T instance;
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (obj)
                    {
                        instance = new T();
                    }
                }
                return instance;
            }
        }
    }
	public interface IUserBiz
	{
        Task<LoginResult> Authen(UserAuthen user);
        void ResetToken();
	}

	public class UserBiz :BaseBiz<UserBiz>, IUserBiz
	{
        public string UGToken
        {
            get
            {
                return Settings.AccessToken;
            }
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    Settings.AccessToken = value;
                }
            }
        }
        public UserAuthen User
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Settings.UserName))
                    return null;
                var user = new UserAuthen(Settings.UserName, Settings.Password);
                return user;
            }
            set
            {
                if (value!=null)
                {
                    Settings.UserName=value.UserName;
                    Settings.Password = value.Password;
                }
            }
        }
        public string ProfileJson
        {
            get
            {
                return Settings.ProfileJson;
            }
            set
            {
                Settings.ProfileJson = value;
            }
        }
        public async Task<LoginResult> Authen(UserAuthen user)
        {
            User = user;
            LoginResult status;
            if (Settings.IsUsingSSO)
            {
                status = await AgentRepository.Instance.Users.SSOAuthenticate(user);
            }
            else
            {
                status = await AgentRepository.Instance.Users.Authenticate(user);
            }
            UGToken = status.UGToken;
            ProfileJson = status.ProfileJson;
            return status;
        }
        public void ResetToken()
        {
            Settings.AccessToken = string.Empty;
        }
    }
}

