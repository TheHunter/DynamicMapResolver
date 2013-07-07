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
    public class SourceMapper<TSource, TDestination>
        : SourceTransformer<TSource, TDestination>, ISourceMapper<TSource, TDestination>
        where TSource : class
        where TDestination : class, new()
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyMappers"></param>
        /// <param name="beforeMapping"></param>
        /// <param name="afterMapping"></param>
        public SourceMapper(IEnumerable<IPropertyMapper<TSource, TDestination>> propertyMappers, Action<TDestination> beforeMapping, Action<TDestination> afterMapping)
            : base(propertyMappers, beforeMapping, afterMapping)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public TDestination Map(TSource source)
        {
            TDestination destination = new TDestination();
            this.OnMapping(source, destination, this.PropertyMappers);
            return destination;
        }

    }
}
