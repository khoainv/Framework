using System.Linq;
using System.Threading.Tasks;
using IdentityServer3.AspNetIdentity;
using IdentityServer3.Core.Configuration;
using IdentityServer3.Core.Services;
using IdentityServer3.Core.Models;
using SSD.SSO.Identity;
using System;
using IdentityServer3.Core.Extensions;
using System.Collections.Generic;

namespace SSD.SSO.IdentityServer
{
    public static class UserServiceExtensions
    {
        public static void ConfigureUserService(this IdentityServerServiceFactory factory)
        {
            factory.UserService = new Registration<IUserService, UserService>();
            //factory.ClaimsProvider = new Registration<IClaimsProvider, AscendClaimsProvider>();
            factory.Register(new Registration<ApplicationUserManager>());

            //factory.Register(new Registration<ApplicationDbContext>(new ApplicationDbContext()));
            //DBContext not Caching, Fix bug DB Changed => IdSrv not change
            factory.Register(new Registration<ApplicationDbContext>(resolver => new ApplicationDbContext()));
        }
    }
    
    public class UserService : AspNetIdentityUserService<ApplicationUser, string>
    {
        public UserService(ApplicationUserManager userMgr)
            : base(userMgr)
        {
        }

        protected override async Task<AuthenticateResult> PostAuthenticateLocalAsync(ApplicationUser user, SignInMessage message)
        {
            if (base.userManager.SupportsUserTwoFactor)
            {
                var id = user.Id;

                if (await userManager.GetTwoFactorEnabledAsync(id))
                {
                    var code = await this.userManager.GenerateTwoFactorTokenAsync(id, "Email Code");
                    var result = await userManager.NotifyTwoFactorTokenAsync(id, "Email Code", code);
                    if (!result.Succeeded)
                    {
                        return new IdentityServer3.Core.Models.AuthenticateResult(result.Errors.First());
                    }

                    var name = await GetDisplayNameForAccountAsync(id);
                    return new IdentityServer3.Core.Models.AuthenticateResult("~/2fa", id, name);
                }
            }

            return null;
        }
        //Fix error requestedClaimTypes TODO after fix better <=> FIXED with get userinfo on client
        //public override async Task GetProfileDataAsync(ProfileDataRequestContext ctx)
        //{
        //    var subject = ctx.Subject;

        //    if (subject == null) throw new ArgumentNullException("subject");

        //    string key = ConvertSubjectToKey(subject.GetSubjectId());
        //    var acct = await userManager.FindByIdAsync(key);
        //    if (acct == null)
        //    {
        //        throw new ArgumentException("Invalid subject identifier");
        //    }
        //    var claims = await GetClaimsFromAccount(acct);
        //    ctx.IssuedClaims = claims;
        //}
        //Fix get username AD
        protected override Task<ApplicationUser> InstantiateNewUserFromExternalProviderAsync(string provider, string providerId, IEnumerable<System.Security.Claims.Claim> claims)
        {
            var user = new ApplicationUser() { UserName = Guid.NewGuid().ToString("N") };

            if (claims != null)
            {
                //Set UserName
                var cl = from c in claims
                         where c.Type == IdentityServer3.Core.Constants.ClaimTypes.PreferredUserName
                         select c.Value;

                if(cl.Count()>0)
                {
                    user.UserName = cl.First();
                }
                
                //Set Email
                cl = from c in claims
                     where c.Type == IdentityServer3.Core.Constants.ClaimTypes.Email
                         select c.Value;
                if (cl.Count() > 0)
                {
                    user.Email = cl.First();
                }
            }
            return Task.FromResult(user);
        }
    }
}
