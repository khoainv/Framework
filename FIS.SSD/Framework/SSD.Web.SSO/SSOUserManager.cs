using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;
using SSD.Web.Identity;
using SSD.Web.Models;
using SSD.Web.Extensions;

namespace SSD.Web.SSO
{
    public class SSOUserManager : BaseManager,IUserManager
    {
        private SSODbContext _db;
        public SSODbContext Context { get { return _db; } }
        public SSOUserManager() : base()
        {
            _db = HttpContext.Current.GetOwinContext().Get<SSODbContext>();
        }
        public SSOUserManager(SSODbContext context) 
        {
            _db = context;
        }
        public static SSOUserManager Create(IdentityFactoryOptions<SSOUserManager> options, IOwinContext context)
        {
            return new SSOUserManager(context.Get<SSODbContext>());
        }

        public bool IsExist(string userName)
        {
            return Context.Set<UserGroup>().Where(x=>x.UserName == userName).Count()>0;
        }

        public IQueryable<User> UGUsers
        {
            get
            {
                return Context.Set<UserGroup>().Distinct().Select(u => new User() { UserName = u.UserName });
            }
        }
        public User GetBySSOUser(IPrincipal user)
        {
            var u = new User()
            {
                Id = user.GetUserId(),
                UserName = user.GetUserName(),
                Email = user.GetEmail(),
                PhoneNumber = user.GetPhoneNumber().Count > 0 ? user.GetPhoneNumber().First() : string.Empty
            };

            return u;
        }
    }
}