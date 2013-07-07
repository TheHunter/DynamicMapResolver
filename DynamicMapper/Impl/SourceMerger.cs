using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DynamicMapper.Exceptions;

namespace DynamicMapper.Impl
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TDestination"></typeparam>
    public class SourceMerger<TSource, TDestination>
        : SourceTransformer<TSource, TDestination>, ISourceMerger<TSource, TDestination>
        where TSource : class
        where TDestination : class
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyMappers"></param>
        /// <param name="beforeMapping"></param>
        /// <param name="afterMapping"></param>
        public SourceMerger(IEnumerable<IPropertyMapper<TSource, TDestination>> propertyMappers, Action<TDestination> beforeMapping, Action<TDestination> afterMapping)
            : base(propertyMappers, beforeMapping, afterMapping)
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <returns></returns>
        public TDestination Merge(TSource source, TDestination destination)
        {
            this.OnMapping(source, destination, this.PropertyMappers);
            return destination;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <param name="filters"></param>
        /// <returns></returns>
        public TDestination Merge(TSource source, TDestination destination, IEnumerable<string> filters)
        {
            IEnumerable<IPropertyMapper<TSource, TDestination>> mappers
                = filters != null
                    ? PropertyMappers.Where(n => filters.Contains(n.PropertySource))
                    : PropertyMappers;

            this.OnMapping(source, destination, mappers);
            return destination;
        }
    }
}
