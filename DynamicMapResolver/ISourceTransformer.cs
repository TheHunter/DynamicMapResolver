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
    /// <typeparam name="TDestination"></typeparam>
    public interface ISourceTransformer
    {
        /// <summary>
        /// 
        /// </summary>
        Type SourceType { get; }

        /// <summary>
        /// 
        /// </summary>
        Type DestinationType { get; }

        /// <summary>
        /// 
        /// </summary>
        bool IgnoreExceptionOnMapping { get; set; }

    }
}
