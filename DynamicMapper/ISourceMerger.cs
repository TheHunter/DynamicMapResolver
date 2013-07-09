using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynamicMapper
{
    /// <summary>
    /// A generic interface for merging object data source into object data destination.
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TDestination"></typeparam>
    public interface ISourceMerger<TSource, TDestination>
        : ISourceTransformer<TSource, TDestination>
        where TSource : class
        where TDestination : class
    {
        /// <summary>
        /// Merges the object data source into object data destination.
        /// </summary>
        /// <param name="source">Data source object type</param>
        /// <param name="destination">Data destination object type</param>
        /// <returns>returns a reference destination after merging with objectdata source.</returns>
        TDestination Merge(TSource source, TDestination destination);

        /// <summary>
        /// Merges the object data source into object data destination using a custom source property filter.
        /// </summary>
        /// <param name="source">Data source object type</param>
        /// <param name="destination">Data destination object type</param>
        /// <param name="filter">A set of source properties names which will be used for filtering property mappers</param>
        /// <returns>returns a reference destination after merging with objectdata source.</returns>
        TDestination Merge(TSource source, TDestination destination, IEnumerable<string> filter);
    }
}
