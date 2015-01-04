using System;
using System.Reflection;

namespace DynamicMapResolver
{
    /// <summary>
    /// A generic interface for mapping properties.
    /// </summary>
    /// <typeparam name="TSource">The source type</typeparam>
    /// <typeparam name="TDestination">The destination type</typeparam>
    public interface IPropertyMapper<TSource, TDestination>
        : IPropertyMapInfo
        where TSource : class
        where TDestination : class
    {
        /// <summary>
        /// The action to execute for setting the destionation property value.
        /// </summary>
        Action<TSource, TDestination> Setter { get; }
    }

    /// <summary>
    /// Rappresents a property mapper for the given property info about setter and getter.
    /// </summary>
    public interface IPropertyMapper
        : IPropertyMapper<object, object>
    {
        /// <summary>
        /// Gets the source property information.
        /// </summary>
        /// <value>
        /// The source property information.
        /// </value>
        PropertyInfo SrcPropertyInfo { get; }

        /// <summary>
        /// Gets the dest property information.
        /// </summary>
        /// <value>
        /// The dest property information.
        /// </value>
        PropertyInfo destPropertyInfo { get; }

    }
}
