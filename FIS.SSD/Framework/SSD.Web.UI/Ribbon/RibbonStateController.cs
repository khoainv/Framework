using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SSD.Web.UI.FileManager
{
    public class RibbonStateController : Controller
    {

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult GetCurrentTab()
        {
            return Content(Session["RibbonState.CurrentTab"] == null ? "0" : Session["RibbonState.CurrentTab"].ToString());
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SetCurrentTab(string accessKey)
        {
            Session["RibbonState.CurrentTab"] = accessKey;
            return Content(accessKey);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult GetAllState()
        {
            Dictionary<string,object> obj = new Dictionary<string,object>();
            obj["IsHide"] = Session["RibbonState.IsHide"] == null ? false : Convert.ToBoolean(Session["RibbonState.IsHide"]);
            obj["CurrentTab"] = Session["RibbonState.CurrentTab"] == null ? 0 : Convert.ToInt32(Session["RibbonState.CurrentTab"]);

            return Json(obj);// Content(new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(obj));
        }
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SetShowHide(bool isHide)
        {
            Session["RibbonState.IsHide"] = isHide;
            return Content(isHide.ToString());
        }
    }
}
