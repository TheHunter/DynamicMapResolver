using System;
using System.Linq;
using System.Collections.Generic;
using DynamicMapResolver.Exceptions;

namespace DynamicMapResolver.Impl
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
            : this(propertyMappers, typeof(TSource), typeof(TDestination), beforeMapping, afterMapping)
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyMappers"></param>
        /// <param name="sourceType"></param>
        /// <param name="destinationType"></param>
        /// <param name="beforeMapping"></param>
        /// <param name="afterMapping"></param>
        protected SourceMerger(IEnumerable<IPropertyMapper<TSource, TDestination>> propertyMappers, Type sourceType, Type destinationType,
                                Action<TDestination> beforeMapping, Action<TDestination> afterMapping)
            : base(propertyMappers, sourceType, destinationType, beforeMapping, afterMapping)
        {
            if (sourceType.IsPrimitive)
                throw new MapperParameterException("sourceType", "The source type cannot be a primitive type.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <returns></returns>
        public virtual TDestination Merge(TSource source, TDestination destination)
        {
            //this.OnMapping(source, destination, this.PropertyMappers);
            this.OnMapping(source, destination);
            return destination;
        }        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <returns></returns>
        object ISourceMerger.Merge(object source, object destination)
        {
            if (source == null || destination == null)
                return destination;

            return this.Merge(source as TSource, destination as TDestination);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj is SourceMerger<TSource, TDestination>)
                return this.GetHashCode() == obj.GetHashCode();

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return typeof(SourceMerger<TSource, TDestination>).GetHashCode() + base.GetHashCode();
        }
    }


    /// <summary>
    /// 
    /// </summary>
    public class SourceMerger
        : SourceMerger<object, object>
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceType"></param>
        /// <param name="destinationType"></param>
        /// <param name="propertyMappers"></param>
        internal SourceMerger(Type sourceType, Type destinationType, IEnumerable<IPropertyMapper> propertyMappers)
            : base(propertyMappers.Select<IPropertyMapper, IPropertyMapper<object, object>>(n => n), sourceType, destinationType, null, null)
        {
        }

    }
}
