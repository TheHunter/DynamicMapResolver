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
        where TSource : class
        where TDestination : class
    {
        /// <summary>
        /// An action which be executed before mapping properties.
        /// </summary>
        Action<TDestination> BeforeMapping { get; }

        /// <summary>
        /// An action which be executed after mapping properties.
        /// </summary>
        Action<TDestination> AfterMapping { get; }

        /// <summary>
        /// A set of properties mappers for mapping destination object properties.
        /// </summary>
        IEnumerable<IPropertyMapper<TSource, TDestination>> PropertyMappers { get; }

    }
}
