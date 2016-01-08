using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Threading;
using System.Xml;

namespace SSD.Framework.Extensions
{
    public static class TypeExtensions
    {
        public static List<Type> GetUGSubclassesOf(this Type type, string assemblyNameStart = "UG.")
        {
            List<Type> list = new List<Type>();
            IEnumerator enumerator = Thread.GetDomain().GetAssemblies().GetEnumerator();
            while (enumerator.MoveNext())
            {
                try
                {
                    Type[] types = ((Assembly)enumerator.Current).GetTypes();
                    if (((Assembly)enumerator.Current).FullName.StartsWith(assemblyNameStart))
                    {
                        IEnumerator enumerator2 = types.GetEnumerator();
                        while (enumerator2.MoveNext())
                        {
                            Type current = (Type)enumerator2.Current;
                            if (current.IsClass && !current.IsAbstract)
                            {
                                if (type.IsInterface)
                                {
                                    if (current.GetInterface(type.FullName) != null)
                                    {
                                        list.Add(current);
                                    }
                                }
                                else if (current.IsSubclassOf(type))
                                {
                                    list.Add(current);
                                }
                            }
                        }
                    }
                }
                catch
                {
                }
            }
            return list;
        }
        public static List<Type> GetSubclassesOf(this Type type, bool excludeSystemTypes=false)
        {
            List<Type> list = new List<Type>();
            IEnumerator enumerator = Thread.GetDomain().GetAssemblies().GetEnumerator();
            while (enumerator.MoveNext())
            {
                try
                {
                    Type[] types = ((Assembly)enumerator.Current).GetTypes();
                    if (!excludeSystemTypes || (excludeSystemTypes && !((Assembly)enumerator.Current).FullName.StartsWith("System.")))
                    {
                        IEnumerator enumerator2 = types.GetEnumerator();
                        while (enumerator2.MoveNext())
                        {
                            Type current = (Type)enumerator2.Current;
                            if (type.IsInterface)
                            {
                                if (current.GetInterface(type.FullName) != null)
                                {
                                    list.Add(current);
                                }
                            }
                            else if (current.IsSubclassOf(type))
                            {
                                list.Add(current);
                            }
                        }
                    }
                }
                catch
                {
                }
            }
            return list;
        }
    }
}
