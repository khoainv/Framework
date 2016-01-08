using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace SSD.Web.UI.Maps.Utils
{
    public static class WebUtils
    {
        private static MethodInfo _getWebResourceUrlMethod;
        private static object _getWebResourceUrlLock = new object();

        public static string GetWebResourceUrl<T>(string resourceName)
        {
            if (string.IsNullOrEmpty(resourceName))
            {
                throw new ArgumentNullException("resourceName");
            }

            if (_getWebResourceUrlMethod == null)
            {
                lock (_getWebResourceUrlLock)
                {
                    if (_getWebResourceUrlMethod == null)
                    {
                        //_getWebResourceUrlMethod = typeof(System.Web.Handlers.AssemblyResourceLoader).GetMethod(
                        //    "GetWebResourceUrlInternal",
                        //    BindingFlags.NonPublic | BindingFlags.Static);

                        // New code works with .NET 4.0 too
                        var methods = typeof(System.Web.Handlers.AssemblyResourceLoader).GetMethods(BindingFlags.NonPublic | BindingFlags.Static);
                        foreach (var method in methods)
                        {
                            if (method.Name == "GetWebResourceUrl" && method.GetParameters().Length == 3)
                            {
                                _getWebResourceUrlMethod = method;
                                break;
                            }
                        }
                    }
                }
            }

            //return "/" + (string)_getWebResourceUrlMethod.Invoke(null,
            //    new object[] { Assembly.GetAssembly(typeof(T)), resourceName, false });

            // New code works with .NET 4.0 too
            return (string)_getWebResourceUrlMethod.Invoke(null,
               new object[] { typeof(T), resourceName, false }); 
        }
    }
}
