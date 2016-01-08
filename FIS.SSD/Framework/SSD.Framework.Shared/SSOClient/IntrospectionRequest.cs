namespace SSD.Framework.SSOClient
{
    public class IntrospectionRequest
    {
        public string Token { get; set; }
        public string TokenTypeHint { get; set; }

        public string ClientId { get; set; }
        public string ClientSecret { get; set; }

        public IntrospectionRequest()
        {
            Token = "";
            ClientId = "";
            ClientSecret = "";
        }
    }
}