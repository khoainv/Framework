using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SSD.Framework;
using SSD.Web.Identity;
using SSD.Web.Models;
using SSD.Web.Mvc.Models;
using Microsoft.AspNet.Identity.Owin;
using SSD.Web.SSO;
using SSD.Web.Mvc.Controllers;

namespace SSD.Web.SSO.Mvc.Controllers
{
    [SSOAutshorize(Groups = "Admin")]
    public class SSOAppConfigsController : AppConfigsControllerBase
    {
        protected override void SetManager()
        {
            _configManager = HttpContext.GetOwinContext().Get<SSOAppConfigManager>();
            _grpManager = HttpContext.GetOwinContext().Get<SSOGroupManager>();
        }
        public SSOAppConfigsController():base(null,null)
        {
        }
        public SSOAppConfigsController(SSOAppConfigManager configManager, GroupManager grpManager) :base(configManager, grpManager)
        {
        }
    }
}
