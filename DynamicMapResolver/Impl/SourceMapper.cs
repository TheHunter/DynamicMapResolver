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
    public class SourceMapper<TSource, TDestination>
        : SourceTransformer<TSource, TDestination>, ISourceMapper<TSource, TDestination>
        where TSource : class
        where TDestination : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SourceMapper{TSource, TDestination}"/> class.
        /// </summary>
        /// <param name="propertyMappers">The property mappers.</param>
        /// <param name="beforeMapping">The before mapping.</param>
        /// <param name="afterMapping">The after mapping.</param>
        public SourceMapper(IEnumerable<IPropertyMapper<TSource, TDestination>> propertyMappers, Action<TDestination> beforeMapping, Action<TDestination> afterMapping)
            : this(propertyMappers, typeof(TSource), typeof (TDestination), beforeMapping, afterMapping)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SourceMapper{TSource, TDestination}"/> class.
        /// </summary>
        /// <param name="propertyMappers">The property mappers.</param>
        /// <param name="sourceType">Type of the source.</param>
        /// <param name="destinationType">Type of the destination.</param>
        /// <param name="beforeMapping">The before mapping.</param>
        /// <param name="afterMapping">The after mapping.</param>
        /// <exception cref="DynamicMapResolver.Exceptions.MapperParameterException">
        /// sourceType;The source type cannot be a primitive type.
        /// or
        /// destinationType;The destination type is not valid, see inner exception for details
        /// </exception>
        protected SourceMapper(IEnumerable<IPropertyMapper<TSource, TDestination>> propertyMappers, Type sourceType, Type destinationType,
                                Action<TDestination> beforeMapping, Action<TDestination> afterMapping)
            : base(propertyMappers, sourceType, destinationType, beforeMapping, afterMapping)
        {

            if (sourceType.IsPrimitive)
                throw new MapperParameterException("sourceType", "The source type cannot be a primitive type.");

            try
            {
                // verify if the destination type is a valid type.
                Activator.CreateInstance(destinationType, true);
            }
            catch (Exception ex)
            {
                throw new MapperParameterException("destinationType", "The destination type is not valid, see inner exception for details", ex);
            }
        }

        /// <summary>
        /// Maps the specified source.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public virtual TDestination Map(TSource source)
        {
            if (source == null)
                return null;

            TDestination destination = Activator.CreateInstance(this.DestinationType, true) as TDestination;
            //this.OnMapping(source, destination, this.PropertyMappers);
            this.OnMapping(source, destination);
            return destination;
        }

        /// <summary>
        /// Transform the given instance into destination type.
        /// </summary>
        /// <param name="source">the instance to transform.</param>
        /// <returns></returns>
        object ISourceMapper.Map(object source)
        {
            if (source == null)
                return null;

            return this.Map(source as TSource);
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

            if (obj is SourceMapper<TSource, TDestination>)
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
            return typeof(SourceMapper<TSource, TDestination>).GetHashCode() + base.GetHashCode();
        }
    }


    /// <summary>
    /// 
    /// </summary>
    public class SourceMapper
        : SourceMapper<object, object>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SourceMapper"/> class.
        /// </summary>
        /// <param name="sourceType">Type of the source.</param>
        /// <param name="destinationType">Type of the destination.</param>
        /// <param name="propertyMappers">The property mappers.</param>
        internal SourceMapper(Type sourceType, Type destinationType, IEnumerable<IPropertyMapper> propertyMappers)
            : base(propertyMappers.Select<IPropertyMapper, IPropertyMapper<object, object>>(n => n), sourceType, destinationType, null, null)
        {
        }

    }
}
