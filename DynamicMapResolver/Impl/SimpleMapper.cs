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
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TDestination"></typeparam>
    public class SimpleMapper<TSource, TDestination>
        : SourceTransformer, ISimpleMapper<TSource, TDestination>
    {
        private readonly Func<TSource, TDestination> converter;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="converter"></param>
        public SimpleMapper(Func<TSource, TDestination> converter)
            :base(typeof(TSource), typeof(TDestination))
        {
            if (converter == null)
                throw new MapperParameterException("converter", "The given lambda converter cannot be null.");

            this.converter = converter;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
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
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        object ISourceMapper.Map(object source)
        {
            if (source is TSource)
                return this.Map((TSource)source);

            throw new MapperParameterException("source", string.Format("Error before executing the Map method from ISimpleTransformer instance, caused by incorrect source type (of <{0}>), instead It's expected <{1}>", source.GetType().Name, typeof(TSource).Name));
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

            if (obj is SimpleMapper<TSource, TDestination>)
                return this.GetHashCode() == obj.GetHashCode();

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return typeof(SimpleMapper<TSource, TDestination>).GetHashCode() + base.GetHashCode();
        }
    }
}
