using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynamicMapResolver
{
    /// <summary>
    /// 
    /// </summary>
    public interface ITransformerResolver
        : ITransformerObserver
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TMapper"></typeparam>
        /// <returns></returns>
        bool IsMapperObserved<TMapper>() where TMapper : ISourceMapper;

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TMapper"></typeparam>
        /// <param name="keyService"></param>
        /// <returns></returns>
        bool IsMapperObserved<TMapper>(string keyService) where TMapper : ISourceMapper;

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TMerger"></typeparam>
        /// <returns></returns>
        bool IsMergerObserved<TMerger>() where TMerger : ISourceMerger;

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TMerger"></typeparam>
        /// <param name="keyService"></param>
        /// <returns></returns>
        bool IsMergerObserved<TMerger>(string keyService) where TMerger : ISourceMerger;

        #region using mappers

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        TDestination TryToMap<TSource, TDestination>(TSource source);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destinationType"></param>
        /// <returns></returns>
        object TryToMap(object source, Type destinationType);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="source"></param>
        /// <param name="keyService"></param>
        /// <returns></returns>
        TDestination TryToMap<TSource, TDestination>(TSource source, string keyService);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destinationType"></param>
        /// <param name="keyService"></param>
        /// <returns></returns>
        object TryToMap(object source, Type destinationType, string keyService);

        #endregion

        #region using mergers

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <returns></returns>
        TDestination TryToMerge<TSource, TDestination>(TSource source, TDestination destination);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <returns></returns>
        object TryToMerge(object source, object destination);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <param name="keyService"></param>
        /// <returns></returns>
        TDestination TryToMerge<TSource, TDestination>(TSource source, TDestination destination, string keyService);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <param name="keyService"></param>
        /// <returns></returns>
        object TryToMerge(object source, object destination, string keyService);

        #endregion

        
    }
}
