using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynamicMapResolver
{
    /// <summary>
    /// Rappresents a factory for dynamic transformers.
    /// </summary>
    public interface IDynamicTransformerBuilder
    {
        /// <summary>
        /// Builds the automatic resolver mapper for compatibles properties.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TDestination">The type of the destination.</typeparam>
        /// <param name="beforeMapping">The before mapping.</param>
        /// <param name="afterMapping">The after mapping.</param>
        /// <returns></returns>
        bool BuildAutoResolverMapper<TSource, TDestination>(Action<TDestination> beforeMapping,
                                                            Action<TDestination> afterMapping)
            where TSource : class
            where TDestination : class;

        /// <summary>
        /// Builds the automatic resolver mapper for compatibles properties using the given key service for dedicated context and actions.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TDestination">The type of the destination.</typeparam>
        /// <param name="keyService">The key service.</param>
        /// <param name="beforeMapping">The before mapping.</param>
        /// <param name="afterMapping">The after mapping.</param>
        /// <returns></returns>
        bool BuildAutoResolverMapper<TSource, TDestination>(object keyService, Action<TDestination> beforeMapping,
                                                            Action<TDestination> afterMapping)
            where TSource : class
            where TDestination : class;

        /// <summary>
        /// Builds the automatic resolver mapper for compatibles properties.
        /// </summary>
        /// <param name="sourceType">Type of the source.</param>
        /// <param name="destinationType">Type of the destination.</param>
        /// <returns></returns>
        bool BuildAutoResolverMapper(Type sourceType, Type destinationType);

        /// <summary>
        /// Builds the automatic resolver mapper for compatibles properties using a service key for dedicated context.
        /// </summary>
        /// <param name="keyService">The key service.</param>
        /// <param name="sourceType">Type of the source.</param>
        /// <param name="destinationType">Type of the destination.</param>
        /// <returns></returns>
        bool BuildAutoResolverMapper(object keyService, Type sourceType, Type destinationType);

        /// <summary>
        /// Builds the automatic resolver merger for compatibles properties.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TDestination">The type of the destination.</typeparam>
        /// <param name="beforeMapping">The before mapping.</param>
        /// <param name="afterMapping">The after mapping.</param>
        /// <returns></returns>
        bool BuildAutoResolverMerger<TSource, TDestination>(Action<TDestination> beforeMapping,
                                                            Action<TDestination> afterMapping)
            where TSource : class
            where TDestination : class;

        /// <summary>
        /// Builds the automatic resolver merger for compatibles properties using the given key service for dedicated context and actions.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TDestination">The type of the destination.</typeparam>
        /// <param name="keyService">The key service.</param>
        /// <param name="beforeMapping">The before mapping.</param>
        /// <param name="afterMapping">The after mapping.</param>
        /// <returns></returns>
        bool BuildAutoResolverMerger<TSource, TDestination>(object keyService, Action<TDestination> beforeMapping,
                                                            Action<TDestination> afterMapping)
            where TSource : class
            where TDestination : class;

        /// <summary>
        /// Builds the automatic resolver merger for compatibles properties using a default key service context.
        /// </summary>
        /// <param name="sourceType">Type of the source.</param>
        /// <param name="destinationType">Type of the destination.</param>
        /// <returns></returns>
        bool BuildAutoResolverMerger(Type sourceType, Type destinationType);

        /// <summary>
        /// Builds the automatic resolver merger for compatibles properties using the given key service for dedicated context.
        /// </summary>
        /// <param name="keyService">The key service.</param>
        /// <param name="sourceType">Type of the source.</param>
        /// <param name="destinationType">Type of the destination.</param>
        /// <returns></returns>
        bool BuildAutoResolverMerger(object keyService, Type sourceType, Type destinationType);
    }
}
