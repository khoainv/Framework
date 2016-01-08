using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using SSD.Framework.Exceptions;
using SSD.Framework.Helpers;
using SSD.Framework.Security;
using SSD.Framework.SSOClient;

namespace SSD.Framework
{
    public partial class UsersAgent : BaseAgent<UsersAgent>
    {
        //api/users/Authenticate
        private readonly string ResourceAuthe = "api/users/Authenticate";
        public async Task<LoginResult> Authenticate(UserAuthen user)
        {
            var data = SetDataJson(user);
            var response = new HttpResponseMessage();
            //try
            //{
                response = await HTTPClient.PostAsync(ResourceAuthe, new StringContent(data, Encoding.UTF8, UGConstants.HTTPHeaders.ContentTypeJson));
            //}
            //catch (HttpRequestException ex)
            //{
            //    throw new HTTPException(System.Net.HttpStatusCode.Unauthorized, ex.Message);
            //}
            if (response.IsSuccessStatusCode)
            {
                string contentStr = await response.Content.ReadAsStringAsync();

                var responseContent = GetDataJson<LoginResult>(contentStr);

                if (!responseContent.Successeded)
                    throw new BusinessException(ErrorCode.Permission, responseContent.Message);

                return responseContent;
            }
            else
            {
                throw new UnknownException(response.StatusCode.ToString());
            }
        }

        private readonly string ResourceAutheSSO = "/core/connect/token";
        
        public async Task<LoginResult> SSOAuthenticate(UserAuthen user)
        {
            if (string.IsNullOrWhiteSpace(Settings.AuthorityBase)
                || string.IsNullOrWhiteSpace(Settings.SSOClientId)
                || string.IsNullOrWhiteSpace(Settings.SSOClientSecret))
                throw new BusinessException(ErrorCode.InitSettings, "Please setting UGStartup AuthorityBase");


            string tokenEndPoint = Settings.AuthorityBase + ResourceAutheSSO;
            var uri = new Uri(tokenEndPoint);
            var client = new OAuth2Client(uri, Settings.SSOClientId, Settings.SSOClientSecret);

            var result = await client.RequestResourceOwnerPasswordAsync(user.UserName, user.Password, "openid profile");

            if(!string.IsNullOrWhiteSpace(result.AccessToken))
            {
                LoginResult loginResult = new LoginResult();
                loginResult.UGToken = result.AccessToken;
                loginResult.Successeded = true;
                loginResult.Message = "Successfull";

                RequestBase request = new RequestBase("api/users/GetProfile", loginResult.UGToken, user);
                loginResult.ProfileJson = await ExecuteAsync<string>(request);
                return loginResult;
            }
            else
            {
                throw new UnknownException(result.Error);
            }
        }
    }
}
