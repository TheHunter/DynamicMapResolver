using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynamicMapResolver
{
    /// <summary>
    /// 
    /// </summary>
    public interface IDynamicTransformerBuilder
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="beforeMapping"></param>
        /// <param name="afterMapping"></param>
        /// <returns></returns>
        bool BuildAutoResolverMapper<TSource, TDestination>(Action<TDestination> beforeMapping,
                                                            Action<TDestination> afterMapping)
            where TSource : class
            where TDestination : class;

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="keyService"></param>
        /// <param name="beforeMapping"></param>
        /// <param name="afterMapping"></param>
        /// <returns></returns>
        bool BuildAutoResolverMapper<TSource, TDestination>(object keyService, Action<TDestination> beforeMapping,
                                                            Action<TDestination> afterMapping)
            where TSource : class
            where TDestination : class;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceType"></param>
        /// <param name="destinationType"></param>
        /// <returns></returns>
        bool BuildAutoResolverMapper(Type sourceType, Type destinationType);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keyService"></param>
        /// <param name="sourceType"></param>
        /// <param name="destinationType"></param>
        /// <returns></returns>
        bool BuildAutoResolverMapper(object keyService, Type sourceType, Type destinationType);



        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="beforeMapping"></param>
        /// <param name="afterMapping"></param>
        /// <returns></returns>
        bool BuildAutoResolverMerger<TSource, TDestination>(Action<TDestination> beforeMapping,
                                                            Action<TDestination> afterMapping)
            where TSource : class
            where TDestination : class;

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="keyService"></param>
        /// <param name="beforeMapping"></param>
        /// <param name="afterMapping"></param>
        /// <returns></returns>
        bool BuildAutoResolverMerger<TSource, TDestination>(object keyService, Action<TDestination> beforeMapping,
                                                            Action<TDestination> afterMapping)
            where TSource : class
            where TDestination : class;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceType"></param>
        /// <param name="destinationType"></param>
        /// <returns></returns>
        bool BuildAutoResolverMerger(Type sourceType, Type destinationType);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keyService"></param>
        /// <param name="sourceType"></param>
        /// <param name="destinationType"></param>
        /// <returns></returns>
        bool BuildAutoResolverMerger(object keyService, Type sourceType, Type destinationType);
    }
}
