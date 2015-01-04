using System;
using System.Collections.Generic;

namespace DynamicMapResolver
{
    /// <summary>
    /// A generic interface for transforming object data source into object data destination.
    /// </summary>
    /// <typeparam name="TSource">The source object type to transform</typeparam>
    /// <typeparam name="TDestination">The destination object type to be transformed.</typeparam>
    public interface ISourceTransformer<TSource, TDestination>
        : ISourceTransformer
        where TSource : class
        where TDestination : class
    {
        /// <summary>
        /// A set of properties mappers for mapping destination object properties.
        /// </summary>
        IEnumerable<IPropertyMapper<TSource, TDestination>> PropertyMappers { get; }

    }

    /// <summary>
    /// 
    /// </summary>
    public interface ISourceTransformer
    {
        /// <summary>
        /// Gets the type of the source.
        /// </summary>
        /// <value>
        /// The type of the source.
        /// </value>
        Type SourceType { get; }

        /// <summary>
        /// Gets the type of the destination.
        /// </summary>
        /// <value>
        /// The type of the destination.
        /// </value>
        Type DestinationType { get; }

        /// <summary>
        /// Gets or sets a value indicating whether [ignore exception on mapping].
        /// </summary>
        /// <value>
        /// <c>true</c> if [ignore exception on mapping]; otherwise, <c>false</c>.
        /// </value>
        bool IgnoreExceptionOnMapping { get; set; }

    }
}
