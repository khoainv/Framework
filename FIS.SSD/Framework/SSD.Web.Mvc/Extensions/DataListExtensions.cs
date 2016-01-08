using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;

namespace SSD.Web.Mvc.Extensions
{
    public static class DataListExtensions
    {
        public static IHtmlString DataListBindDiv<T>(this HtmlHelper helper, IEnumerable<T> items, int columns,
            Func<T, HelperResult> template)
            where T : class
        {
            if (items == null)
                return new HtmlString("");

            var sb = new StringBuilder();
            sb.Append("<div>");

            int cellIndex = 0;

            foreach (T item in items)
            {
                if (cellIndex == 0)
                {
                    //bắt đầu dòng mới
                    sb.Append("<div class=\"row-item\">");
                    ///bắt đầu cột đầu
                    sb.Append("<div class=\"col-item\">");
                }
                else if (cellIndex == columns - 1)
                {
                    //bắt đầu cột cuối
                    sb.Append("<div class=\"col-item\">");
                }
                else
                {
                    sb.Append("<div class=\"col-item\"");
                    sb.Append(">");
                }

                sb.Append(template(item).ToHtmlString());
                sb.Append("</div>");

                cellIndex++;

                if (cellIndex == columns)
                {
                    cellIndex = 0;
                    sb.Append("</div>");
                    sb.Append("<div style=\"clear:both;\"></div>");
                }
            }

            sb.Append("</div>");

            return new HtmlString(sb.ToString());
        }

        public static IHtmlString DataListBindDiv2<T>(this HtmlHelper helper, IEnumerable<T> items, int columns,
            Func<T, HelperResult> template)
            where T : class
        {
            const int width = 1024;
            int wCol = width/columns;
            if (items == null)
                return new HtmlString("");

            var sb = new StringBuilder();

            int cellIndex = 0;
            int iCount = items.ToList().Count();

            for (int i = 0; i < iCount; i++)
            {
                T item = items.ToList()[i];

                if (cellIndex == 0)
                {
                    //bắt đầu dòng mới
                    sb.Append("<div class=\"row-item\">");
                    ///bắt đầu cột đầu
                    sb.Append("<div class=\"col-item\" style=\"width:" + wCol + "px;\">");
                }
                //cot cuoi cung
                else if (cellIndex == columns - 1 || i == iCount - 1)
                {
                    //bắt đầu cột cuối
                    sb.Append("<div class=\"col-item\" style=\"width:" + wCol + "px;\">");
                }
                else
                {
                    sb.Append("<div class=\"col-item\" style=\"width:" + wCol + "px;\"");
                    sb.Append(">");
                }

                sb.Append(template(item).ToHtmlString());
                sb.Append("</div>");

                cellIndex++;

                if (cellIndex == columns)
                {
                    cellIndex = 0;
                    sb.Append("</div>");
                    sb.Append("<div style=\"clear:both;\"></div>");
                }
            }

            return new HtmlString(sb.ToString());
        }
    }
}
