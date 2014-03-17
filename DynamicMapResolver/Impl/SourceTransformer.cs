using System;
using System.Collections.Generic;
using System.Linq;
using DynamicMapResolver.Exceptions;

namespace DynamicMapResolver.Impl
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TDestination"></typeparam>
    public abstract class SourceTransformer<TSource, TDestination>
        : ISourceTransformer<TSource, TDestination>
        where TSource : class
        where TDestination : class
    {
        private readonly Type sourceType;
        private readonly Type destinationType;
        private readonly Action<TDestination> beforeMapping;
        private readonly Action<TDestination> afterMapping;
        private readonly HashSet<IPropertyMapper<TSource, TDestination>> propertyMappers;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyMappers"></param>
        /// <param name="sourceType"></param>
        /// <param name="destinationType"></param>
        /// <param name="beforeMapping"></param>
        /// <param name="afterMapping"></param>
        protected SourceTransformer(IEnumerable<IPropertyMapper<TSource, TDestination>> propertyMappers, Type sourceType, Type destinationType,
            Action<TDestination> beforeMapping, Action<TDestination> afterMapping)
        {
            if (propertyMappers == null || !propertyMappers.Any())
                throw new MapperParameterException("propertyMappers", "Collection of property mappers cannot be empty or null.");
            
            this.propertyMappers = new HashSet<IPropertyMapper<TSource, TDestination>>(propertyMappers);

            if (this.propertyMappers.Count != propertyMappers.Count())
                throw new NonUniqueSetterException("Property mappers must be unique, so verify the lambda setter expressions.");

            if (sourceType == null)
                throw new MapperParameterException("sourceType", "The source type cannot be null.");

            if (destinationType == null)
                throw new MapperParameterException("destinationType", "The destination type cannot be null.");

            this.sourceType = sourceType;
            this.destinationType = destinationType;
            this.beforeMapping = beforeMapping;
            this.afterMapping = afterMapping;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <param name="propertiesToMap"></param>
        protected void OnMapping(TSource source, TDestination destination, IEnumerable<IPropertyMapper<TSource, TDestination>> propertiesToMap)
        {
            this.OnMapping(source, destination, false, propertiesToMap);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <param name="ignoreExceptionOnMapping"></param>
        /// <param name="propertiesToMap"></param>
        protected void OnMapping(TSource source, TDestination destination, bool ignoreExceptionOnMapping, IEnumerable<IPropertyMapper<TSource, TDestination>> propertiesToMap)
        {
            #region Executing BeforeMappingAction
            try
            {
                if (this.beforeMapping != null)
                    this.beforeMapping.Invoke(destination);
            }
            catch (Exception ex)
            {
                throw new MappingFailedActionException("Error on executing BeforeMapping action.", ex);
            }
            #endregion

            try
            {
                propertiesToMap.All
                    (
                        mapper =>
                        {
                            mapper.Setter.Invoke(source, destination);
                            return true;
                        }
                    );
            }
            catch (Exception ex)
            {
                if (!ignoreExceptionOnMapping)
                    throw new FailedSetPropertyException("Exception on execution lambda setter action.", ex);
            }

            #region Executing AfterMapping Action.
            try
            {
                if (this.afterMapping != null)
                    this.afterMapping.Invoke(destination);
            }
            catch (Exception ex)
            {
                throw new MappingFailedActionException("Error on executing AfterMapping action.", ex);
            }
            #endregion
        }

        /// <summary>
        /// 
        /// </summary>
        public Type SourceType
        {
            get { return this.sourceType; }
        }

        /// <summary>
        /// 
        /// </summary>
        public Type DestinationType
        {
            get { return this.destinationType; }
        }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<IPropertyMapper<TSource, TDestination>> PropertyMappers
        {
            get { return this.propertyMappers; }
        }
    }
}
