﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Routing;

namespace SSD.Web.Mvc.MvcPaging
{

    public class AjaxPager
    {
        private readonly AjaxHelper _ajaxHelper;
        private ViewContext _viewContext;
        private readonly Options _options;
        private readonly RouteValueDictionary _linkWithoutPageValuesDictionary;
        private readonly AjaxOptions _ajaxOptions;

        public AjaxPager(AjaxHelper helper, ViewContext viewContext, Options options, AjaxOptions ajaxOptions, RouteValueDictionary valueDictionary)
        {
            _ajaxHelper = helper;
            _viewContext = viewContext;
            _options = options;
            _ajaxOptions = ajaxOptions;
            _linkWithoutPageValuesDictionary = valueDictionary;
        }

        public string RenderHtml()
        {
            if (_options.TotalItemCount <= _options.PageSize)
            {
                return string.Empty;
            }
            var pageNumber1 = (int)Math.Ceiling((double)this._options.TotalItemCount / (double)this._options.PageSize);
            const int num1 = 10;
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat("<div class=\"pagination pagination-{0} pagination-{1} {2}\"><ul>", (object)((object)this._options.Size).ToString(), (object)((object)this._options.Alignment).ToString(), (object)this._options.CssClass);
            if (this._options.IsShowFirstLast)
            {
                if (this._options.CurrentPage > 1)
                    stringBuilder.AppendFormat("<li class=\"first\">{0}</li>", (object)this.GeneratePageLink(this._options.ItemTexts.First, 1, this._options.TooltipTitles.First, this._options.ItemIcon.First, ""));
                else if (string.IsNullOrWhiteSpace(this._options.ItemIcon.First))
                    stringBuilder.AppendFormat("<li class=\"active first\"><span>{0}</span></li>", (object)this._options.ItemTexts.First);
                else
                    stringBuilder.AppendFormat("<li class=\"active first\"><span><i class=\"{1}\"></i> {0}</span></li>", (object)this._options.ItemTexts.First, (object)this._options.ItemIcon.First);
            }
            if (this._options.IsShowControls)
            {
                if (this._options.CurrentPage > 1)
                    stringBuilder.AppendFormat("<li class=\"previous\">{0}</li>", (object)this.GeneratePageLink(this._options.ItemTexts.Previous, this._options.CurrentPage - 1, this._options.TooltipTitles.Previous, this._options.ItemIcon.Previous, ""));
                else if (string.IsNullOrWhiteSpace(this._options.ItemIcon.Previous))
                    stringBuilder.AppendFormat("<li class=\"active previous\"><span>{0}</span></li>", (object)this._options.ItemTexts.Previous);
                else
                    stringBuilder.AppendFormat("<li class=\"active previous\"><span><i class=\"{1}\"></i> {0}</span></li>", (object)this._options.ItemTexts.Previous, (object)this._options.ItemIcon.Previous);
            }
            int num2 = 1;
            int num3 = pageNumber1;
            if (this._options.IsShowPages)
            {
                if (pageNumber1 > num1)
                {
                    int num4 = (int)Math.Ceiling((double)num1 / 2.0) - 1;
                    int num5 = this._options.CurrentPage - num4;
                    int num6 = this._options.CurrentPage + num4;
                    if (num5 < 4)
                    {
                        num6 = num1;
                        num5 = 1;
                    }
                    else if (num6 > pageNumber1 - 4)
                    {
                        num6 = pageNumber1;
                        num5 = pageNumber1 - num1;
                    }
                    num2 = num5;
                    num3 = num6;
                }
                if (!this._options.IsShowFirstLast && num2 > 3)
                {
                    stringBuilder.AppendFormat("<li>{0}</li>", (object)this.GeneratePageLink(string.Format("{0}{1}", (object)this._options.ItemTexts.Page, (object)"1"), 1, this._options.TooltipTitles.Page, this._options.ItemIcon.Page, ""));
                    stringBuilder.AppendFormat("<li>{0}</li>", (object)this.GeneratePageLink(string.Format("{0}{1}", (object)this._options.ItemTexts.Page, (object)"2"), 2, this._options.TooltipTitles.Page, this._options.ItemIcon.Page, ""));
                    stringBuilder.Append("<li class=\"disabled\"><span>...</span></li>");
                }
                for (int pageNumber2 = num2; pageNumber2 <= num3; ++pageNumber2)
                {
                    if (pageNumber2 == this._options.CurrentPage)
                        stringBuilder.AppendFormat("<li class=\"active\"><span>{0}</span></li>", (object)string.Format("{0}{1}", (object)this._options.ItemTexts.Page, (object)pageNumber2));
                    else
                        stringBuilder.AppendFormat("<li>{0}</li>", (object)this.GeneratePageLink(string.Format("{0}{1}", (object)this._options.ItemTexts.Page, (object)pageNumber2.ToString()), pageNumber2, this._options.TooltipTitles.Page, this._options.ItemIcon.Page, ""));
                }
                if (!this._options.IsShowFirstLast && num3 < pageNumber1 - 3)
                {
                    stringBuilder.Append("<li class=\"disabled\"><span>...</span></li>");
                    stringBuilder.AppendFormat("<li>{0}</li>", (object)this.GeneratePageLink(string.Format("{0}{1}", (object)this._options.ItemTexts.Page, (object)(pageNumber1 - 1).ToString()), pageNumber1 - 1, this._options.TooltipTitles.Page, this._options.ItemIcon.Page, ""));
                    stringBuilder.AppendFormat("<li>{0}</li>", (object)this.GeneratePageLink(string.Format("{0}{1}", (object)this._options.ItemTexts.Page, (object)pageNumber1.ToString()), pageNumber1, this._options.TooltipTitles.Page, this._options.ItemIcon.Page, ""));
                }
            }
            if (this._options.IsShowControls)
            {
                if (this._options.CurrentPage < pageNumber1)
                    stringBuilder.AppendFormat("<li class=\"next\">{0}</li>", (object)this.GeneratePageLink(this._options.ItemTexts.Next, this._options.CurrentPage + 1, this._options.TooltipTitles.Next, this._options.ItemIcon.Next, "last"));
                else if (string.IsNullOrWhiteSpace(this._options.ItemIcon.Next))
                    stringBuilder.AppendFormat("<li class=\"active next\"><span>{0}</span></li>", (object)this._options.ItemTexts.Next);
                else
                    stringBuilder.AppendFormat("<li class=\"active next\"><span>{0} <i class=\"{1}\"></i></span></li>", (object)this._options.ItemTexts.Next, (object)this._options.ItemIcon.Next);
            }
            if (this._options.IsShowFirstLast)
            {
                if (this._options.CurrentPage < pageNumber1)
                    stringBuilder.AppendFormat("<li class=\"last\">{0}</li>", (object)this.GeneratePageLink(this._options.ItemTexts.Last, pageNumber1, this._options.TooltipTitles.Last, this._options.ItemIcon.Last, "last"));
                else if (string.IsNullOrWhiteSpace(this._options.ItemIcon.Last))
                    stringBuilder.AppendFormat("<li class=\"active last\"><span>{0}</span></li>", (object)this._options.ItemTexts.Last);
                else
                    stringBuilder.AppendFormat("<li class=\"active last\"><span>{0} <i class=\"{1}\"></i></span></li>", (object)this._options.ItemTexts.Last, (object)this._options.ItemIcon.Last);
            }
            stringBuilder.Append("</ul></div>");
            return ((object)stringBuilder).ToString();
        }

        private MvcHtmlString GeneratePageLink(string linkText, int pageNumber, string title, string icon, string buttonType = "")
        {
            RouteValueDictionary values1 = this._viewContext.RequestContext.RouteData.Values;
            var routeValues = new RouteValueDictionary(this._linkWithoutPageValuesDictionary)
            {
                {"page", (object) pageNumber}
            };

            if (!routeValues.ContainsKey("controller") && values1.ContainsKey("controller"))
                routeValues.Add("controller", values1["controller"]);
            if (!routeValues.ContainsKey("action") && values1.ContainsKey("action"))
                routeValues.Add("action", values1["action"]);

            string str1 = string.Format(title, (object)pageNumber);
            var routeValueDictionary = new RouteValueDictionary {{"title", (object) str1}};
            MvcHtmlString mvcHtmlString2;
            if (string.IsNullOrWhiteSpace(icon))
            {
                mvcHtmlString2 = this._ajaxHelper.ActionLink(linkText, routeValues["action"].ToString(), routeValues, this._ajaxOptions, routeValueDictionary);
            }
            else
            {
                string str2 = string.Empty;
                var tagBuilder = new TagBuilder("i");
                tagBuilder.MergeAttribute("class", icon);
                mvcHtmlString2 = new MvcHtmlString((!string.IsNullOrWhiteSpace(buttonType) ? AjaxExtensions.ActionLink(this._ajaxHelper, linkText + " [replaceIcon]", routeValues["action"].ToString(), routeValues, this._ajaxOptions, (IDictionary<string, object>)routeValueDictionary).ToHtmlString() : AjaxExtensions.ActionLink(this._ajaxHelper, "[replaceIcon] " + linkText, routeValues["action"].ToString(), routeValues, this._ajaxOptions, (IDictionary<string, object>)routeValueDictionary).ToHtmlString()).Replace("[replaceIcon]", ((object)tagBuilder).ToString()));
            }
            return mvcHtmlString2;
        }
    }
}
