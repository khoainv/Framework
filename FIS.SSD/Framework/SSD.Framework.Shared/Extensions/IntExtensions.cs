#region

using System.Collections.Generic;

#endregion

namespace SSD.Framework.Extensions
{
    public static class LongExtensions
    {
        public static string ToStringN0(this long s)
        {
            return s.ToString("N0");
        }
    }
    public static class IntExtensions
    {
        public static string ToStringN0(this int s)
        {
            return s.ToString("N0");
        }
        public static string ListToString(this List<int> lst, string delimiter=",", string first = "", string last = "")
        {
            string str = first;
            foreach (int i in lst)
            {
                str = str + i + delimiter;
            }
            if (lst.Count > 0)
                str = str.Remove(str.Length - 1, 1);
            str = str + last;

            return str;
        }
    }
    public static class DecimalExtensions
    {
        public static string ToStringN0(this decimal s)
        {
            return s.ToString("N0");
        }
    }
}
