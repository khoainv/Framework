#region

using System;

#endregion

namespace SSD.Framework.Extensions
{
    public static class DateExtensions
    {
        public static int GetQuarter(this DateTime dt)
        {
            return (dt.Month - 1) / 3 + 1;
        }
    }
}
