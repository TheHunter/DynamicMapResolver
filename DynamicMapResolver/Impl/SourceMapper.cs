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
            : base(propertyMappers, beforeMapping, afterMapping)
        {
            try
            {
                // verify if the destination type is a valid type.
                Activator.CreateInstance(typeof (TDestination), true);
            }
            catch (Exception ex)
            {
                throw new MapperParameterException("tDestination", "The destination type is not valid, see inner exception for details", ex);
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

            TDestination destination = Activator.CreateInstance(typeof(TDestination), true) as TDestination;
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
    }

    /// <summary>
    /// 
    /// </summary>
    public class SourceMapper
        : SourceMapper<object, object>
    {
        private readonly Type tSource;
        private readonly Type tDestination;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tSource"></param>
        /// <param name="tDestination"></param>
        /// <param name="propertyMappers"></param>
        internal SourceMapper(Type tSource, Type tDestination, IEnumerable<IPropertyMapper> propertyMappers)
            : base(propertyMappers.Select<IPropertyMapper, IPropertyMapper<object, object>>(n => n), null, null)
        {
            if (tSource == null)
                throw new MapperParameterException("tSource", "The source type cannot be null.");

            if (tDestination == null)
                throw new MapperParameterException("tDestination", "The destination type cannot be null.");

            if (tSource.IsPrimitive)
                throw new MapperParameterException("tSource", "The source type cannot be a primitive type.");

            try
            {
                // verify if the destination type is a valid type.
                Activator.CreateInstance(tDestination, true);
            }
            catch (Exception ex)
            {
                throw new MapperParameterException("tDestination", "The destination type is not valid, see inner exception for details", ex);
            }

            this.tSource = tSource;
            this.tDestination = tDestination;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public override object Map(object source)
        {
            if (source == null)
                return null;

            if (!tSource.IsInstanceOfType(source))
                return null;

            object destination = Activator.CreateInstance(tDestination, true);
            this.OnMapping(source, destination, this.PropertyMappers);
            return destination;
        }
    }
}
