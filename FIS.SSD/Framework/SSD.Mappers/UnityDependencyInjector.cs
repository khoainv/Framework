using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using SSD.Mappers.Interfaces;

namespace SSD.Mappers
{
    internal sealed class UnityDependencyInjector : IDependencyInjector, IDisposable
    {
        private const string UnityContainerNameKey = "UnityContainerName";
        private readonly IUnityContainer _container;

        public UnityDependencyInjector()
        {
            this._container = (IUnityContainer)new UnityContainer();
            UnityConfigurationSection configurationSection = (UnityConfigurationSection)ConfigurationManager.GetSection("unity");
            string configuredContainerName = ConfigurationManager.AppSettings["UnityContainerName"];
            if (string.IsNullOrEmpty(configuredContainerName))
                configurationSection.Configure(this._container);
            else
                configurationSection.Configure(this._container, configuredContainerName);
        }

        public void Dispose()
        {
            if (this._container == null)
                return;
            this._container.Dispose();
        }

        public T Resolve<T>()
        {
            return Microsoft.Practices.Unity.UnityContainerExtensions.Resolve<T>(this._container, new ResolverOverride[0]);
        }

        public T Resolve<T>(string name, params IDependencyParameterOverride[] parameters)
        {
            if (!string.IsNullOrEmpty(name))
            {
                if (parameters == null)
                    return Microsoft.Practices.Unity.UnityContainerExtensions.Resolve<T>(this._container, name, new ResolverOverride[0]);
                ResolverOverride[] resolverOverrideArray = (ResolverOverride[])Enumerable.ToArray<ParameterOverride>(Enumerable.Select<IDependencyParameterOverride, ParameterOverride>((IEnumerable<IDependencyParameterOverride>)parameters, (Func<IDependencyParameterOverride, ParameterOverride>)(p => new ParameterOverride(p.ParameterName, p.ParameterValue))));
                return Microsoft.Practices.Unity.UnityContainerExtensions.Resolve<T>(this._container, name, resolverOverrideArray);
            }
            else if (parameters != null)
                return this.ResolveParametersOnly<T>(parameters);
            else
                return this.Resolve<T>();
        }

        public T Resolve<T>(params IDependencyParameterOverride[] parameters)
        {
            if (parameters != null)
                return this.ResolveParametersOnly<T>(parameters);
            else
                return this.Resolve<T>();
        }

        public T TryResolve<T>(Func<T> defaultFactory = null, params IDependencyParameterOverride[] parameters)
        {
            try
            {
                return this.Resolve<T>(parameters);
            }
            catch
            {
                return defaultFactory != null ? defaultFactory() : default(T);
            }
        }

        public T TryResolve<T>(string name, Func<T> defaultFactory = null, params IDependencyParameterOverride[] parameters)
        {
            try
            {
                return this.Resolve<T>(name, parameters);
            }
            catch
            {
                return defaultFactory != null ? defaultFactory() : default(T);
            }
        }

        private T ResolveParametersOnly<T>(params IDependencyParameterOverride[] parameters)
        {
            return Microsoft.Practices.Unity.UnityContainerExtensions.Resolve<T>(this._container, (ResolverOverride[])Enumerable.ToArray<ParameterOverride>(Enumerable.Select<IDependencyParameterOverride, ParameterOverride>((IEnumerable<IDependencyParameterOverride>)parameters, (Func<IDependencyParameterOverride, ParameterOverride>)(p => new ParameterOverride(p.ParameterName, p.ParameterValue)))));
        }

        internal IUnityContainer TestGetContainer()
        {
            return this._container;
        }
    }
}