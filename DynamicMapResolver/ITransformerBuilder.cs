using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace DynamicMapResolver
{
    /// <summary>
    /// A transformer builder which is able to add / remove property mappers in order to customize mappers / mergers.
    /// </summary>
    /// <typeparam name="TSource">The type of the source.</typeparam>
    /// <typeparam name="TDestination">The type of the destination.</typeparam>
    public interface ITransformerBuilder<TSource, TDestination>
        : IContainerBuilder<TSource, TDestination>
        where TSource : class
        where TDestination : class
    {
        /// <summary>
        /// Includes the specified property mapper.
        /// </summary>
        /// <param name="propertyMapper">The property mapper.</param>
        /// <returns></returns>
        ITransformerBuilder<TSource, TDestination> Include(IPropertyMapper<TSource, TDestination> propertyMapper);

        /// <summary>
        /// Includes the specified property function mapper.
        /// </summary>
        /// <param name="propertyFuncMapper">The property function mapper.</param>
        /// <returns></returns>
        ITransformerBuilder<TSource, TDestination> Include(
            Func<ITransformerResolver, IPropertyMapper<TSource, TDestination>> propertyFuncMapper);

        /// <summary>
        /// Excludes the specified property name.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns></returns>
        ITransformerBuilder<TSource, TDestination> Exclude(string propertyName);

        /// <summary>
        /// Excludes the specified property.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <returns></returns>
        ITransformerBuilder<TSource, TDestination> Exclude(PropertyInfo property);
    }
}
