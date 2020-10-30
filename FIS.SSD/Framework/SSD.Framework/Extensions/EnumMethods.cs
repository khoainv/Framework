#region

using System;
using System.ComponentModel;
using System.Reflection;

#endregion

namespace SSD.Framework.Extensions
{
    public static class EnumMethods
    {
        #region Extended Methods For Enum type

        /// <summary>
        /// Author: MarkNguyen
        /// Created on: 24/02/2014 19:36
        /// Description: To the unique identifier.
        /// </summary>
        /// <typeparam name="TEnum">The type of the t enum.</typeparam>
        /// <param name="enumValue">The enum value.</param>
        /// <returns>Guid.</returns>
        public static Guid ToGuid<TEnum>(this TEnum enumValue) where TEnum : struct
        {
            Guid output = Guid.Empty;
            Type type = enumValue.GetType();

            FieldInfo fi = type.GetField(enumValue.ToString());
            var attrs =
               fi.GetCustomAttributes(typeof(EnumGuidAttribute),
                                       false) as EnumGuidAttribute[];
            if (attrs != null && attrs.Length > 0)
            {
                output = attrs[0].Value;
            }

            return output;
        }

        /// <summary>
        /// Author: MarkNguyen
        /// Created on: 24/02/2014 19:36
        /// Description: Descriptions the specified enum value.
        /// </summary>
        /// <typeparam name="TEnum">The type of the t enum.</typeparam>
        /// <param name="enumValue">The enum value.</param>
        /// <returns>System.String.</returns>
        public static string Description<TEnum>(this TEnum enumValue) where TEnum : struct
        {
            string output = null;
            Type type = enumValue.GetType();

            FieldInfo fi = type.GetField(enumValue.ToString());
            var attrs =
               fi.GetCustomAttributes(typeof(DescriptionAttribute),
                                       false) as DescriptionAttribute[];
            if (attrs != null && attrs.Length > 0)
            {
                output = attrs[0].Description;
            }

            return output;
        }

        public static int ToInt(this Enum enumValue)
        {
            return Convert.ToInt32(enumValue);
        } 
        public static short ToShort(this Enum enumValue)
        {
            return Convert.ToInt16(enumValue);
        } 
        public static Byte ToByte(this Enum enumValue)
        {
            return Convert.ToByte(enumValue);
        } 
        #endregion
    }
}