using System;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Routing;
using SSD.Web.Mvc.MvcPaging;

namespace SSD.Web.Mvc.Extensions
{
    public static class AjaxPagingExtensions
    {
        public static string Pager(this AjaxHelper ajaxHelper, Options options, AjaxOptions ajaxOptions, object values)
        {
            return AjaxPagingExtensions.Pager(ajaxHelper, options, ajaxOptions, new RouteValueDictionary(values));
        }

        public static string Pager(this AjaxHelper ajaxHelper, Options options, AjaxOptions ajaxOptions, RouteValueDictionary valuesDictionary)
        {
            if (valuesDictionary == null)
                valuesDictionary = new RouteValueDictionary();
            if (options.ActionName != null)
            {
                if (valuesDictionary.ContainsKey("action"))
                    throw new ArgumentException("The valuesDictionary already contains an action.", "actionName");
                valuesDictionary.Add("action", (object)options.ActionName);
            }
            return new AjaxPager(ajaxHelper, ajaxHelper.ViewContext, options, ajaxOptions, valuesDictionary).RenderHtml();
        }
    }
}