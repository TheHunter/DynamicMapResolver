using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynamicMapResolver.Impl
{
    /// <summary>
    /// 
    /// </summary>
    public class TransformerObserver
        : ITransformerObserver
    {
        private static readonly ITransformerObserver instance;
        private static readonly string defaultKey;
        private readonly HashSet<ServiceTransformer> mapperResolver;
        private readonly HashSet<ServiceTransformer> mergerResolver; 

        /// <summary>
        /// 
        /// </summary>
        static TransformerObserver()
        {
            instance = new TransformerObserver();
            defaultKey = "default";
        }

        /// <summary>
        /// 
        /// </summary>
        public TransformerObserver()
        {
            this.mapperResolver = new HashSet<ServiceTransformer>();
            this.mergerResolver = new HashSet<ServiceTransformer>();
        }

        /// <summary>
        /// Gets the default observer.
        /// </summary>
        /// <value>
        /// The default.
        /// </value>
        internal static ITransformerObserver Default { get { return instance; } }

        /// <summary>
        /// Tries to map the given instance into destination type.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="destinationType">Type of the destination.</param>
        /// <returns></returns>
        public object TryToMap(object source, Type destinationType)
        {
            return this.TryToMap(source, destinationType, defaultKey);
        }

        /// <summary>
        /// Tries to map the given instance into destination type using an specific key service.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="destinationType">Type of the destination.</param>
        /// <param name="keyService">The key service.</param>
        /// <returns></returns>
        public object TryToMap(object source, Type destinationType, object keyService)
        {
            if (source == null)
                return null;

            var serviceMapper = this.mapperResolver.FirstOrDefault(
                transformer => transformer.Match(keyService, source.GetType(), destinationType));

            if (serviceMapper == null)
                return null;

            ISourceMapper mapper = serviceMapper.ServiceAs<ISourceMapper>();
            return mapper.Map(source);
        }

        /// <summary>
        /// Tries to map the given instance into generic destination type.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TDestination">The type of the destination.</typeparam>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public TDestination TryToMap<TSource, TDestination>(TSource source)
        {
            return this.TryToMap<TSource, TDestination>(source, defaultKey);
        }

        /// <summary>
        /// Tries to map the given instance into generic destination type using an specific key service.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TDestination">The type of the destination.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="keyService">The key service.</param>
        /// <returns></returns>
        public TDestination TryToMap<TSource, TDestination>(TSource source, object keyService)
        {
            object src = source;
            if (src == null)
                return default(TDestination);

            var serviceMapper =
                this.mapperResolver.FirstOrDefault(
                    transformer => transformer.Match(keyService, typeof(TSource), typeof(TDestination)));

            if (serviceMapper == null)
                return default(TDestination);

            ISimpleMapper<TSource, TDestination> mapper = serviceMapper.ServiceAs<ISimpleMapper<TSource, TDestination>>();
            if (mapper != null)
                return mapper.Map(source);

            ISourceMapper mp = serviceMapper.ServiceAs<ISourceMapper>();
            if (mp == null)
                return default(TDestination);
            return (TDestination)mp.Map(source);
        }

        /// <summary>
        /// Tries to merge the given instance with the destination instance.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="destination">The destination.</param>
        /// <returns></returns>
        public object TryToMerge(object source, object destination)
        {
            return this.TryToMerge(source, destination, defaultKey);
        }

        /// <summary>
        /// Tries to merge the given instance with the destination instance using an specific key service.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="destination">The destination.</param>
        /// <param name="keyService">The key service.</param>
        /// <returns></returns>
        public object TryToMerge(object source, object destination, object keyService)
        {
            if (source == null || destination == null)
                return destination;

            var serviceMerger = this.mergerResolver.FirstOrDefault(
                transformer => transformer.Match(keyService, source.GetType(), destination.GetType()));

            if (serviceMerger == null)
                return destination;

            ISourceMerger merger = serviceMerger.ServiceAs<ISourceMerger>();
            return merger.Merge(source, destination);
        }

        /// <summary>
        /// Tries to merge the given instance with the destination instance.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TDestination">The type of the destination.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="destination">The destination.</param>
        /// <returns></returns>
        public TDestination TryToMerge<TSource, TDestination>(TSource source, TDestination destination)
        {
            return this.TryToMerge(source, destination, defaultKey);
        }

        /// <summary>
        /// given instance with the destination instance using an specific key service
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TDestination">The type of the destination.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="destination">The destination.</param>
        /// <param name="keyService">The key service.</param>
        /// <returns></returns>
        public TDestination TryToMerge<TSource, TDestination>(TSource source, TDestination destination, object keyService)
        {
            object src = source;
            object dst = destination;

            if (src == null || dst == null)
                return destination;

            var serviceMerger = this.mergerResolver.FirstOrDefault(
                transformer => transformer.Match(keyService, typeof(TSource), typeof(TDestination))
                );

            if (serviceMerger == null)
                return destination;

            ISourceMerger merger = serviceMerger.ServiceAs<ISourceMerger>();
            if (merger != null)
                merger.Merge(source, destination);

            return destination;
        }

        #region registration transformers

        /// <summary>
        /// Registers the mapper.
        /// </summary>
        /// <typeparam name="TMapper">The type of the mapper.</typeparam>
        /// <param name="mapper">The mapper.</param>
        /// <returns></returns>
        public bool RegisterMapper<TMapper>(TMapper mapper) where TMapper : class, ISourceMapper
        {
            return this.RegisterMapper(mapper, defaultKey);
        }

        /// <summary>
        /// Registers the mapper.
        /// </summary>
        /// <typeparam name="TMapper">The type of the mapper.</typeparam>
        /// <param name="mapper">The mapper.</param>
        /// <param name="keyService">The key service.</param>
        /// <returns></returns>
        public bool RegisterMapper<TMapper>(TMapper mapper, object keyService) where TMapper : class, ISourceMapper
        {
            if (mapper == null)
                return false;

            var service =
                this.mapperResolver.FirstOrDefault(
                    transformer =>
                    transformer.KeyEquals(keyService) && transformer.ComparableTo(keyService, mapper))
                    ;

            if (service != null)
                return false;

            return this.mapperResolver.Add(new ServiceTransformer<TMapper>(keyService, mapper));
        }

        /// <summary>
        /// Registers the merger.
        /// </summary>
        /// <typeparam name="TMerger">The type of the merger.</typeparam>
        /// <param name="merger">The merger.</param>
        /// <returns></returns>
        public bool RegisterMerger<TMerger>(TMerger merger) where TMerger : class, ISourceMerger
        {
            return this.RegisterMerger(merger, defaultKey);
        }

        /// <summary>
        /// Registers the merger.
        /// </summary>
        /// <typeparam name="TMerger">The type of the merger.</typeparam>
        /// <param name="merger">The merger.</param>
        /// <param name="keyService">The key service.</param>
        /// <returns></returns>
        public bool RegisterMerger<TMerger>(TMerger merger, object keyService) where TMerger : class, ISourceMerger
        {
            if (merger == null)
                return false;

            var service = this.mergerResolver.FirstOrDefault(
                transformer => transformer.KeyEquals(keyService) && transformer.ComparableTo(keyService, merger))
                ;

            if (service != null)
                return false;

            return this.mergerResolver.Add(new ServiceTransformer<TMerger>(keyService, merger));
        }

        #endregion

        #region ITransfomerInitializer

        /// <summary>
        /// Makes the transformer builder following the specified builder type.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TDestination">The type of the destination.</typeparam>
        /// <param name="type">The builder type to use.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException">Builder type not implemented by now.</exception>
        public ITransformerBuilder<TSource, TDestination> MakeTransformerBuilder<TSource, TDestination>(BuilderType type)
            where TSource: class
            where TDestination: class
        {
            switch (type)
            {
                case BuilderType.DefaultMappers:
                    return new TransformerBuilder<TSource, TDestination>(this, FactoryMapper.GetDefaultPropertyMappers<TSource, TDestination>());
                case BuilderType.Empty:
                    return new TransformerBuilder<TSource, TDestination>(this);
                case BuilderType.DynamicResolver:
                    return new TransformerBuilder<TSource, TDestination>(this, FactoryMapper.GetDefaultPropertyMappers<TSource, TDestination>(this));
                default:
                    throw new NotImplementedException("Builder type not implemented by now.");
            }
        }

        /// <summary>
        /// Makes the transformer builder with the given property mappers.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TDestination">The type of the destination.</typeparam>
        /// <param name="propertyMappers">The property mappers.</param>
        /// <returns></returns>
        public ITransformerBuilder<TSource, TDestination> MakeTransformerBuilder<TSource, TDestination>(IEnumerable<IPropertyMapper<TSource, TDestination>> propertyMappers)
            where TSource : class
            where TDestination : class
        {
            return new TransformerBuilder<TSource, TDestination>(this, propertyMappers);
        }
        
        #endregion

        #region IDynamicTransfomerBuilder

        /// <summary>
        /// Builds the automatic resolver mapper for compatibles properties.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TDestination">The type of the destination.</typeparam>
        /// <param name="beforeMapping">The before mapping.</param>
        /// <param name="afterMapping">The after mapping.</param>
        /// <returns></returns>
        public bool BuildAutoResolverMapper<TSource, TDestination>(Action<TDestination> beforeMapping, Action<TDestination> afterMapping)
            where TSource : class
            where TDestination : class
        {
            return this.BuildAutoResolverMapper<TSource, TDestination>(defaultKey, beforeMapping, afterMapping);
        }

        /// <summary>
        /// Builds the automatic resolver mapper for compatibles properties using the given key service for dedicated context and actions.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TDestination">The type of the destination.</typeparam>
        /// <param name="keyService">The key service.</param>
        /// <param name="beforeMapping">The before mapping.</param>
        /// <param name="afterMapping">The after mapping.</param>
        /// <returns></returns>
        public bool BuildAutoResolverMapper<TSource, TDestination>(object keyService, Action<TDestination> beforeMapping, Action<TDestination> afterMapping)
            where TSource : class
            where TDestination : class
        {
            ISourceMapper mapper =
                new SourceMapper<TSource, TDestination>(
                    FactoryMapper.GetDefaultPropertyMappers<TSource, TDestination>(this), beforeMapping, afterMapping);
            
            return this.RegisterMapper(mapper, keyService);
        }

        /// <summary>
        /// Builds the automatic resolver mapper for compatibles properties.
        /// </summary>
        /// <param name="sourceType">Type of the source.</param>
        /// <param name="destinationType">Type of the destination.</param>
        /// <returns></returns>
        public bool BuildAutoResolverMapper(Type sourceType, Type destinationType)
        {
            return this.BuildAutoResolverMapper(defaultKey, sourceType, destinationType);
        }

        /// <summary>
        /// Builds the automatic resolver mapper for compatibles properties using a service key for dedicated context.
        /// </summary>
        /// <param name="keyService">The key service.</param>
        /// <param name="sourceType">Type of the source.</param>
        /// <param name="destinationType">Type of the destination.</param>
        /// <returns></returns>
        public bool BuildAutoResolverMapper(object keyService, Type sourceType, Type destinationType)
        {
            ISourceMapper mapper = new SourceMapper(sourceType, destinationType,
                                                    FactoryMapper.GetDefaultPropertyMappers(sourceType, destinationType,
                                                                                            this));

            return this.RegisterMapper(mapper, keyService);
        }

        /// <summary>
        /// Builds the automatic resolver merger for compatibles properties.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TDestination">The type of the destination.</typeparam>
        /// <param name="beforeMapping">The before mapping.</param>
        /// <param name="afterMapping">The after mapping.</param>
        /// <returns></returns>
        public bool BuildAutoResolverMerger<TSource, TDestination>(Action<TDestination> beforeMapping, Action<TDestination> afterMapping)
            where TSource : class
            where TDestination : class
        {
            return this.BuildAutoResolverMerger<TSource, TDestination>(defaultKey, beforeMapping, afterMapping);
        }

        /// <summary>
        /// Builds the automatic resolver merger for compatibles properties using the given key service for dedicated context and actions.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TDestination">The type of the destination.</typeparam>
        /// <param name="keyService">The key service.</param>
        /// <param name="beforeMapping">The before mapping.</param>
        /// <param name="afterMapping">The after mapping.</param>
        /// <returns></returns>
        public bool BuildAutoResolverMerger<TSource, TDestination>(object keyService, Action<TDestination> beforeMapping, Action<TDestination> afterMapping)
            where TSource : class
            where TDestination : class
        {
            ISourceMerger merger =
                new SourceMerger<TSource, TDestination>(
                    FactoryMapper.GetDefaultPropertyMappers<TSource, TDestination>(this), beforeMapping, afterMapping);

            return this.RegisterMerger(merger, keyService);
        }

        /// <summary>
        /// Builds the automatic resolver merger for compatibles properties using a default key service context.
        /// </summary>
        /// <param name="sourceType">Type of the source.</param>
        /// <param name="destinationType">Type of the destination.</param>
        /// <returns></returns>
        public bool BuildAutoResolverMerger(Type sourceType, Type destinationType)
        {
            return this.BuildAutoResolverMerger(defaultKey, sourceType, destinationType);
        }

        /// <summary>
        /// Builds the automatic resolver merger for compatibles properties using the given key service for dedicated context.
        /// </summary>
        /// <param name="keyService">The key service.</param>
        /// <param name="sourceType">Type of the source.</param>
        /// <param name="destinationType">Type of the destination.</param>
        /// <returns></returns>
        public bool BuildAutoResolverMerger(object keyService, Type sourceType, Type destinationType)
        {
            ISourceMerger merger = new SourceMerger(sourceType, destinationType,
                                                    FactoryMapper.GetDefaultPropertyMappers(sourceType, destinationType,
                                                                                            this));

            return this.RegisterMerger(merger, keyService);
        }

        #endregion

        #region ITransformerObserver

        /// <summary>
        /// Retrieves the mapper.
        /// </summary>
        /// <typeparam name="TMapper">The type of the mapper.</typeparam>
        /// <param name="keyService">The key service.</param>
        /// <returns></returns>
        public TMapper RetrieveMapper<TMapper>(object keyService = null)
            where TMapper : class, ISourceMapper
        {
            var mapper = this.mapperResolver.FirstOrDefault(transformer => transformer.Match<TMapper>(keyService ?? defaultKey));
            return mapper == null ? null : mapper.ServiceAs<TMapper>();
        }

        /// <summary>
        /// Retrieves the merger.
        /// </summary>
        /// <typeparam name="TMerger">The type of the merger.</typeparam>
        /// <param name="keyService">The key service.</param>
        /// <returns></returns>
        public TMerger RetrieveMerger<TMerger>(object keyService = null)
            where TMerger : class, ISourceMerger
        {
            var merger = this.mergerResolver.FirstOrDefault(transformer => transformer.Match<TMerger>(keyService ?? defaultKey));
            return merger == null ? null : merger.ServiceAs<TMerger>();
        }
        
        #endregion
        
    }
}
