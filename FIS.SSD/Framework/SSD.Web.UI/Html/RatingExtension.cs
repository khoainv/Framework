using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Globalization;

namespace SSD.Web.UI.Html
{
    public static class RatingExtension
    {
        public static MvcHtmlString Rating(this HtmlHelper helper, string controlID, double currentValue, double step = 0.5, int maxRate = 5, bool includeScript = false, string srcJs = "")
        {
            var str = new StringBuilder(string.Format("<input type=\"hidden\" id=\"{0}\" name=\"{0}\" value=\"{1}\" step=\"{2}\" >", controlID, currentValue, step));
            str.AppendFormat("<div class=\"rateit\" id=\"{0}div\" data-rateit-backingfld=\"#{0}\" data-rateit-resetable=\"false\" data-rateit-ispreset=\"true\" data-rateit-min=\"0\" data-rateit-max=\"{1}\"></div>", controlID, maxRate)
                .Append("<script type=\"text/javascript\">")
                .Append(string.Format("$(\"#{0}div\").bind('hover', function (event, value)",controlID) + "{ $(this).attr('title', value); });")
                .Append(string.Format("$(\"#{0}div\").bind('rated', function (event, value)",controlID) + "{ $('#"+controlID+"').val(value); });")
                .Append("</script>");
            if (includeScript)
                str.Append(string.Format("<script src=\"{0}\" type=\"text/javascript\"></script>", srcJs));
            return new MvcHtmlString(str.ToString());
        }

        public static MvcHtmlString RatingAjax(this HtmlHelper helper,string controller, string action, string objectId,  string controlID,double currentValue, double step = 0.5, int maxRate = 5, bool includeScript = false, string srcJs = "")
        {
            var str = new StringBuilder(string.Format("<input type=\"hidden\" id=\"{0}\" name=\"{0}\" value=\"{1}\" step=\"{2}\" >", controlID, currentValue, step));

            str.AppendFormat("<div id=\"{0}{1}\" data-productid=\"{1}\" data-rateit-backingfld=\"#{0}\" class=\"rateit\" data-rateit-resetable=\"false\" data-rateit-ispreset=\"true\" data-rateit-min=\"0\" data-rateit-max=\"{2}\"></div>", controlID,objectId, maxRate)
                .Append("<script type=\"text/javascript\">")
                .AppendFormat("$('#{0}{1}').bind('rated reset', function (e)", controlID, objectId)
                .Append("{var ri = $(this);var value = ri.rateit('value');var productID = ri.data('productid');ri.rateit('readonly', true);")
                .Append("$.ajax({")
                .AppendFormat("url: '/{0}/{1}',",controller,action)
                .Append("data: { id: productID, value: value },")
                .Append("type: 'POST',")
                .Append("success: function (data) {if(!(!data || 0 == data.length)) alert(data.toString()); },")
                .Append("error: function (jxhr, msg, err) { alert(msg);}")
                .Append("});")
                .Append("});")
                .Append("</script>");
            if (includeScript)
                str.AppendFormat("<script src=\"{0}\" type=\"text/javascript\"></script>", srcJs);
            return new MvcHtmlString(str.ToString());
        }
    }
}
