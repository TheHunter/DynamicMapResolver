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
       : ISimpleMapper<TSource, TDestination>
    {
        private readonly Type sourceType;
        private readonly Type destinationType;
        private readonly Action<TSource, TDestination> converter;
        private readonly Action<TDestination> beforeMapping;
        private readonly Action<TDestination> afterMapping;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="converter"></param>
        /// <param name="beforeMapping"></param>
        /// <param name="afterMapping"></param>
        public SimpleMapper(Action<TSource, TDestination> converter, Action<TDestination> beforeMapping, Action<TDestination> afterMapping)
        {
            if (converter == null)
                throw new MapperParameterException("converter", "The given lambda converter cannot be null.");

            this.converter = converter;
            this.sourceType = typeof (TSource);
            this.destinationType = typeof (TDestination);
            this.beforeMapping = beforeMapping;
            this.afterMapping = afterMapping;
        }

        /// <summary>
        /// 
        /// </summary>
        public Type SourceType
        {
            get { return sourceType; }
        }

        /// <summary>
        /// 
        /// </summary>
        public Type DestinationType
        {
            get { return destinationType; }
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
                TDestination dest = (TDestination)Activator.CreateInstance(this.DestinationType, true);
                if (this.beforeMapping != null)
                    this.beforeMapping(dest);
                
                this.converter.Invoke(source, dest);

                if (this.afterMapping != null)
                    this.afterMapping(dest);
                
                return dest;
            }
            catch (Exception ex)
            {
                throw new MappingFailedActionException(string.Format("Error on executing the action for transforming the given source (of <{0}>).", typeof (TSource).Name), ex);
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

            throw new MappingFailedActionException(string.Format("Error before executing the Map method from ISimpleTransformer instance, caused by incorrect source type (of <{0}>), instead It's expected <{1}>", source.GetType().Name, typeof(TSource).Name));
        }

    }
}
