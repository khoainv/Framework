using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SSD.Web.UI.Ribbon;
using System.Web.Helpers;
using SSD.Web.UI.Captcha;
using SSD.Web.UI.FileManager;

namespace MvcAppTest.Controllers
{
    public class HomeController : BaseController
    {
        //
        // GET: /Home/
        public ActionResult Index()
        {
            //KeyValuePair<List<MenuRibbon>, List<MenuItem>> k = new KeyValuePair<List<SSD.Web.UI.Ribbon.MenuRibbon>, List<SSD.Web.UI.Ribbon.MenuItem>>();//
            //k = d.First();
            return View();
        }
        public ActionResult IntergrateTest()
        {
            return View();
        }
        public ActionResult MaskInput()
        {
            return View();
        }
        public ActionResult FileManager()
        {
            return View();
        }
        //For forms with captcha you need to take CaptchaModel.
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Index(CaptchaModel captchaModel, string SimleText, string Description)
        {
            //To verify the captcha call the CaptchaHelper.Verify.(CaptchaModel)
            //as a parameter to transfer the CaptchaModel.
            if (CaptchaHelper.Verify(captchaModel))
            {
                return
                    Content(string.Format("CAPTCHA is valid.<br/>Your Model:<br/>{0}<br/>{1}:", SimleText,
                                          Description));
            }
            
            return Content("CAPTCHA is invalid");
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AjaxRibbon(string SimleText, string Description)
        {
            return Content("Ajax ribbon" + SimleText + Description);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult ImageProperty()
        {
            return PartialView();
        }
    }
}
