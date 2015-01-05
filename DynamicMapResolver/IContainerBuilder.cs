using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynamicMapResolver
{
    /// <summary>
    /// Rappresents a generic builder for customization of mappers / mergers.
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TDestination"></typeparam>
    public interface IContainerBuilder<TSource, TDestination>
        where TSource : class
        where TDestination : class
    {
        /// <summary>
        /// Builds the internal mapper.
        /// </summary>
        /// <param name="serviceKey">The default key.</param>
        /// <returns></returns>
        ISourceMapper<TSource, TDestination> BuildMapper(string serviceKey = null);

        /// <summary>
        /// Builds the internal mapper with the given actions and possible service key.
        /// </summary>
        /// <param name="beforeMapping">The before mapping.</param>
        /// <param name="afterMapping">The after mapping.</param>
        /// <param name="defaultKey">The default key.</param>
        /// <returns></returns>
        ISourceMapper<TSource, TDestination> BuildMapper(Action<TDestination> beforeMapping,
            Action<TDestination> afterMapping,
            string defaultKey = null
            );

        /// <summary>
        /// Builds the internal merger.
        /// </summary>
        /// <param name="serviceKey">The default key.</param>
        /// <returns></returns>
        ISourceMerger<TSource, TDestination> BuildMerger(string serviceKey = null);

        /// <summary>
        /// Builds the internal merger.
        /// </summary>
        /// <param name="beforeMapping">The before mapping.</param>
        /// <param name="afterMapping">The after mapping.</param>
        /// <param name="defaultKey">The default key.</param>
        /// <returns></returns>
        ISourceMerger<TSource, TDestination> BuildMerger(Action<TDestination> beforeMapping,
            Action<TDestination> afterMapping,
            string defaultKey = null
            );
    }
}
