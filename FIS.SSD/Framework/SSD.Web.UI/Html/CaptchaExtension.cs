using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Globalization;
using SSD.Web.UI.Captcha;

namespace SSD.Web.UI.Html
{
    public static class CaptchaExtension
    {
        /// <summary>
        /// Helper method to create captcha.
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static MvcHtmlString Captcha(this HtmlHelper htmlHelper, int length)
        {
            return CaptchaHelper.GenerateFullCaptcha(htmlHelper, length);
        }

    }
}
