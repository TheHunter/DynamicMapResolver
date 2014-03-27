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
        : ActionTransformer<TSource, TDestination>, ISourceTransformer<TSource, TDestination>
        where TSource : class
        where TDestination : class
    {
        private readonly HashSet<IPropertyMapper<TSource, TDestination>> propertyMappers;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyMappers"></param>
        /// <param name="sourceType"></param>
        /// <param name="destinationType"></param>
        /// <param name="beforeMapping"></param>
        /// <param name="afterMapping"></param>
        /// <param name="onTransforming"></param>
        protected SourceTransformer(IEnumerable<IPropertyMapper<TSource, TDestination>> propertyMappers, Type sourceType, Type destinationType,
            Action<TDestination> beforeMapping, Action<TDestination> afterMapping, Action<TSource, TDestination> onTransforming)
            : base(sourceType, destinationType, beforeMapping, afterMapping, onTransforming)
        {
            if (propertyMappers == null || !propertyMappers.Any())
                throw new MapperParameterException("propertyMappers", "Collection of property mappers cannot be empty or null.");

            this.propertyMappers = new HashSet<IPropertyMapper<TSource, TDestination>>(propertyMappers);

            if (this.propertyMappers.Count != propertyMappers.Count())
                throw new NonUniqueSetterException("Property mappers must be unique, so verify the lambda setter expressions.");
        }

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
            : base(sourceType, destinationType, beforeMapping, afterMapping)
        {
            if (propertyMappers == null || !propertyMappers.Any())
                throw new MapperParameterException("propertyMappers", "Collection of property mappers cannot be empty or null.");
            
            this.propertyMappers = new HashSet<IPropertyMapper<TSource, TDestination>>(propertyMappers);

            if (this.propertyMappers.Count != propertyMappers.Count())
                throw new NonUniqueSetterException("Property mappers must be unique, so verify the lambda setter expressions.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <param name="propMappers"></param>
        protected void UsePropertyMappers(TSource source, TDestination destination, IEnumerable<IPropertyMapper<TSource, TDestination>> propMappers)
        {
            propMappers.All
                    (
                        mapper =>
                        {
                            mapper.Setter.Invoke(source, destination);
                            return true;
                        }
                    );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <param name="propertiesToMap"></param>
        protected void OnMapping(TSource source, TDestination destination, IEnumerable<IPropertyMapper<TSource, TDestination>> propertiesToMap)
        {
            this.OnTransforming = (source1, destination1) => UsePropertyMappers(source1, destination1, propertiesToMap);
            this.Transform(source, destination);
        }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<IPropertyMapper<TSource, TDestination>> PropertyMappers
        {
            get { return this.propertyMappers; }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class SourceTransformer
        : ISourceTransformer
    {
        private readonly Type sourceType;
        private readonly Type destinationType;
        private bool ignoreExceptionOnMapping;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceType"></param>
        /// <param name="destinationType"></param>
        public SourceTransformer(Type sourceType, Type destinationType)
        {
            if (sourceType == null)
                throw new MapperParameterException("sourceType", "The source type cannot be null.");

            if (destinationType == null)
                throw new MapperParameterException("destinationType", "The destination type cannot be null.");

            this.sourceType = sourceType;
            this.destinationType = destinationType;
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
        public bool IgnoreExceptionOnMapping
        {
            get { return this.ignoreExceptionOnMapping; }
            set { this.ignoreExceptionOnMapping = value; }
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

            if (obj is SourceTransformer)
                return this.GetHashCode() == obj.GetHashCode();
            
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return 11 * (this.sourceType.GetHashCode() - this.destinationType.GetHashCode());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("sourceType: {0}, destinationType: {1}", this.sourceType.Name,
                                 this.destinationType.Name);
        }
    }

}
