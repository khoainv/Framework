using SSD.Mappers.Interfaces;

namespace SSD.Mappers
{
    /// <summary>
    /// This class implements a scoped automapper configuration, so that a map created in one area
    /// is not accidentally picked up by another.
    /// The scope is defined by a the TBootstrap type parameter, which performs the mapping setup.
    /// This class also manages one-time configuration. Once setup has been done for a particular
    /// bootstrap, it won't be invoked again (per AppDomain).
    /// </summary>
    /// <typeparam name="TBootstrap">Automapper Bootstrap class that defines scope.</typeparam>
    public abstract class PrivateMapper<TBootstrap> where TBootstrap : PrivateMapperBootstrap, new()
    {
        #region Public methods

        /// <summary>
        /// Accessor for mapping engine which will carry out mappings. Use this property instead
        /// of the usual Mapper class.
        /// </summary>
        public static IMappingEngine Mapper
        {
            get
            {
                if (s_mapper == null)
                {
                    lock (SLockObject)
                    {
                        if (s_mapper == null)
                        {
                            s_mapper = InitialiseMapper();
                        }
                    }
                }

                return s_mapper;
            }
        }

        #endregion

        #region Private fields

        private static readonly object SLockObject = new object();

        private static IMappingEngine s_mapper;

        #endregion

        #region Private methods

        private static IMappingEngine InitialiseMapper()
        {

            var bootstrap = DependencyProvider.Current.Resolve<TBootstrap>();

            return AutoMapperMappingEngine.Create(bootstrap);
        }

        #endregion

#if DEBUG
        /// <summary>
        /// For testing, allow reset of mapper
        /// </summary>
        internal static void TestResetMapper()
        {
            s_mapper = null;
        }
#endif
    }
}