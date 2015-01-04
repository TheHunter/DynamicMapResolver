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
        : ISourceTransformer<TSource, TDestination>, ISourceMerger
        where TSource : class
        where TDestination : class
    {
        /// <summary>
        /// Merges the specified source.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="destination">The destination.</param>
        /// <returns></returns>
        TDestination Merge(TSource source, TDestination destination);
    }

    /// <summary>
    /// An interface for merging object data source into object data destination.
    /// </summary>
    public interface ISourceMerger
        : ISourceTransformer
    {
        /// <summary>
        /// Merges the specified source.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="destination">The destination.</param>
        /// <returns></returns>
        object Merge(object source, object destination);
    }
}
