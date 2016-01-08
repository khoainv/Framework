#region

using System.Reflection;

#endregion

namespace SSD.Framework.Extensions
{
    public static class PropertyExtensions
    {
        public static void SetPropertyValue(this object obj, string propName, object value)
        {
            PropertyInfo prop = obj.GetType().GetProperty(propName, BindingFlags.Public | BindingFlags.Instance);
            if (null != prop && prop.CanWrite)
            {
                prop.SetValue(obj, value, null);
            }
        }
        public static void SetPublicFieldValue(this object obj, string memberName, object value)
        {
            FieldInfo member = obj.GetType().GetField(memberName);
            if (null != member && member.IsPublic)
            {
                member.SetValue(obj, value);
            }
        }
        public static object GetPublicFieldValue(this object obj, string memberName)
        {
            FieldInfo member = obj.GetType().GetField(memberName);
            if (null != member && member.IsPublic)
            {
                return member.GetValue(obj);
            }
            return null;
        }
    }
}
