using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynamicMapper.Impl
{
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


        public TDestination Merge(TSource source, TDestination destination)
        {
            this.OnMapping(source, destination);
            return destination;
        }
    }
}
