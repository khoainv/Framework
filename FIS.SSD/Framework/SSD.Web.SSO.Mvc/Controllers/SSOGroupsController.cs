using SSD.Web.Mvc.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Data.Entity;
using System;
using SSD.Web.Identity;
using SSD.Web.Models;
using SSD.Web.SSO;
using SSD.Web.Mvc.Controllers;

namespace SSD.Web.SSO.Mvc.Controllers
{
    [SSOAutshorize(Groups = "Admin")]
    public class SSOGroupsController : GroupsControllerBase
    {
        protected override void SetManager()
        {
            _grpManager = HttpContext.GetOwinContext().Get<SSOGroupManager>();
            _perManager = HttpContext.GetOwinContext().Get<SSOPermissionManager>();
        }
        public SSOGroupsController() : base(null,null)
        {
        }
        public SSOGroupsController(SSOGroupManager grpManager, SSOPermissionManager perManager) : base(grpManager, perManager)//, grpManager)
        {
        }
    }
}
