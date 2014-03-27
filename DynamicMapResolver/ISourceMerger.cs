using System;
using System.Collections.Generic;

namespace DynamicMapResolver
{
    /// <summary>
    /// A generic interface for merging object data source into object data destination.
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TDestination"></typeparam>
    public interface ISourceMerger<TSource, TDestination>
        //: ISourceTransformer<TSource, TDestination>, ISimpleMerger<TSource, TDestination>
        : ISourceTransformer<TSource, TDestination>, ISourceMerger
        where TSource : class
        where TDestination : class
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <returns></returns>
        TDestination Merge(TSource source, TDestination destination);

        
        //TDestination Merge(TSource source, TDestination destination, IEnumerable<string> filter);
    }

    /// <summary>
    /// 
    /// </summary>
    public interface ISourceMerger
        : ISourceTransformer
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <returns></returns>
        object Merge(object source, object destination);
    }
}
