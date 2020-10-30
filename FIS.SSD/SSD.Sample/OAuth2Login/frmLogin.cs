using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SSD.Framework;
using SSD.Framework.Models;
using SSD.Framework.SSOClient;
using SSD.Framework.Extensions;
using System.IO;
using Ionic.Zlib;

namespace OAuth2Login
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }
        const string APIUri = "http://localhost:1733/api/Sample/Get";
        const string IoTClientId = "919ba91e6589419395c1cb0e3bd5237c";
        const string IoTClientSecret = "grABvcYypOU8a82tucxBdFikH";

        private void btnRemote_Click(object sender, EventArgs e)
        {
            var uri = new Uri(UGConstants.SSO.TokenEndpoint);
            var client = new OAuth2Client(uri, UGConstants.SSOClient.ClientId, UGConstants.SSOClient.ClientSecret);

            var result = client.RequestResourceOwnerPasswordAsync(txtUserName.Text, txtPassword.Text, "openid profile");

            UserAccessInfo.AccessToken = result.Result.AccessToken;
        }

        private async void btnApiAccess_Click(object sender, EventArgs e)
        {
            var claim = await GetClaimsAsync(UserAccessInfo.AccessToken);

            var query = from info in claim where info.Item1 == UGConstants.ClaimTypes.PreferredUserName select info.Item2;
            string userName = string.Empty;
            if (query.Count() > 0)
                userName = query.First();

            var client = new HttpClient();
            client.SetBearerToken(UserAccessInfo.AccessToken);
            client.DefaultRequestHeaders.Add(UGConstants.HTTPHeaders.IOT_CLIENT_ID, IoTClientId);
            client.DefaultRequestHeaders.Add(UGConstants.HTTPHeaders.IOT_CLIENT_SECRET, IoTClientSecret);
            client.DefaultRequestHeaders.Add(UGConstants.ClaimTypes.PreferredUserName, userName);

            BaseMessage msg = new BaseMessage("", "", SSD.Framework.Exceptions.ErrorCode.IsSuccess, "");
            msg.MsgJson = "{\"LocationStoreID\":1,\"ID\":1}";
            StringContent queryString = new StringContent(msg.ToJson());
            var response = await client.PostAsync(APIUri,queryString);

            if (response.IsSuccessStatusCode)
            {
                string contentStr = string.Empty;

                IEnumerable<string> lst;
                if (response.Content.Headers != null
                    && response.Content.Headers.TryGetValues(UGConstants.HTTPHeaders.CONTENT_ENCODING, out lst)
                    && lst.First().ToLower() == "deflate")
                {
                    var bj = await response.Content.ReadAsByteArrayAsync();
                    using (var inStream = new MemoryStream(bj))
                    {
                        using (var bigStreamsss = new DeflateStream(inStream, CompressionMode.Decompress, true))
                        {
                            contentStr = await (new StreamReader(bigStreamsss)).ReadToEndAsync();
                            //using (var bigStreamOut = new MemoryStream())
                            //{
                            //    bigStreamsss.CopyTo(bigStreamOut);
                            //    contentStr = Encoding.UTF8.GetString(bigStreamOut.ToArray(), 0, bigStreamOut.ToArray().Length);
                            //}
                        }

                    }
                }
                else contentStr = await response.Content.ReadAsStringAsync();
                MessageBox.Show(contentStr);
            }
        }
        private async Task<IEnumerable<Tuple<string, string>>> GetClaimsAsync(string token)
        {
            var client = new UserInfoClient(
                new Uri(UGConstants.SSO.UserInfoEndpoint),
                token);

            var response = await client.GetAsync();
            var claim = response.Claims;
            return response.Claims;
        }
    }
}
