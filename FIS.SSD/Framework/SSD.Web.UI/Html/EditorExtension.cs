using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Globalization;
using SSD.Web.UI.Editor;

namespace SSD.Web.UI.Html
{
    public static class UGEditorExtension
    {
        /// <summary>
        /// Normalizes a url in the form ~/path/to/resource.aspx.
        /// </summary>
        /// <param name="html"></param>
        /// <param name="relativeUrl"></param>
        /// <returns></returns>
        public static MvcHtmlString UGEditor(this HtmlHelper html, string name)
        {
            return MvcHtmlString.Create(EditorService.Render(name));
        }
        /// <summary>
        /// Normalizes a url in the form ~/path/to/resource.aspx.
        /// </summary>
        /// <param name="html"></param>
        /// <param name="relativeUrl"></param>
        /// <returns></returns>
        public static MvcHtmlString UGEditor(this HtmlHelper html, string providerName, string name)
        {
            return MvcHtmlString.Create(EditorService.Render(providerName, name));
        }
        /// <summary>
        /// Normalizes a url in the form ~/path/to/resource.aspx.
        /// </summary>
        /// <param name="html"></param>
        /// <param name="relativeUrl"></param>
        /// <returns></returns>
        public static MvcHtmlString UGCodeEditor(this HtmlHelper html, string providerName, string name, string pathFile)
        {
            return MvcHtmlString.Create(EditorService.CodeRender(providerName, name, pathFile));
        }
    }
}
