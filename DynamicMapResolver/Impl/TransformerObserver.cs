﻿using System;
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
        /// 
        /// </summary>
        internal static ITransformerObserver Default { get { return instance; } }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destinationType"></param>
        /// <returns></returns>
        public object TryToMap(object source, Type destinationType)
        {
            return this.TryToMap(source, destinationType, defaultKey);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destinationType"></param>
        /// <param name="keyService"></param>
        /// <returns></returns>
        public object TryToMap(object source, Type destinationType, string keyService)
        {
            return this.TryToMap(source, destinationType, keyService as object);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destinationType"></param>
        /// <param name="keyService"></param>
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
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public TDestination TryToMap<TSource, TDestination>(TSource source)
        {
            return this.TryToMap<TSource, TDestination>(source, defaultKey);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="source"></param>
        /// <param name="keyService"></param>
        /// <returns></returns>
        public TDestination TryToMap<TSource, TDestination>(TSource source, string keyService)
        {
            return this.TryToMap<TSource, TDestination>(source, keyService as object);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="source"></param>
        /// <param name="keyService"></param>
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
            return mapper.Map(source);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <returns></returns>
        public object TryToMerge(object source, object destination)
        {
            return this.TryToMerge(source, destination, defaultKey);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <param name="keyService"></param>
        /// <returns></returns>
        public object TryToMerge(object source, object destination, string keyService)
        {
            return this.TryToMerge(source, destination, keyService as object);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <param name="keyService"></param>
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
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <returns></returns>
        public TDestination TryToMerge<TSource, TDestination>(TSource source, TDestination destination)
        {
            return this.TryToMerge(source, destination, defaultKey);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <param name="keyService"></param>
        /// <returns></returns>
        public TDestination TryToMerge<TSource, TDestination>(TSource source, TDestination destination, string keyService)
        {
            return this.TryToMerge(source, destination, keyService as object);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <param name="keyService"></param>
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
            merger.Merge(source, destination);
            return destination;
        }

        #region registration transformers

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TMapper"></typeparam>
        /// <param name="mapper"></param>
        /// <returns></returns>
        public bool RegisterMapper<TMapper>(TMapper mapper) where TMapper : class, ISourceMapper
        {
            return this.RegisterMapper(mapper, defaultKey);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TMapper"></typeparam>
        /// <param name="mapper"></param>
        /// <param name="keyService"></param>
        /// <returns></returns>
        public bool RegisterMapper<TMapper>(TMapper mapper, string keyService) where TMapper : class, ISourceMapper
        {
            return this.mapperResolver.Add(new ServiceTransformer<TMapper>(keyService, mapper));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TMapper"></typeparam>
        /// <param name="mapper"></param>
        /// <param name="keyService"></param>
        /// <returns></returns>
        public bool RegisterMapper<TMapper>(TMapper mapper, object keyService) where TMapper : class, ISourceMapper
        {   
            return this.mapperResolver.Add(new ServiceTransformer<TMapper>(keyService, mapper));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TMerger"></typeparam>
        /// <param name="merger"></param>
        /// <returns></returns>
        public bool RegisterMerger<TMerger>(TMerger merger) where TMerger : class, ISourceMerger
        {
            return this.RegisterMerger(merger, defaultKey);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TMerger"></typeparam>
        /// <param name="merger"></param>
        /// <param name="keyService"></param>
        /// <returns></returns>
        public bool RegisterMerger<TMerger>(TMerger merger, string keyService) where TMerger : class, ISourceMerger
        {
            return this.mergerResolver.Add(new ServiceTransformer<TMerger>(keyService, merger));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TMerger"></typeparam>
        /// <param name="merger"></param>
        /// <param name="keyService"></param>
        /// <returns></returns>
        public bool RegisterMerger<TMerger>(TMerger merger, object keyService) where TMerger : class, ISourceMerger
        {
            return this.mergerResolver.Add(new ServiceTransformer<TMerger>(keyService, merger));
        }

        #endregion

        #region

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
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
                default:
                    throw new NotImplementedException("Builder type not implemented.");
            }
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="propertyMappers"></param>
        /// <returns></returns>
        public ITransformerBuilder<TSource, TDestination> MakeTransformerBuilder<TSource, TDestination>(IEnumerable<IPropertyMapper<TSource, TDestination>> propertyMappers)
            where TSource : class
            where TDestination : class
        {
            return new TransformerBuilder<TSource, TDestination>(this, propertyMappers);
        }
        #endregion

    }
}