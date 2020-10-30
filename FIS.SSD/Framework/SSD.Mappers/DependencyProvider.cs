using System;
using System.Configuration;
using SSD.Mappers.Interfaces;

namespace SSD.Mappers
{
    public static class DependencyProvider
    {
        private static readonly object SInjectorLock = new object();
        private const string DependencyInjectorImplAssemblyKey = "DependencyInjectorImplAssembly";
        private const string DependencyInjectorImplTypeKey = "DependencyInjectorImplType";
        private static IDependencyInjector s_injector;

        public static IDependencyInjector Current
        {
            get
            {
                if (DependencyProvider.s_injector == null)
                {
                    lock (DependencyProvider.SInjectorLock)
                    {
                        if (DependencyProvider.s_injector == null)
                        {
                            string local_0 = typeof(UnityDependencyInjector).Assembly.FullName;
                            string local_1 = typeof(UnityDependencyInjector).FullName;
                            string local_2 = ConfigurationManager.AppSettings["DependencyInjectorImplAssembly"];
                            string local_3 = ConfigurationManager.AppSettings["DependencyInjectorImplType"];
                            DependencyProvider.s_injector = (IDependencyInjector)AppDomain.CurrentDomain.CreateInstanceAndUnwrap(string.IsNullOrEmpty(local_2) ? local_0 : local_2, string.IsNullOrEmpty(local_3) ? local_1 : local_3);
                        }
                    }
                }
                return DependencyProvider.s_injector;
            }
        }

        static DependencyProvider()
        {
        }

        internal static void TestResetCurrent()
        {
            DependencyProvider.s_injector = (IDependencyInjector)null;
        }
    }
}