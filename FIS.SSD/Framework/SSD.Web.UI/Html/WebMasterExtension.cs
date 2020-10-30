using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Globalization;
using System.Web;

namespace SSD.Web.UI.Html
{
    public static class WebMasterExtension
    {
        public static MvcHtmlString BingSiteVerificationMetaTag(this HtmlHelper helper, string key)
        {
            var html = new TagBuilder("meta");
            html.MergeAttribute("name", "msvalidate.01");
            html.MergeAttribute("content", key);
            return MvcHtmlString.Create(html.ToString(TagRenderMode.SelfClosing));
        }
        public static MvcHtmlString GoogleSiteVerificationMetaTag(this HtmlHelper helper, string key)
        {
            var html = new TagBuilder("meta");
            html.MergeAttribute("name", "google-site-verification");
            html.MergeAttribute("content", key);
            return MvcHtmlString.Create(html.ToString(TagRenderMode.SelfClosing));
        }

        public static MvcHtmlString GoogleAnalyticsTrackingScript(this HtmlHelper helper, string accountId)
        {
            accountId = HttpUtility.HtmlEncode(accountId);
            var html = new TagBuilder("script");
            html.MergeAttribute("type", "text/javascript");
            var javascriptCode = new StringBuilder("");
            javascriptCode.Append(Environment.NewLine);
            javascriptCode.Append("var _gaq = _gaq || [];");
            javascriptCode.Append(Environment.NewLine);
            javascriptCode.Append(string.Format("_gaq.push(['_setAccount', '{0}']);", accountId));
            javascriptCode.Append(Environment.NewLine);
            javascriptCode.Append("_gaq.push(['_trackPageview']);");
            javascriptCode.Append(Environment.NewLine);
            javascriptCode.Append("(function() {");
            javascriptCode.Append(Environment.NewLine);
            javascriptCode.Append("var ga = document.createElement('script'); ga.type = 'text/javascript'; ga.async = true;");
            javascriptCode.Append(Environment.NewLine);
            javascriptCode.Append("ga.src = ('https:' == document.location.protocol ? 'https://ssl' : 'http://www') ");
            javascriptCode.Append(Environment.NewLine);
            javascriptCode.Append("+ '.google-analytics.com/ga.js';");
            javascriptCode.Append(Environment.NewLine);
            javascriptCode.Append("var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(ga, s);");
            javascriptCode.Append(Environment.NewLine);
            javascriptCode.Append("})();");
            javascriptCode.Append(Environment.NewLine);
            html.InnerHtml = javascriptCode.ToString();
            return MvcHtmlString.Create(html.ToString(TagRenderMode.Normal));
        }

    }
}
