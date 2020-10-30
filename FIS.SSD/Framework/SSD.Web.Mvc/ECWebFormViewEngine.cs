using System;
using System.Web.Mvc;

namespace SSD.Web.Mvc
{
    public class ECWebFormViewEngine : WebFormViewEngine
    {

        private string strDefaultTheme = "Default";

        public ECWebFormViewEngine()
        {
            this.MasterLocationFormats = new[]
                                    {
                                        //"~/Media/System/{1}/{0}.master",
                                        //"~/Media/System/{2}/{1}/{0}.master",
                                        "~/Views/{1}/{0}.master",
                                        "~/Views/Shared/{0}.master"
                                    };

            ViewLocationFormats = new[]
                                  {                                     
                                      "~/Views/{1}/{0}.aspx",
                                      "~/Views/{1}/{0}.ascx",
                                      "~/Widgets/{0}.ascx",
                                      "~/Widgets/Common/{0}.ascx",
                                      "~/Views/Shared/{0}.aspx",
                                      "~/Widgets/{0}/View/{0}.ascx",
                                      "~/Widgets/Common/{0}/View/{0}.ascx",
                                      "~/Views/Admin/Widgets/{0}.ascx"
                                  };

            PartialViewLocationFormats = ViewLocationFormats;
        }
        public ECWebFormViewEngine(string template)
        {
            this.MasterLocationFormats = new[]
                                    {
                                        //"~/Media/System/{1}/{0}.master",
                                        //"~/Media/System/{2}/{1}/{0}.master",
                                        "~/"+template+"/Views/{1}/{0}.master",
                                        "~/"+template+"/Views/Shared/{0}.master"
                                    };

            ViewLocationFormats = new[]
                                  {                                     
                                      "~/"+template+"/Views/{1}/{0}.aspx",
                                      "~/"+template+"/Views/{1}/{0}.ascx",
                                      "~/"+template+"/Widgets/{0}.ascx",
                                      "~/"+template+"/Widgets/Common/{0}.ascx",
                                      "~/"+template+"/Views/Shared/{0}.aspx",
                                      "~/"+template+"/Widgets/{0}/View/{0}.ascx",
                                      "~/"+template+"/Widgets/Common/{0}/View/{0}.ascx",
                                      "~/"+template+"/Views/Admin/Widgets/{0}.ascx"
                                  };

            PartialViewLocationFormats = ViewLocationFormats;
        }


        protected override IView CreatePartialView(ControllerContext controllerContext, string strPartialPath)
        {
            return new WebFormView(controllerContext,strPartialPath);
        }

        protected override IView CreateView(ControllerContext controllerContext, string strViewPath, string strMasterPath)
        {
            return new WebFormView(controllerContext, strViewPath, strMasterPath);
        }

        public override ViewEngineResult FindPartialView(ControllerContext controllerContext, string strPartialViewName, bool fUseCache)
        {
            if (controllerContext == null)
            {
                throw new ArgumentNullException("controllerContext");
            }

            if (string.IsNullOrEmpty(strPartialViewName))
            {
                throw new ArgumentNullException("partialViewName");
            }

            string strThemeName = controllerContext.HttpContext.Application["ThemeName"].ToString() ; 
            string strControllerName = controllerContext.RouteData.GetRequiredString("controller");

            string[] arrLocationsSearched;
            string strPartialPath = FindPath(PartialViewLocationFormats, strPartialViewName, strControllerName, strThemeName,
                                          out arrLocationsSearched);

            return !string.IsNullOrEmpty(strPartialPath)
                       ? new ViewEngineResult(CreatePartialView(controllerContext, strPartialPath), this)
                       : new ViewEngineResult(arrLocationsSearched);
        }

        public override ViewEngineResult FindView(ControllerContext controllerContext, string strViewName, string strMasterName, bool fUseCache)
        {
            if (controllerContext == null)
            {
                throw new ArgumentNullException("controllerContext");
            }

            if (string.IsNullOrEmpty(strViewName))
            {
                throw new ArgumentNullException("viewName");
            }

            string strThemeName = controllerContext.HttpContext.Application["ThemeName"].ToString(); //todo(nheskew): where/how do user themes come into play?
            string strControllerName = controllerContext.RouteData.GetRequiredString("controller");

            string[] arrViewLocationsSearched;
            string strViewPath = FindPath(ViewLocationFormats, strViewName, strControllerName, strThemeName,
                                       out arrViewLocationsSearched);

            if (string.IsNullOrEmpty(strViewPath))
            {
                return new ViewEngineResult(arrViewLocationsSearched);
            }

            string[] arrMasterLocationsSearched;
            string strMasterPath = FindPath(MasterLocationFormats, strMasterName, strControllerName, strThemeName,
                                         out arrMasterLocationsSearched);

            if (!string.IsNullOrEmpty(strMasterName) && string.IsNullOrEmpty(strMasterPath))
            {
                return new ViewEngineResult(arrMasterLocationsSearched);
            }

            return new ViewEngineResult(CreateView(controllerContext, strViewPath, strMasterPath), this);
        }

        private string FindPath(string[] arrLocationsToSearch, string strViewName, string strControllerName, string strRequestedTheme,
                                out string[] arrLocationsSearched)
        {
            int iLocationCount = 0;
            string strThemeName = !string.IsNullOrEmpty(strRequestedTheme) ? strRequestedTheme : strDefaultTheme;
            arrLocationsSearched = new string[arrLocationsToSearch.Length];

            foreach (string strLocationFormat in arrLocationsToSearch)
            {
                string path = string.Format(strLocationFormat, strViewName, strControllerName, strThemeName);
                arrLocationsSearched[iLocationCount++] = path;
                if (VirtualPathProvider.FileExists(path))
                {
                    return path;
                }
            }

            return null;
        }
    }
}
