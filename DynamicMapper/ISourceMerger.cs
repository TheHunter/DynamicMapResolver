using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynamicMapper
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TDestination"></typeparam>
    public interface ISourceMerger<TSource, TDestination>
        : ISourceTransformer<TSource, TDestination>
        where TSource : class
        where TDestination : class
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="source"></param>
        TDestination Merge(TSource source, TDestination destination);
    }
}
