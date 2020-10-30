using AutoMapper;
using AutoMapper.Mappers;
using IMappingEngine = SSD.Mappers.Interfaces.IMappingEngine;

namespace SSD.Mappers
{
    internal class AutoMapperMappingEngine : IMappingEngine
    {
        #region Private Members
        /// <summary>
        /// Our AutoMapper engine instance
        /// </summary>
        private readonly AutoMapper.IMappingEngine _impl;

        #endregion

        #region Static Methods

        /// <summary>
        /// Creates a new AutoMapperMappingEngine using the specified bootstrap.
        /// </summary>
        /// <param name="bootstrap"></param>
        /// <returns></returns>
        public static IMappingEngine Create(PrivateMapperBootstrap bootstrap)
        {

            var config = new ConfigurationStore(new TypeMapFactory(), MapperRegistry.Mappers);

            bootstrap.CreateMaps(config);

            config.AssertConfigurationIsValid();

            return new AutoMapperMappingEngine(config);
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="configProvider"></param>
        private AutoMapperMappingEngine(IConfigurationProvider configProvider)
        {
            _impl = new MappingEngine(configProvider);
        }

        #endregion

        #region Public Methods
        /// <summary>
        /// Map an instance from source to destination types.
        /// </summary>
        /// <typeparam name="TSource">source type</typeparam>
        /// <typeparam name="TDestination">destination type</typeparam>
        /// <param name="source">source instance</param>
        /// <returns>destination instance</returns>
        public TDestination Map<TSource, TDestination>(TSource source)
        {
            return _impl.Map<TSource, TDestination>(source);
        }

        /// <summary>
        /// Map an instance from runtime type to destination type.
        /// </summary>
        /// <typeparam name="TDestination">destination type</typeparam>
        /// <param name="source">source instance</param>
        /// <returns>destination instance</returns>
        public TDestination Map<TDestination>(object source)
        {
            return _impl.Map<TDestination>(source);
        }

        #endregion
    }
}