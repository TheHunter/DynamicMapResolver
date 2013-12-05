using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynamicMapResolver
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TDestination"></typeparam>
    public interface IContainerBuilder<TSource, TDestination>
        where TSource : class
        where TDestination : class
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        ISourceMapper<TSource, TDestination> BuildMapper();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        ISourceMerger<TSource, TDestination> BuildMerger();
    }
}
