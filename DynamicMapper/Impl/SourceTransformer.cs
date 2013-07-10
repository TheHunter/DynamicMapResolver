using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DynamicMapper.Exceptions;

namespace DynamicMapper.Impl
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
        private readonly Action<TDestination> beforeMapping;
        private readonly Action<TDestination> afterMapping;
        private readonly HashSet<IPropertyMapper<TSource, TDestination>> propertyMappers;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyMappers"></param>
        /// <param name="beforeMapping"></param>
        /// <param name="afterMapping"></param>
        protected SourceTransformer(IEnumerable<IPropertyMapper<TSource, TDestination>> propertyMappers, Action<TDestination> beforeMapping, Action<TDestination> afterMapping)
        {
            if (propertyMappers == null)
                throw new MapperParameterException("propertiesToMap", "Collection of property mappers cannot be null.");
            
            this.propertyMappers = new HashSet<IPropertyMapper<TSource, TDestination>>(propertyMappers);

            if (this.propertyMappers.Count != propertyMappers.Count())
                throw new NonUniqueSetterException("Property mappers must be unique, so verify the lambda setter expressions.");

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
                if (this.BeforeMapping != null)
                    this.BeforeMapping.Invoke(destination);
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
                if (this.AfterMapping != null)
                    this.AfterMapping.Invoke(destination);
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
        public Action<TDestination> BeforeMapping
        {
            get { return this.beforeMapping; }
        }

        /// <summary>
        /// 
        /// </summary>
        public Action<TDestination> AfterMapping
        {
            get { return this.afterMapping; }
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
