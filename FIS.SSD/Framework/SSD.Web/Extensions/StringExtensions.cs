using System;
using System.Text;
using System.Web;
using System.Text.RegularExpressions;

namespace SSD.Web.Extensions
{
    public static class StringExtensions
    {
        public static string MapPath(this string virtualPath)
        {
            return HttpContext.Current.Server.MapPath(virtualPath);
        }
        /// <summary>
        /// Lay anh trong bai viet lam anh xem truoc
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string GetImageFromContent(this string content)
        {
            string pattern = @"<\s*(img|IMG)[^>]*(src|SRC)\s*=\s*[\'\""]([^\'\""]*)\s*[\'\""][^>]*>";
            string img = "Images/noImage.jpg";
            if (String.IsNullOrEmpty(content))
                return img;
            Regex re = new Regex(pattern);
            if (re.IsMatch(content))
            {
                foreach (Match item in re.Matches(content))
                {
                    if (!string.IsNullOrEmpty(item.Groups[item.Groups.Count - 1].Value))
                    {
                        img = item.Groups[item.Groups.Count - 1].Value;
                        break;
                    }
                }
            }
            return img;
        }
        /// <summary>
        /// Lay anh trong bai viet lam anh xem truoc
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string Split(this string str, int lengthChar)
        {
            if (!string.IsNullOrEmpty(str))
            {
                if (str.Length <= lengthChar)
                    return str + "...";

                System.Text.RegularExpressions.Regex regHtml = new System.Text.RegularExpressions.Regex("<[^>]*>");
                str = regHtml.Replace(str, "");

                return str.Substring(0, lengthChar) + "...";
            }
            return str;
        }

        public static string SplitWithWord(this string str, int length)
        {
            if (!string.IsNullOrEmpty(str))
            {
                if (str.Length <= length)
                    return str + "...";

                System.Text.RegularExpressions.Regex regHtml = new System.Text.RegularExpressions.Regex("<[^>]*>");
                str = regHtml.Replace(str, "");

                str = str.Substring(0, length);
                string[] aStr = str.Split(new char[] {' '});
                StringBuilder strBuilder = new StringBuilder();
                for (int i = 0; i < aStr.Length - 1; i++)
                {
                    if (strBuilder.ToString() != string.Empty)
                        strBuilder.Append(' ');
                    strBuilder.Append(aStr[i]);
                }
                strBuilder.Append("...");
                return strBuilder.ToString();
            }
            return str;
        }
        public static string CreateSlug(this string source)
        {
            var regex = new Regex(@"([^a-z0-9\-]?)");
            string slug = "";

            if (!string.IsNullOrEmpty(source))
            {
                slug = source.Trim().ToLower();
                slug = slug.Replace(' ', '-');
                slug = slug.Replace("---", "-");
                slug = slug.Replace("--", "-");
                if (regex != null)
                    slug = regex.Replace(slug, "");

                if (slug.Length * 2 < source.Length)
                    return "";

                if (slug.Length > 100)
                    slug = slug.Substring(0, 100);
            }

            return slug;


        }
    }
}
