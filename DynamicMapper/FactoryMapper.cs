using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynamicMapper
{
    /// <summary>
    /// 
    /// </summary>
    public class FactoryMapper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDestination"></typeparam>
        /// <returns></returns>
        public static ISourceMapper<TSource, TDestination> CreateMapper<TSource, TDestination>()
            where TSource: class
            where TDestination: class, new()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDestination"></typeparam>
        /// <returns></returns>
        public static ISourceMerger<TSource, TDestination> CreateMerger<TSource, TDestination>()
            where TSource : class
            where TDestination : class
        {
            throw new NotImplementedException();
        }
    }
}
