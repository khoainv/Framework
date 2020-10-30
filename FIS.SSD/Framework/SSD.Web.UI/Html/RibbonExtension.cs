using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Globalization;
using SSD.Web.UI.Ribbon;

namespace SSD.Web.UI.Html
{
    public static class RibbonExtension
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="html"></param>
        /// <param name="relativeUrl"></param>
        /// <returns></returns>
        public static MvcHtmlString MenuRibbonTree(this HtmlHelper html, List<MenuItem> menu)
        {
            return MvcHtmlString.Create(RibbonService.RenderMenuTree(menu));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="html"></param>
        /// <param name="relativeUrl"></param>
        /// <returns></returns>
        public static MvcHtmlString MenuRibbon(this HtmlHelper html, List<MenuRibbon> menu)
        {
            return MvcHtmlString.Create(RibbonService.RenderMenuRibbon(html, menu));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="html"></param>
        /// <param name="relativeUrl"></param>
        /// <returns></returns>
        public static MvcHtmlString MenuRibbon(this HtmlHelper html, List<MenuItem> sysMenu, List<MenuRibbon> menu)
        {
            return MvcHtmlString.Create(RibbonService.RenderMenu(html,sysMenu, menu));
        }
    }
}
