using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using SSD.Framework.Security;
using SSD.Web.Caching;
using SSD.Web.Identity;
using SSD.Web.Models;
using SSD.Web.Mvc.Controllers;
using SSD.Web.Security;

namespace SSD.SSO.Sample.Controllers
{

    [UGPermission(Key = "SampleController", Name = "Danh sách chức năng trang chủ")]
    public class SampleController : SampleBaseController
    {
        [UGPermission(Key = "SampleController.Get", Name = "Get thông tin User")]
        public ActionResult Get()
        {
            return View(User.Claims);
        }
       
    }
}