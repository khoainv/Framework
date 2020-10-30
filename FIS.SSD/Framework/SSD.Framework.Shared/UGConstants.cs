namespace SSD.Framework
{
    public enum TargetPlatform
    {
        Other,
        iOS,
        Android,
        WinPhone
    }
    public partial class UGConstants
    {
        public class HTTPHeaders
        {
            //HTTP Header
            public const string TOKEN_NAME = "UGToken";
            public const string IOT_CLIENT_ID = "IoTClientId";
            public const string IOT_CLIENT_SECRET = "IoTClientSecret";
            public const string CONTENT_ENCODING = "Content-Encoding";
            public const string CONTENT_TYPE = "Content-Type";
            public const string ContentTypeJson = "application/json";
            public const string ContentEncodingDeflate = "deflate";
        }

        public static class Security
        {
            public readonly static string MsgValidPublicKey = "PublicKey không tồn tại trong hệ thống, đề nghị kiểm tra lại!";
            public readonly static string MsgValidSignature = "Dữ liệu đã bị thay đổi trên đường truyền, đề nghị kiểm tra lại!";
            public readonly static string MsgValidStorePermission = "Bạn không đủ quyền truy cập dữ liệu cửa hàng, yêu cầu đăng nhập và cấu hình lại cửa hàng";
            public readonly static string MsgValidAcctionPermission = "Bạn không có quyền vào chức năng: {0} với 'Mã: {1}'";
            public readonly static string MsgMissingUGToken = "Bạn không có quyền truy cập chức năng này. Bạn bị lỗi hoặc thiếu UGToken để truy cập";
            public readonly static string MsgMissingUserName = "Bạn không có quyền truy cập chức năng này. Bạn bị thiếu PreferredUserName trên Header để truy cập";
            public readonly static string MsgValidLogin = "Bạn chưa login vào hệ thống, đề nghị login và dùng token";


            public readonly static bool IS_VERIFY_CRL = false;
            public readonly static bool IS_ENCRYPT = false;
            public readonly static bool IS_VERIFY_SIGNATURE = false;

            public readonly static bool USING_SIGNATURE = false;
        }
        public static class ClaimTypes
        {
            public const string Email = "email";
            public const string Id = "id";
            public const string Name = "name";
            public const string NickName = "nickname";
            public const string GivenName = "given_name";
            //
            // Summary:
            //     End-User's preferred telephone number. E.164 (https://www.itu.int/rec/T-REC-E.164/e)
            //     is RECOMMENDED as the format of this Claim, for example, +1 (425) 555-1212 or
            //     +56 (2) 687 2400. If the phone number contains an extension, it is RECOMMENDED
            //     that the extension be represented using the RFC 3966 [RFC3966] extension syntax,
            //     for example, +1 (604) 555-1234;ext=5678.
            public const string PhoneNumber = "phone_number";
            //
            // Summary:
            //     Shorthand name by which the End-User wishes to be referred to at the RP, such
            //     as janedoe or j.doe. This value MAY be any valid JSON string including special
            //     characters such as @, /, or whitespace. The relying party MUST NOT rely upon
            //     this value being unique
            //
            // Remarks:
            //     The RP MUST NOT rely upon this value being unique, as discussed in http://openid.net/specs/openid-connect-basic-1_0-32.html#ClaimStability
            public const string PreferredUserName = "preferred_username";
            //
            // Summary:
            //     URL of the End-User's profile page. The contents of this Web page SHOULD be about
            //     the End-User.
            public const string Profile = "profile";
            public const string Role = "role";
            //
            // Summary:
            //     OpenID Connect requests MUST contain the "openid" scope value. If the openid
            //     scope value is not present, the behavior is entirely unspecified. Other scope
            //     values MAY be present. Scope values used that are not understood by an implementation
            //     SHOULD be ignored.
            public const string Scope = "scope";
            public const string Secret = "secret";
            //
            // Summary:
            //     Unique Identifier for the End-User at the Issuer.
            public const string Subject = "sub";
        }
        public static class Authentication
        {
            public const string SigninId = "signinid";
            public const string SignoutId = "id";
            public const string KatanaAuthenticationType = "katanaAuthenticationType";
            public const string PartialLoginRememberMe = "idsvr:rememberme";
        }
    }
}
