using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynamicMapResolver.Impl
{
    /// <summary>
    /// 
    /// </summary>
    public class TransformerResolver
        : ITransformerResolver
    {
        private static readonly ITransformerResolver instance;
        private static readonly string defaultKey;
        private readonly HashSet<ServiceType<ISourceMapper>> mapperResolver;
        private readonly HashSet<ServiceType<ISourceMerger>> mergerResolver; 

        /// <summary>
        /// 
        /// </summary>
        static TransformerResolver()
        {
            instance = new TransformerResolver();
            defaultKey = "default";
        }

        /// <summary>
        /// 
        /// </summary>
        public TransformerResolver()
        {
            this.mapperResolver = new HashSet<ServiceType<ISourceMapper>>();
            this.mergerResolver = new HashSet<ServiceType<ISourceMerger>>();
        }

        /// <summary>
        /// 
        /// </summary>
        public static ITransformerResolver Default { get { return instance; } }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TMapper"></typeparam>
        /// <param name="mapper"></param>
        /// <returns></returns>
        bool ITransformerObserver.ObserveMapper<TMapper>(TMapper mapper)
        {
            return this.mapperResolver.Add(new ServiceType<ISourceMapper>(defaultKey, mapper));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TMapper"></typeparam>
        /// <param name="mapper"></param>
        /// <param name="keyService"></param>
        /// <returns></returns>
        bool ITransformerObserver.ObserveMapper<TMapper>(TMapper mapper, string keyService)
        {
            return this.mapperResolver.Add(new ServiceType<ISourceMapper>(keyService, mapper));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TMerger"></typeparam>
        /// <param name="merger"></param>
        /// <returns></returns>
        bool ITransformerObserver.ObserveMerger<TMerger>(TMerger merger)
        {
            return this.mergerResolver.Add(new ServiceType<ISourceMerger>(defaultKey, merger));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TMerger"></typeparam>
        /// <param name="merger"></param>
        /// <param name="keyService"></param>
        /// <returns></returns>
        bool ITransformerObserver.ObserveMerger<TMerger>(TMerger merger, string keyService)
        {
            return this.mergerResolver.Add(new ServiceType<ISourceMerger>(keyService, merger));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TMapper"></typeparam>
        /// <returns></returns>
        public bool IsMapperObserved<TMapper>() where TMapper : ISourceMapper
        {
            return this.mapperResolver.Any(type => type.KeyType == typeof (TMapper) && type.Match(defaultKey));
        }

        public bool IsMapperObserved<TMapper>(string keyService) where TMapper : ISourceMapper
        {
            throw new NotImplementedException();
        }

        public bool IsMergerObserved<TMerger>() where TMerger : ISourceMerger
        {
            throw new NotImplementedException();
        }

        public bool IsMergerObserved<TMerger>(string keyService) where TMerger : ISourceMerger
        {
            throw new NotImplementedException();
        }

        public TDestination TryToMap<TSource, TDestination>(TSource source)
        {
            throw new NotImplementedException();
        }

        public object TryToMap(object source, Type destinationType)
        {
            throw new NotImplementedException();
        }

        public TDestination TryToMap<TSource, TDestination>(TSource source, string keyService)
        {
            throw new NotImplementedException();
        }

        public object TryToMap(object source, Type destinationType, string keyService)
        {
            throw new NotImplementedException();
        }

        public TDestination TryToMerge<TSource, TDestination>(TSource source, TDestination destination)
        {
            throw new NotImplementedException();
        }

        public object TryToMerge(object source, object destination)
        {
            throw new NotImplementedException();
        }

        public TDestination TryToMerge<TSource, TDestination>(TSource source, TDestination destination, string keyService)
        {
            throw new NotImplementedException();
        }

        public object TryToMerge(object source, object destination, string keyService)
        {
            throw new NotImplementedException();
        }
    }
}
