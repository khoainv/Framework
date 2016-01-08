using System;

namespace SSD.Mappers.Interfaces
{
    public interface IDependencyInjector
    {
        T Resolve<T>();

        T Resolve<T>(params IDependencyParameterOverride[] parameters);

        T Resolve<T>(string name, params IDependencyParameterOverride[] parameters);

        T TryResolve<T>(Func<T> defaultFactory, params IDependencyParameterOverride[] parameters);

        T TryResolve<T>(string name, Func<T> defaultFactory, params IDependencyParameterOverride[] parameters);
    }
}