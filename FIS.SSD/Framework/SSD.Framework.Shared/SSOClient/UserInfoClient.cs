using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace SSD.Framework.SSOClient
{
    public class UserInfoClient
    {
        private readonly HttpClient _client;

        public UserInfoClient(Uri endpoint, string token)
            : this(endpoint, token, new HttpClientHandler())
        { }

        public UserInfoClient(Uri endpoint, string token, HttpClientHandler innerHttpClientHandler)
        {
            if (endpoint == null)
                throw new ArgumentNullException("endpoint");

            if (string.IsNullOrEmpty(token))
                throw new ArgumentNullException("token");

            if (innerHttpClientHandler == null)
                throw new ArgumentNullException("innerHttpClientHandler");

            _client = new HttpClient(innerHttpClientHandler)
            {
                BaseAddress = endpoint
            };

            _client.SetBearerToken(token);
        }

        public TimeSpan Timeout
        {
            set
            {
                _client.Timeout = value;
            }
        }

        public async Task<UserInfoResponse> GetAsync()
        {
            var response = await _client.GetAsync("");

            if (response.StatusCode != HttpStatusCode.OK)
                return new UserInfoResponse(response.StatusCode, response.ReasonPhrase);

            var content = await response.Content.ReadAsStringAsync();
            return new UserInfoResponse(content);
        }
    }
}
