using System.Linq;
using System.Security.Claims;
using System.Collections.Generic;

namespace SSD.SSO.Extensions
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public static class UserExtensions
    {
        public static bool IsRole(this System.Security.Principal.IPrincipal user, string role)
        {
            var claims = user as ClaimsPrincipal;
            if (claims != null && claims.Claims != null)
            {
                var cl = from c in claims.Claims
                         where c.Type == IdentityServer3.Core.Constants.ClaimTypes.Role && c.Value == role
                         select c;
                return cl.Count() > 0;
            }
            return false;
        }
        public static List<string> ListRole(this System.Security.Principal.IPrincipal user)
        {
            var claims = user as ClaimsPrincipal; 
            if (claims != null && claims.Claims != null)
            {
                var cl = from c in claims.Claims
                         where c.Type == IdentityServer3.Core.Constants.ClaimTypes.Role
                         select c.Value;
                return cl.ToList();
            }
            return new List<string>();
        }
        public static string GetUserName(this System.Security.Principal.IPrincipal user)
        {
            var claims = user as ClaimsPrincipal;
            if (claims != null && claims.Claims != null)
            {
                var cl = from c in claims.Claims
                         where c.Type == IdentityServer3.Core.Constants.ClaimTypes.PreferredUserName
                         select c.Value;
                return cl.Count() > 0 ? cl.First() : string.Empty;
            }
            return string.Empty;
        }
        public static string GetUserId(this System.Security.Principal.IPrincipal user)
        {
            var claims = user as ClaimsPrincipal;
            if (claims != null && claims.Claims != null)
            {
                var cl = from c in claims.Claims
                         where c.Type == IdentityServer3.Core.Constants.ClaimTypes.Subject
                         select c.Value;
                return cl.Count() > 0 ? cl.First() : string.Empty;
            }
            return string.Empty;
        }
        public static string GetEmail(this System.Security.Principal.IPrincipal user)
        {
            var claims = user as ClaimsPrincipal;
            if (claims != null && claims.Claims != null)
            {
                var cl = from c in claims.Claims
                         where c.Type == IdentityServer3.Core.Constants.ClaimTypes.Email
                         select c.Value;
                return cl.Count() > 0 ? cl.First() : string.Empty;
            }
            return string.Empty;
        }
        public static List<string> GetPhoneNumber(this System.Security.Principal.IPrincipal user)
        {
            var claims = user as ClaimsPrincipal;
            if (claims != null && claims.Claims != null)
            {
                var cl = from c in claims.Claims
                         where c.Type == IdentityServer3.Core.Constants.ClaimTypes.PhoneNumber
                         select c.Value;
                return cl.ToList();
            }
            return new List<string>();
        }
    }
}