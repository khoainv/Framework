using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace UG.Framework.Extensions
{
    public static class StringExtensions {
        public static string ListToString(this string s, List<string> lst, string delimiter)
        {
            foreach (string str in lst)
                s = s + delimiter + str;
            return s.Substring(1);
        }
        public static string RolesToString(this IList<string> lst, string delimiter)
        {
            string str = "(";
            foreach (string s in lst)
                str = s + delimiter;
            if (lst.Count > 0)
                str = str.Remove(str.Length - 1, 1);
            str = str + ")";
            return str;
        }
        public static string CreateSlug(this string source) {
            var regex = new Regex(@"([^a-z0-9\-]?)");
            string slug = "";

            if (!string.IsNullOrEmpty(source)) {
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

        public static string Truncate(this string s, int maxLength)
        {
            if (string.IsNullOrEmpty(s) || maxLength <= 0)
                return string.Empty;
            else if (s.Length > maxLength)
                return s.Substring(0, maxLength) + "...";
            else
                return s;
        }

        // IsNumeric Function
        public static bool IsNumeric(this string Expression)
        {
            // Variable to collect the Return value of the TryParse method.
            bool isNum;

            // Define variable to collect out parameter of the TryParse method. If the conversion fails, the out parameter is zero.
            double retNum;

            // The TryParse method converts a string in a specified style and culture-specific format to its double-precision floating point number equivalent.
            // The TryParse method does not generate an exception if the conversion fails. If the conversion passes, True is returned. If it does not, False is returned.
            isNum = Double.TryParse(Expression, System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out retNum);
            return isNum;
        }
    }
}
