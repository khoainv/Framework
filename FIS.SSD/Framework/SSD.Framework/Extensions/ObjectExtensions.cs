using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace SSD.Framework.Extensions
{
    public static class ObjectExtensions
    {
        /// <summary>
        /// Returns an Object with the specified Type and whose value is equivalent to the specified object.
        /// </summary>
        /// </remarks>
        public static object ChangeType(this object value, Type conversionType)
        {
            // Note: This if block was taken from Convert.ChangeType as is, and is needed here since we're
            // checking properties on conversionType below.
            if (conversionType == null)
                throw new ArgumentNullException("conversionType");

            // If it's not a nullable type, just pass through the parameters to Convert.ChangeType

            if (conversionType.IsGenericType && conversionType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                if (value == null)
                    return null;
                NullableConverter nullableConverter = new NullableConverter(conversionType);
                conversionType = nullableConverter.UnderlyingType;
            }

            // Now that we've guaranteed conversionType is something Convert.ChangeType can handle (i.e. not a
            // nullable type), pass the call on to Convert.ChangeType
            return System.Convert.ChangeType(value, conversionType);
        }

        public static byte[] ToBinary<T>(this T o) where T : class, new()
        {
            byte[] bytes = null;
            DataContractSerializer dc = new DataContractSerializer(typeof(T));

            using (MemoryStream ms = new MemoryStream())
            {
                //formatter.Serialize(ms, value);
                dc.WriteObject(ms, o);
                ms.Seek(0, 0);
                bytes = ms.ToArray();
            }

            return bytes;
        }

        public static TResult FromBinary<TResult>(this TResult input, byte[] bits) where TResult : class, new()
        {
            TResult result = default(TResult);
            DataContractSerializer dc = new DataContractSerializer(typeof(TResult));
            //IFormatter formatter = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream(bits))
            {
                result = (TResult)dc.ReadObject(ms);
            }

            return result;
        }

        //Creates an object from an XML string.
        public static TResult FromXml<TResult>(this TResult input, string Xml) where TResult : class, new()
        {
            TResult result = default(TResult);
            DataContractSerializer dc = new DataContractSerializer(typeof(TResult));
            using (StringReader str = new StringReader(Xml))
            {
                using (XmlTextReader xml = new XmlTextReader(str))
                {
                    result = (TResult)dc.ReadObject(xml);
                }
            }
            return result;
        }

        //Serializes the <i>Obj</i> to an XML string.
        public static string ToXml<T>(this T o) where T : class, new()
        {
            System.Xml.XmlDocument doc = new XmlDocument();
            DataContractSerializer dc = new DataContractSerializer(typeof(T));
            using (MemoryStream ms = new MemoryStream())
            {
                dc.WriteObject(ms, o);
                ms.Position = 0;
                doc.Load(ms);
                return doc.InnerXml;
            }
        }        
    }
}
