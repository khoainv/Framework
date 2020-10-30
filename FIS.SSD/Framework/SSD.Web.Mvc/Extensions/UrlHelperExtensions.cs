using System;
using System.Web;
using System.Web.Mvc;

namespace SSD.Web.Mvc.Extensions
{
    public static class UrlHelperExtensions
    {
        /// <summary>
        /// Description: Modules the URL.
        /// </summary>
        /// <param name="urlHelper">The URL helper.</param>
        /// <param name="url">The URL.</param>
        /// <returns>System.String.</returns>
        public static string ModuleUrl(this UrlHelper urlHelper, string url)
        {
            return ResolveUrl(url);
        }
        public static string RenderProductUrl(this UrlHelper urlHelper, string productName, int productId)
        {
            var seName = UrlSlugger.ToUrlSlug(productName);
            return string.Format("{0}-p{1}.html", seName, productId);
        }


        public static string ResolveUrl(this UrlHelper urlHelper, string url, bool isExternalUrl = false)
        {
            if (isExternalUrl)
            {
                return url;
            }
            return ResolveUrl(url);
        }

        public static string ResolveUrl(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return url;
            }
            url = url.TrimStart('/');
            return BaseUrl() + url;
        }

        /// <summary>
        /// Author: MarkNguen
        /// Created on: 11/14/2014 11:32 AM
        /// Description: Bases the URL.
        /// </summary>
        /// <returns>System.String.</returns>
        public static string BaseUrl()
        {
            string resolveUrl;
            var applicationPath = HttpContext.Current.Request.ApplicationPath + "";
            if (applicationPath == "/")
            {
                resolveUrl = applicationPath;
            }
            else
            {
                if (!applicationPath.EndsWith("/"))
                {
                    applicationPath += "/";
                }
                resolveUrl = applicationPath;
            }
            return resolveUrl;
        }

        public static string AbsoluteUrl(this UrlHelper helper, string actionUrl)
        {
            Uri requestUrl = helper.RequestContext.HttpContext.Request.Url;
            string absoluteAction = string.Format("{0}://{1}/{2}",
                                                  requestUrl.Scheme,
                                                  requestUrl.Authority,
                                                  actionUrl);
            return absoluteAction;
        }

        public static string AbsoluteAction(this UrlHelper helper, string actionName)
        {
            string relUrl = helper.Action(actionName);
            return string.Format("{0}/{1}", GetSiteUrl(helper), relUrl);
        }
        public static string AbsoluteAction(this UrlHelper helper, string actionName, string controllerName)
        {
            string relUrl = helper.Action(actionName, controllerName);
            return string.Format("{0}/{1}", GetSiteUrl(helper), relUrl);
        }

        public static string AbsoluteAction(this UrlHelper helper, string actionName, string controllerName, object values)
        {
            string relUrl = helper.Action(actionName, controllerName, values);

            return string.Format("{0}/{1}", GetSiteUrl(helper), relUrl);
        }

        public static string GetSiteUrl(this UrlHelper helper)
        {
            var ctx = HttpContext.Current;//helper.RequestContext.HttpContext;
            string Port = ctx.Request.ServerVariables["SERVER_PORT"];
            if (Port == null || Port == "80" || Port == "443")
                Port = "";
            else
                Port = ":" + Port;

            string Protocol = ctx.Request.ServerVariables["SERVER_PORT_SECURE"];
            if (Protocol == null || Protocol == "0")
                Protocol = "http://";
            else
                Protocol = "https://";

            string appPath = ctx.Request.ApplicationPath;
            if (appPath == "/")
                appPath = "";

            string sOut = Protocol + ctx.Request.ServerVariables["SERVER_NAME"] + Port + appPath;
            return sOut;
        }
        public static string GetSiteUrl(this HttpContext context)
        {
            HttpContextWrapper wrapper = new HttpContextWrapper(context);
            return GetSiteUrl(wrapper as HttpContextBase, true);
        }
        public static string GetSiteUrl(HttpContextBase context, bool usePort)
        {
            string Port = context.Request.ServerVariables["SERVER_PORT"];
            if (usePort)
            {
                if (Port == null || Port == "80" || Port == "443")
                    Port = "";
                else
                    Port = ":" + Port;
            }
            string Protocol = context.Request.ServerVariables["SERVER_PORT_SECURE"];
            if (Protocol == null || Protocol == "0")
                Protocol = "http://";
            else
                Protocol = "https://";

            string appPath = context.Request.ApplicationPath;
            if (appPath == "/")
                appPath = "";

            string sOut = Protocol + context.Request.ServerVariables["SERVER_NAME"] + Port + appPath;
            return sOut;

        }

        public static string GetSiteUrl(this ViewPage pg)
        {
            return GetSiteUrl(pg.Url);
        }
        public static string GetSiteUrl(this ViewMasterPage pg)
        {
            return GetSiteUrl(pg.Url);
        }

        public static string GetReturnUrl(this UrlHelper helper)
        {
            return GetReturnUrl(helper.RequestContext.HttpContext);

        }

        public static string GetReturnUrl(this ViewMasterPage pg)
        {
            return GetReturnUrl(pg.ViewContext.HttpContext);
        }

        public static string GetReturnUrl(this ViewUserControl pg)
        {
            return GetReturnUrl(pg.ViewContext.HttpContext);
        }
        /// <summary>
        /// Creates a ReturnUrl for use with the Login page
        /// </summary>
        public static string GetReturnUrl(this ViewPage pg)
        {
            return GetReturnUrl(pg.ViewContext.HttpContext);
        }

        static string GetReturnUrl(HttpContextBase ctx)
        {
            string returnUrl = "";

            if (ctx.Request.QueryString["ReturnUrl"] != null)
            {
                returnUrl = ctx.Request.QueryString["ReturnUrl"];
            }
            else
            {
                returnUrl = ctx.Request.Url.AbsoluteUri;
            }
            return returnUrl;
        }
        
        public static string BaseSiteUrl
        {
            get
            {
                System.Web.HttpContext context = System.Web.HttpContext.Current;
                string baseUrl = context.Request.Url.Scheme + "://" + context.Request.Url.Authority + context.Request.ApplicationPath.TrimEnd('/') + '/';
                return baseUrl;
            }
        }
        public static string SiteUrl
        {
            get
            {
                System.Web.HttpContext context = System.Web.HttpContext.Current;
                string baseUrl = context.Request.Url.Scheme + "://" + context.Request.Url.Authority + context.Request.ApplicationPath.TrimEnd('/');
                return baseUrl;
            }
        }
    }
}
