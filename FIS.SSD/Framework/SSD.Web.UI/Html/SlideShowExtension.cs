using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Globalization;
using SSD.Web.UI.Maps;

namespace SSD.Web.UI.Html
{
    public enum ThemesSlideShowOnPage
    {
        Default,
        Orman,
        Pascal
    }
    public static class SlideShowExtension
    {
        public static MvcHtmlString SlideShow(this HtmlHelper helper, List<SSD.Web.PictureCore> lstPics)
        {
            var html = new TagBuilder("div");
            html.MergeAttribute("id", "picGallery");
            html.MergeAttribute("class", "gallery");
            if (lstPics.Count > 0)
            {
                var str = new StringBuilder("<div id=\"picGallery-main\">");
                str.AppendFormat("<a href=\"{0}\" rel=\"prettyPhoto[gallery2]\" title=\"{1}\">", lstPics[0].ImgUrl, lstPics[0].DisplayName)
                    .AppendFormat("<img alt=\"{0}\" title=\"{0}\" src=\"{1}\" /></a>", lstPics[0].DisplayName, lstPics[0].ImgUrlThumb_W300xH200)
                    .Append("</div>");
                if (lstPics.Count > 1)
                {
                    str.Append("<div id=\"picGallery-more\">")
                       .Append("<div id=\"picGallery-thumbnails\">");
                    if (lstPics.Count > 5)
                        str.Append("<div id=\"slideleft\" title=\"Slide Left\" ></div>")
                            .Append("<div id=\"slidearea\">")
                            .AppendFormat("<div id=\"slider\" style='width: {0}px; left: 0px;'>", (lstPics.Count) * 58 + 2);
                    else str.Append("<div id=\"slidearea\" class = \"slidearea\" >")
                            .Append("<div id=\"slider\" style='width: 260px; left: 0px;'>");

                    //str.Append("<ul>");
                    for (int i = 1; i < lstPics.Count; i++)
                    {
                        var item = lstPics[i];
                        //str.Append("<li>")
                        str.AppendFormat("<a href=\"{0}\" rel=\"prettyPhoto[gallery2]\" title=\"{1}\" class=\"imgThumb\">", item.ImgUrl, item.DisplayName)
                        .AppendFormat("<img id=\"pic-thumbnails{0}\" src=\"{1}\" width=\"64\" height=\"42\" alt=\"{2}\" /></a>", i, item.ImgUrlThumb_W64xH42, item.DisplayName);
                        //.Append("</li>");
                    }
                    //str.Append("</ul>");

                    str.Append("</div>")
                        .Append("</div>");
                    if (lstPics.Count > 5)
                    {
                        str.Append("<div id=\"slideright\" title=\"Slide Right\" ></div>");
                        str.Append("<script type=\"text/javascript\" >")
                            .Append("TINY.GetID('picGallery-more').style.display = 'block'; ")
                            .Append("var scroller = new TINY.scroller(\"scroller\"); ")
                            .Append("window.onload = function () { ")
                            .Append("scroller.thumbs = \"slider\"; ")
                            .Append("scroller.left = \"slideleft\"; ")
                            .Append("scroller.right = \"slideright\"; ")
                            .Append("scroller.scrollSpeed = 4; ")
                            .Append("scroller.init(); }; </script>");
                    }
                    str.Append("</div>")
                        .Append("</div>");
                }
                str.Append("<script type=\"text/javascript\" >")
                    .Append("$(document).ready(function () {")
                    .Append("$(\".gallery:first a[rel^='prettyPhoto']\").prettyPhoto({ animationSpeed: 'slow', slideshow: 2000, autoplay_slideshow: false });")/*theme: 'light_square',*/
                    .Append("});</script>");
                html.InnerHtml = str.ToString();
            }
            return new MvcHtmlString(html.ToString(TagRenderMode.Normal));
        }

        public static MvcHtmlString SlideShowOnPage(this HtmlHelper helper, List<SSD.Web.PictureCore> lstPics, ThemesSlideShowOnPage theme)
        {
            var html = new TagBuilder("div");
            html.MergeAttribute("class", "slider-wrapper " + GetThemesSlideShowOnPage(theme));
            if (lstPics.Count > 0)
            {
                var str = new StringBuilder("<div class=\"ribbon\"></div><div id=\"sliderOnPage\" class=\"nivoSlider\">");
                foreach (var pic in lstPics)
                {
                    if (!string.IsNullOrWhiteSpace(pic.LinkReference))
                        str.AppendFormat("<a href=\"{0}\" title = \"{1}\">", pic.LinkReference, pic.DisplayName);
                    str.AppendFormat("<img src=\"{0}\" alt=\"{1}\" title=\"#htmlcaption{2}\" />", pic.ImgUrl, pic.DisplayName, pic.PictureID);
                    if (!string.IsNullOrWhiteSpace(pic.LinkReference))
                        str.Append("</a>");
                }
                str.Append("</div>");
                foreach (var pic in lstPics)
                {
                    if (!string.IsNullOrWhiteSpace(pic.Description))
                    {
                        str.AppendFormat("<div id=\"htmlcaption{0}\" class=\"nivo-html-caption\">{1}</div>", pic.PictureID, pic.Description);
                    }
                }
                str.Append("<script type=\"text/javascript\">")
                    .Append("$(window).load(function() { $('#sliderOnPage').nivoSlider(); });")
                    .Append("</script>");

                html.InnerHtml = str.ToString();
            }
            return new MvcHtmlString(html.ToString(TagRenderMode.Normal));
        }

        private static string GetThemesSlideShowOnPage(ThemesSlideShowOnPage theme)
        {
            switch (theme)
            {
                case ThemesSlideShowOnPage.Orman:
                    return "theme-orman";
                case ThemesSlideShowOnPage.Pascal:
                    return "theme-pascal";
                default:
                    return "theme-default";
            }
        }

        public static MvcHtmlString SlideShowAjax(this HtmlHelper helper, List<SSD.Web.PictureCore> lstPics)
        {
            var html = new TagBuilder("div");
            html.MergeAttribute("id", "oSlide");
            html.MergeAttribute("class", "oSlide");
            if (lstPics.Count > 0)
            {
                var str = new StringBuilder("<script type=\"text/javascript\"> var Photos=[");
                
                foreach (var pic in lstPics)
                {
                    str.Append("{ \"url\": \""+ pic.ImgUrl + "\" ,  \"desc\" : \""+ pic.Description+"\"  },");
                }
                str.Remove(str.Length-1,1);
                str.Append("]; ")
                    .Append("$(document).ready(function(){ $(\"#oSlide\").oSlide({ images: Photos }); }) </script>");
                
                html.InnerHtml = str.ToString();
            }
            return new MvcHtmlString(html.ToString(TagRenderMode.Normal));
        }
    }
}
