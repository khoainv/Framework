using IdentityServer3.Core.Extensions;
using IdentityServer3.Core.Models;
using IdentityServer3.Core.Services;
using IdentityServer3.Core.Validation;
using System.Threading.Tasks;
using IdentityServer3.Core;

namespace ADIdentityServer.IdSvr
{
    class CustomGrantValidator : ICustomGrantValidator
    {
        private readonly IUserService _users;

        public CustomGrantValidator(IUserService users)
        {
            _users = users;
        }

        public async Task<CustomGrantValidationResult> ValidateAsync(ValidatedTokenRequest request)
        {
            var legacyAccountStoreType = request.Raw.Get(Constants.ClaimTypes.Email);
            var id = request.Raw.Get("legacy_id");
            var secret = request.Raw.Get("legacy_secret");

            if (string.IsNullOrWhiteSpace(legacyAccountStoreType) ||
                string.IsNullOrWhiteSpace(id) ||
                string.IsNullOrWhiteSpace(secret))
            {
                return null;
            }

            var message = new SignInMessage { Tenant = legacyAccountStoreType };
            var context = new LocalAuthenticationContext
            {
                UserName = id,
                Password = secret,
                SignInMessage = message
            };
            await _users.AuthenticateLocalAsync(context);

            var result = context.AuthenticateResult;
            if (result.IsError)
            {
                return new CustomGrantValidationResult("Authentication failed.");
            }

            return new CustomGrantValidationResult(
                result.User.GetSubjectId(),
                "custom",
                result.User.Claims);
        }

        public string GrantType
        {
            get { return Constants.ClaimTypes.Email; }
        }
    }
}