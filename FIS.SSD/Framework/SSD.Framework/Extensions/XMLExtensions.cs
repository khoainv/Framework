#region

using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Serialization;

#endregion

namespace SSD.Framework.Extensions
{
    public static class XMLExtensions
    {
        public static void CheckNull<T>(this T obj, string parameterName)
        where T : class
        {
            if (obj == null) throw new ArgumentNullException(parameterName);
        }
        /// <summary>
        /// Extension method that takes objects and serialized them.
        /// </summary>
        /// <typeparam name="T">The type of the object to be serialized.</typeparam>
        /// <param name="source">The object to be serialized.</param>
        /// <returns>A string that represents the serialized XML.</returns>
        public static string SerializeXML<T>(this T source) where T : class, new()
        {
            source.CheckNull("Object to be serialized.");

            //Loi trong truong hop Object in Object tren Windows 2008 Server => Su dung phuong phap DataContractSerializer
            //var serializer = new XmlSerializer(typeof(T));
            //using (var writer = new StringWriter())
            //{
            //    serializer.Serialize(writer, source);
            //    return writer.ToString();
            //}
            var serializer = new DataContractSerializer(source.GetType());
            using (var writer = new StringWriter())
            using (var stm = new XmlTextWriter(writer))
            {
                serializer.WriteObject(stm, source);
                return writer.ToString();
            }
        }
        public static string SerializeXML(this object source, Type type)
        {
            source.CheckNull("Object to be serialized.");

            //Loi trong truong hop Object in Object tren Windows 2008 Server => Su dung phuong phap DataContractSerializer
            //var serializer = new XmlSerializer(type);
            //using (var writer = new StringWriter())
            //{
            //    serializer.Serialize(writer, source);
            //    return writer.ToString();
            //}
            var serializer = new DataContractSerializer(type);
            using (var writer = new StringWriter())
            using (var stm = new XmlTextWriter(writer))
            {
                serializer.WriteObject(stm, source);
                return writer.ToString();
            }
        }

        /// <summary>
        /// Extension method to string which attempts to deserialize XML with the same name as the source string.
        /// </summary>
        /// <typeparam name="T">The type which to be deserialized to.</typeparam>
        /// <param name="XML">The source string</param>
        /// <returns>The deserialized object, or null if unsuccessful.</returns>
        public static T DeserializeXML<T>(this string XML) where T : class, new()
        {
            XML.CheckNull("XML-string");

            var serializer = new DataContractSerializer(typeof(T));
            using (var reader = new StringReader(XML))
            using (var stm = new XmlTextReader(reader))
            {
                try { return (T)serializer.ReadObject(stm); }
                catch//Doc dinh dang XML cu
                {
                    var serializer1 = new XmlSerializer(typeof(T));
                    using (var reader1 = new StringReader(XML))
                    {
                        try { return (T)serializer1.Deserialize(reader1); }
                        catch { return null; } // Could not be deserialized to this type.
                    }
                }
            }
        }

        public static object DeserializeXML(this string XML, Type type)
        {
            XML.CheckNull("XML-string");

            var serializer = new DataContractSerializer(type);
            using (var reader = new StringReader(XML))
            using (var stm = new XmlTextReader(reader))
            {
                try { return serializer.ReadObject(stm); }
                catch//Doc dinh dang XML cu
                {
                    var serializer1 = new XmlSerializer(type);
                    using (var reader1 = new StringReader(XML))
                    {
                        try { return serializer1.Deserialize(reader1); }
                        catch { return null; } // Could not be deserialized to this type.
                    }
                }
            }
        }
    }
}
