
using System;
using System.Reflection;
namespace SSD.Framework
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class EnumDescriptionAttribute : Attribute
    {
        private readonly string description;
        public string Description { get { return description; } }
        public EnumDescriptionAttribute(string description)
        {
            this.description = description;
        }
    }
    public partial class EnumHelper
    {
        public static string GetEnumDescription(object enumerator)
        {
            try
            {
                //get the enumerator type
                Type type = enumerator.GetType();

                //get the member info
                var memberInfo = type.GetRuntimeField(enumerator.ToString());

                //if there is member information
                if (memberInfo != null)
                {
                    //we default to the first member info, as it's for the specific enum value
                    var attribute = memberInfo.GetCustomAttribute<EnumDescriptionAttribute>();

                    //return the description if it's found
                    if (attribute != null)
                        return attribute.Description;
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