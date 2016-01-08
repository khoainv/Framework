using Microsoft.Owin;
using System;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using SSD.Framework;
using SSD.Web.Extensions;
using SSD.Web.Identity;
using Microsoft.AspNet.Identity.Owin;
using SSD.Web.Mvc.Controllers;
using System.Collections.Generic;
using SSD.Web.SSO.Mvc.Controllers;

namespace SSD.SSO.Sample.Controllers
{
    //public class NotExtBaseController : SSOBaseController
    //{
    //    public SSOWorkContext WorkContext { get; set; }
    //    public override void InitWorkContext(System.Web.Mvc.Filters.AuthenticationContext filterContext)
    //    {
    //        WorkContext = new SSOWorkContext(filterContext);
    //        //Init WorkContext
    //        SetWorkContext(WorkContext);
    //    }
    //}
    public class SampleBaseController : SSOBaseController<SampleWorkContext>
    {
        public override void InitWorkContext(System.Web.Mvc.Filters.AuthenticationContext filterContext)
        {
            WorkContext = new SampleWorkContext(filterContext);

            var user = filterContext.HttpContext.User;
            if (user.Identity.IsAuthenticated && !WorkContext.IsInitedUser(user.GetUserName()))
            {
                Profile p = new Profile();
                p.Stores = new List<int>() { 1, 2 };
                p.StoreView = 1;
                WorkContext.SetProfile(p);
            }
            //Init WorkContext
            base.InitWorkContext(filterContext);
        }
    }
    public class Profile
    {
        public List<int> Stores { get; set; }
        public int StoreView { get; set; }
    }
    public class SampleWorkContext : SSOWorkContext
    {
        public Profile Profile
        {
            get
            {
                return GetProfile<Profile>();
            }
        }
        public SampleWorkContext(System.Web.Mvc.Filters.AuthenticationContext filterContext) : base(filterContext)
        {
        }
    }
}