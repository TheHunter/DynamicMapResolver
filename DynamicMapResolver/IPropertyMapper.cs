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
    /// 
    /// </summary>
    public interface IPropertyMapper
        : IPropertyMapper<object, object>
    {
        /// <summary>
        /// 
        /// </summary>
        PropertyInfo SrcPropertyInfo { get; }

        /// <summary>
        /// 
        /// </summary>
        PropertyInfo destPropertyInfo { get; }

    }
}
