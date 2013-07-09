using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynamicMapper
{
    /// <summary>
    /// A generic interface for mapping properties.
    /// </summary>
    /// <typeparam name="TSource">The source type</typeparam>
    /// <typeparam name="TDestination">The destination type</typeparam>
    public interface IPropertyMapper<TSource, TDestination>
        where TSource : class
        where TDestination : class
    {
        /// <summary>
        /// The property source which value will set the associated property destination.
        /// </summary>
        string PropertySource { get; }

        /// <summary>
        /// The property destination which value will be set from property source value.
        /// </summary>
        string PropertyDestination { get; }

        /// <summary>
        /// The action to execute for setting the destionation property value.
        /// </summary>
        Action<TSource, TDestination> Setter { get; }
    }
}
