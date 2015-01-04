using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DynamicMapResolver.Exceptions;

namespace DynamicMapResolver.Impl
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TSource">The type of the source.</typeparam>
    /// <typeparam name="TDestination">The type of the destination.</typeparam>
    public class SimpleMapper<TSource, TDestination>
        : SourceTransformer, ISimpleMapper<TSource, TDestination>
    {
        private readonly Func<TSource, TDestination> converter;

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleMapper{TSource, TDestination}"/> class.
        /// </summary>
        /// <param name="converter">The converter.</param>
        /// <exception cref="DynamicMapResolver.Exceptions.MapperParameterException">converter;The given lambda converter cannot be null.</exception>
        public SimpleMapper(Func<TSource, TDestination> converter)
            :base(typeof(TSource), typeof(TDestination))
        {
            if (converter == null)
                throw new MapperParameterException("converter", "The given lambda converter cannot be null.");

            this.converter = converter;
        }

        /// <summary>
        /// Maps the specified source.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        /// <exception cref="DynamicMapResolver.Exceptions.MapperException">Error on executing the expression for transforming source instance into destination type.</exception>
        public TDestination Map(TSource source)
        {
            try
            {
                return this.converter.Invoke(source);
            }
            catch (Exception ex)
            {
                if (this.IgnoreExceptionOnMapping)
                    return default(TDestination);

                throw new MapperException("Error on executing the expression for transforming source instance into destination type.", ex);
            }
            
        }

        /// <summary>
        /// Transform the given instance into destination type.
        /// </summary>
        /// <param name="source">the instance to transform.</param>
        /// <returns></returns>
        /// <exception cref="DynamicMapResolver.Exceptions.MapperParameterException">source</exception>
        object ISourceMapper.Map(object source)
        {
            if (source is TSource)
                return this.Map((TSource)source);

            throw new MapperParameterException("source", string.Format("Error before executing the Map method from ISimpleTransformer instance, caused by incorrect source type (of <{0}>), instead It's expected <{1}>", source.GetType().Name, typeof(TSource).Name));
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

            if (obj is SimpleMapper<TSource, TDestination>)
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
            return typeof(SimpleMapper<TSource, TDestination>).GetHashCode() + base.GetHashCode();
        }
    }
}
