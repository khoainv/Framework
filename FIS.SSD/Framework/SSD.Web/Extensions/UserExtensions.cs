using System.Linq;
using System.Security.Claims;
using System.Collections.Generic;
using SSD.Framework;
using Microsoft.AspNet.Identity.Owin;
using System.Web;
using SSD.Web.Identity;

namespace SSD.Web.Extensions
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public static class UserExtensions
    {
        public static bool IsGroup<T>(this System.Security.Principal.IPrincipal user, string grpName)
        {
            var grpManager = HttpContext.Current.GetOwinContext().Get<T>() as GroupManagerBase;

            if (grpManager != null && user!=null && !string.IsNullOrWhiteSpace(grpName))
            {
                string userName = user.Identity.Name;
                if (string.IsNullOrWhiteSpace(userName))
                    userName = user.GetUserName();
                if (userName != null)
                {
                    return grpManager.IsGroupCache(userName, grpName); ;
                }
            }
            return false;
        }
        public static bool IsRole(this System.Security.Principal.IPrincipal user, string role)
        {
            var claims = user as ClaimsPrincipal;
            if (claims != null && claims.Claims != null)
            {
                var cl = from c in claims.Claims
                         where c.Type == UGConstants.ClaimTypes.Role && c.Value == role
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
                         where c.Type == UGConstants.ClaimTypes.Role
                         select c.Value;
                return cl.ToList();
            }
            return new List<string>();
        }
        public static string GetUserName(this System.Security.Principal.IPrincipal user)
        {
            if (user.Identity != null && !string.IsNullOrWhiteSpace(user.Identity.Name))
                return user.Identity.Name;
            var claims = user as ClaimsPrincipal;
            if (claims != null && claims.Claims != null)
            {
                var cl = from c in claims.Claims
                         where c.Type == UGConstants.ClaimTypes.PreferredUserName
                         select c.Value;
                return cl.Count() > 0 ? cl.First() : string.Empty;
            }
            return string.Empty;
        }
        public static string GetUserName(this ClaimsIdentity claims)
        {
            if (claims != null && claims.Claims != null)
            {
                var cl = from c in claims.Claims
                         where c.Type == UGConstants.ClaimTypes.PreferredUserName
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
                         where c.Type == UGConstants.ClaimTypes.Subject
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
                         where c.Type == UGConstants.ClaimTypes.Email
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
                         where c.Type == UGConstants.ClaimTypes.PhoneNumber
                         select c.Value;
                return cl.ToList();
            }
            return new List<string>();
        }
    }
}