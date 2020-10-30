using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace SSD.Framework
{
    public interface IComboboxSource
    {
        string ValueMember { get; set; }
        string DisplayMember { get; set; }
    }
    public class DataDrop : IComboboxSource
    {
        public const string VALUE = "ValueMember";
        public const string TEXT = "DisplayMember";
        public string ValueMember
        {
            get;
            set;
        }

        public string DisplayMember
        {
            get;
            set;
        }
    }
    public partial class EnumHelper
    {
        public static IList<DataDrop> SelectListOf<TEnum>(bool empty = false)
        {
            var type = typeof(TEnum);
            if (type.IsEnum)
            {
                var list = Enum.GetValues(type)
                    .Cast<TEnum>()
                    .OrderBy(x => x)
                    .Select(x => new DataDrop() { DisplayMember = GetDescription(x), ValueMember = Convert.ToInt32(x) + string.Empty })
                    .ToList();

                if (empty)
                {
                    list.Insert(0, new DataDrop());
                }

                return list;

            }

            return new List<DataDrop>();
        }

        public static string GetDescription(object enumerator)
        {
            try
            {
                //get the enumerator type
                Type type = enumerator.GetType();

                //get the member info
                MemberInfo[] memberInfo = type.GetMember(enumerator.ToString());

                //if there is member information
                if (memberInfo != null && memberInfo.Length > 0)
                {
                    //we default to the first member info, as it's for the specific enum value
                    object[] attributes = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

                    //return the description if it's found
                    if (attributes != null && attributes.Length > 0)
                        return ((DescriptionAttribute)attributes[0]).Description;
                }

                //if there's no description, return the string value of the enum
                return enumerator.ToString();
            }
            catch (Exception e)
            {
                return string.Empty;
            }
        }
    }
}