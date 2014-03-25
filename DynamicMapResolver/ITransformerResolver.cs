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
    {
        
        #region using mappers

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
        /// <param name="source"></param>
        /// <param name="destinationType"></param>
        /// <param name="keyService"></param>
        /// <returns></returns>
        object TryToMap(object source, Type destinationType, string keyService);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destinationType"></param>
        /// <param name="keyService"></param>
        /// <returns></returns>
        object TryToMap(object source, Type destinationType, object keyService);

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
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="source"></param>
        /// <param name="keyService"></param>
        /// <returns></returns>
        TDestination TryToMap<TSource, TDestination>(TSource source, string keyService);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="source"></param>
        /// <param name="keyService"></param>
        /// <returns></returns>
        TDestination TryToMap<TSource, TDestination>(TSource source, object keyService);

        #endregion

        #region using mergers

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
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <param name="keyService"></param>
        /// <returns></returns>
        object TryToMerge(object source, object destination, string keyService);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <param name="keyService"></param>
        /// <returns></returns>
        object TryToMerge(object source, object destination, object keyService);

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
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <param name="keyService"></param>
        /// <returns></returns>
        TDestination TryToMerge<TSource, TDestination>(TSource source, TDestination destination, object keyService);

        #endregion

        
    }
}
