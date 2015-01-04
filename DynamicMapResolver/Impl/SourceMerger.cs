using System;
using System.Linq;
using System.Collections.Generic;
using DynamicMapResolver.Exceptions;

namespace DynamicMapResolver.Impl
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TSource">The type of the source.</typeparam>
    /// <typeparam name="TDestination">The type of the destination.</typeparam>
    public class SourceMerger<TSource, TDestination>
        : SourceTransformer<TSource, TDestination>, ISourceMerger<TSource, TDestination>
        where TSource : class
        where TDestination : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SourceMerger{TSource, TDestination}"/> class.
        /// </summary>
        /// <param name="propertyMappers">The property mappers.</param>
        /// <param name="beforeMapping">The before mapping.</param>
        /// <param name="afterMapping">The after mapping.</param>
        public SourceMerger(IEnumerable<IPropertyMapper<TSource, TDestination>> propertyMappers, Action<TDestination> beforeMapping, Action<TDestination> afterMapping)
            : this(propertyMappers, typeof(TSource), typeof(TDestination), beforeMapping, afterMapping)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SourceMerger{TSource, TDestination}"/> class.
        /// </summary>
        /// <param name="propertyMappers">The property mappers.</param>
        /// <param name="sourceType">Type of the source.</param>
        /// <param name="destinationType">Type of the destination.</param>
        /// <param name="beforeMapping">The before mapping.</param>
        /// <param name="afterMapping">The after mapping.</param>
        /// <exception cref="DynamicMapResolver.Exceptions.MapperParameterException">sourceType;The source type cannot be a primitive type.</exception>
        protected SourceMerger(IEnumerable<IPropertyMapper<TSource, TDestination>> propertyMappers, Type sourceType, Type destinationType,
                                Action<TDestination> beforeMapping, Action<TDestination> afterMapping)
            : base(propertyMappers, sourceType, destinationType, beforeMapping, afterMapping)
        {
            if (sourceType.IsPrimitive)
                throw new MapperParameterException("sourceType", "The source type cannot be a primitive type.");
        }

        /// <summary>
        /// / Merges the specified source./
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="destination">The destination.</param>
        /// <returns></returns>
        /// / / / /
        public virtual TDestination Merge(TSource source, TDestination destination)
        {
            //this.OnMapping(source, destination, this.PropertyMappers);
            this.OnMapping(source, destination);
            return destination;
        }

        /// <summary>
        /// Merges the specified source.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="destination">The destination.</param>
        /// <returns></returns>
        object ISourceMerger.Merge(object source, object destination)
        {
            if (source == null || destination == null)
                return destination;

            return this.Merge(source as TSource, destination as TDestination);
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj is SourceMerger<TSource, TDestination>)
                return this.GetHashCode() == obj.GetHashCode();

            return false;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
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
        /// Initializes a new instance of the <see cref="SourceMerger"/> class.
        /// </summary>
        /// <param name="sourceType">Type of the source.</param>
        /// <param name="destinationType">Type of the destination.</param>
        /// <param name="propertyMappers">The property mappers.</param>
        internal SourceMerger(Type sourceType, Type destinationType, IEnumerable<IPropertyMapper> propertyMappers)
            : base(propertyMappers.Select<IPropertyMapper, IPropertyMapper<object, object>>(n => n), sourceType, destinationType, null, null)
        {
        }

    }
}
