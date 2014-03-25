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
    public class SourceMapper<TSource, TDestination>
        : SourceTransformer<TSource, TDestination>, ISourceMapper<TSource, TDestination>
        where TSource : class
        where TDestination : class
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyMappers"></param>
        /// <param name="beforeMapping"></param>
        /// <param name="afterMapping"></param>
        public SourceMapper(IEnumerable<IPropertyMapper<TSource, TDestination>> propertyMappers, Action<TDestination> beforeMapping, Action<TDestination> afterMapping)
            : this(propertyMappers, typeof (TSource), typeof (TDestination), beforeMapping, afterMapping)
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
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public virtual TDestination Map(TSource source)
        {
            if (source == null)
                return null;

            TDestination destination = Activator.CreateInstance(this.DestinationType, true) as TDestination;
            this.OnMapping(source, destination, this.PropertyMappers);
            return destination;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        object ISourceMapper.Map(object source)
        {
            if (source == null)
                return null;

            return this.Map(source as TSource);
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

            if (obj is SourceMapper<TSource, TDestination>)
                return this.GetHashCode() == obj.GetHashCode();

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
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
        /// 
        /// </summary>
        /// <param name="sourceType"></param>
        /// <param name="destinationType"></param>
        /// <param name="propertyMappers"></param>
        internal SourceMapper(Type sourceType, Type destinationType, IEnumerable<IPropertyMapper> propertyMappers)
            : base(propertyMappers.Select<IPropertyMapper, IPropertyMapper<object, object>>(n => n), sourceType, destinationType, null, null)
        {
        }

    }
}
