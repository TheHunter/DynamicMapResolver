using System;
using System.Collections.Generic;
using System.Linq;
using DynamicMapResolver.Exceptions;

namespace DynamicMapResolver.Impl
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TSource">The type of the source.</typeparam>
    /// <typeparam name="TDestination">The type of the destination.</typeparam>
    public abstract class SourceTransformer<TSource, TDestination>
        : ActionTransformer<TSource, TDestination>, ISourceTransformer<TSource, TDestination>
        where TSource : class
        where TDestination : class
    {
        private readonly HashSet<IPropertyMapper<TSource, TDestination>> propertyMappers;
        private readonly Action<TSource, TDestination> onTransformingDefault;

        /// <summary>
        /// Initializes a new instance of the <see cref="SourceTransformer{TSource, TDestination}"/> class.
        /// </summary>
        /// <param name="propertyMappers">The property mappers.</param>
        /// <param name="sourceType">Type of the source.</param>
        /// <param name="destinationType">Type of the destination.</param>
        /// <param name="beforeMapping">The before mapping.</param>
        /// <param name="afterMapping">The after mapping.</param>
        /// <param name="onTransforming">The on transforming.</param>
        /// <exception cref="DynamicMapResolver.Exceptions.MapperParameterException">propertyMappers;Collection of property mappers cannot be empty or null.</exception>
        /// <exception cref="DynamicMapResolver.Exceptions.NonUniqueSetterException">Property mappers must be unique, so verify the lambda setter expressions.</exception>
        protected SourceTransformer(IEnumerable<IPropertyMapper<TSource, TDestination>> propertyMappers, Type sourceType, Type destinationType,
            Action<TDestination> beforeMapping, Action<TDestination> afterMapping, Action<TSource, TDestination> onTransforming)
            : base(sourceType, destinationType, beforeMapping, afterMapping, onTransforming)
        {
            if (propertyMappers == null || !propertyMappers.Any())
                throw new MapperParameterException("propertyMappers", "Collection of property mappers cannot be empty or null.");

            this.propertyMappers = new HashSet<IPropertyMapper<TSource, TDestination>>(propertyMappers);

            if (this.propertyMappers.Count != propertyMappers.Count())
                throw new NonUniqueSetterException("Property mappers must be unique, so verify the lambda setter expressions.");

            this.onTransformingDefault = (src, dest) => UsePropertyMappers(src, dest, this.propertyMappers);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SourceTransformer{TSource, TDestination}"/> class.
        /// </summary>
        /// <param name="propertyMappers">The property mappers.</param>
        /// <param name="sourceType">Type of the source.</param>
        /// <param name="destinationType">Type of the destination.</param>
        /// <param name="beforeMapping">The before mapping.</param>
        /// <param name="afterMapping">The after mapping.</param>
        /// <exception cref="DynamicMapResolver.Exceptions.MapperParameterException">propertyMappers;Collection of property mappers cannot be empty or null.</exception>
        /// <exception cref="DynamicMapResolver.Exceptions.NonUniqueSetterException">Property mappers must be unique, so verify the lambda setter expressions.</exception>
        protected SourceTransformer(IEnumerable<IPropertyMapper<TSource, TDestination>> propertyMappers, Type sourceType, Type destinationType,
            Action<TDestination> beforeMapping, Action<TDestination> afterMapping)
            : base(sourceType, destinationType, beforeMapping, afterMapping)
        {
            if (propertyMappers == null || !propertyMappers.Any())
                throw new MapperParameterException("propertyMappers", "Collection of property mappers cannot be empty or null.");
            
            this.propertyMappers = new HashSet<IPropertyMapper<TSource, TDestination>>(propertyMappers);

            if (this.propertyMappers.Count != propertyMappers.Count())
                throw new NonUniqueSetterException("Property mappers must be unique, so verify the lambda setter expressions.");

            this.onTransformingDefault = (src, dest) => UsePropertyMappers(src, dest, this.propertyMappers);
        }

        /// <summary>
        /// Uses the property mappers.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="destination">The destination.</param>
        /// <param name="propMappers">The property mappers.</param>
        /// <exception cref="DynamicMapResolver.Exceptions.MapperParameterException">propMappers;The given parameter cannot be empty or null.</exception>
        /// <exception cref="DynamicMapResolver.Exceptions.FailedSetPropertyException"></exception>
        protected void UsePropertyMappers(TSource source, TDestination destination, IEnumerable<IPropertyMapper<TSource, TDestination>> propMappers)
        {
            if (propMappers == null || !propMappers.Any())
                throw new MapperParameterException("propMappers", "The given parameter cannot be empty or null.");

            var mappers = propMappers.ToArray();

            for (int index = 0; index < mappers.Length; index++ )
            {
                var mapper = mappers[index];
                try
                {
                    mapper.Setter(source, destination);   
                }
                catch (Exception ex)
                {
                    throw new FailedSetPropertyException(
                        string.Format(
                            "Error on executing lambda setter expression, property src: {0} - property dest: {1}",
                            mapper.PropertySource, mapper.PropertyDestination), ex);
                }
            }
        }

        /// <summary>
        /// Called when [mapping].
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="destination">The destination.</param>
        protected void OnMapping(TSource source, TDestination destination)
        {
            this.OnTransforming = this.onTransformingDefault;
            this.Transform(source, destination);
        }

        /// <summary>
        /// Called when [mapping].
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="destination">The destination.</param>
        /// <param name="propertiesToMap">The properties to map.</param>
        protected void OnMapping(TSource source, TDestination destination, IEnumerable<IPropertyMapper<TSource, TDestination>> propertiesToMap)
        {
            this.OnTransforming = (src, dest) => UsePropertyMappers(src, dest, propertiesToMap);
            this.Transform(source, destination);
        }

        /// <summary>
        /// A set of properties mappers for mapping destination object properties.
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
        /// Initializes a new instance of the <see cref="SourceTransformer"/> class.
        /// </summary>
        /// <param name="sourceType">Type of the source.</param>
        /// <param name="destinationType">Type of the destination.</param>
        /// <exception cref="DynamicMapResolver.Exceptions.MapperParameterException">
        /// sourceType;The source type cannot be null.
        /// or
        /// destinationType;The destination type cannot be null.
        /// </exception>
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
        /// Gets the type of the source.
        /// </summary>
        /// <value>
        /// The type of the source.
        /// </value>
        public Type SourceType
        {
            get { return this.sourceType; }
        }

        /// <summary>
        /// Gets the type of the destination.
        /// </summary>
        /// <value>
        /// The type of the destination.
        /// </value>
        public Type DestinationType
        {
            get { return this.destinationType; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [ignore exception on mapping].
        /// </summary>
        /// <value>
        /// <c>true</c> if [ignore exception on mapping]; otherwise, <c>false</c>.
        /// </value>
        public bool IgnoreExceptionOnMapping
        {
            get { return this.ignoreExceptionOnMapping; }
            set { this.ignoreExceptionOnMapping = value; }
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

            if (obj is SourceTransformer)
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
            return 11 * (this.sourceType.GetHashCode() - this.destinationType.GetHashCode());
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("sourceType: {0}, destinationType: {1}", this.sourceType.Name,
                                 this.destinationType.Name);
        }
    }

}
