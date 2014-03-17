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
        : ISourceTransformer<TSource, TDestination>, ISimpleMerger<TSource, TDestination>
        where TSource : class
        where TDestination : class
    {
        /// <summary>
        /// Merges the object data source into object data destination using a custom source property filter.
        /// </summary>
        /// <param name="source">Data source object type</param>
        /// <param name="destination">Data destination object type</param>
        /// <param name="filter">A set of source properties names which will be used for filtering property mappers</param>
        /// <returns>returns a reference destination after merging with objectdata source.</returns>
        TDestination Merge(TSource source, TDestination destination, IEnumerable<string> filter);
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
