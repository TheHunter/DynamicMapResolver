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
        /// <returns></returns>
        ISourceMapper<TSource, TDestination> BuildMapper();

        /// <summary>
        /// Builds the internal mapper with the given actions.
        /// </summary>
        /// <param name="beforeMapping">The before mapping.</param>
        /// <param name="afterMapping">The after mapping.</param>
        /// <returns></returns>
        ISourceMapper<TSource, TDestination> BuildMapper(Action<TDestination> beforeMapping,
                                                         Action<TDestination> afterMapping);

        /// <summary>
        /// Builds the internal merger.
        /// </summary>
        /// <returns></returns>
        ISourceMerger<TSource, TDestination> BuildMerger();

        /// <summary>
        /// Builds the internal merger.
        /// </summary>
        /// <param name="beforeMapping">The before mapping.</param>
        /// <param name="afterMapping">The after mapping.</param>
        /// <returns></returns>
        ISourceMerger<TSource, TDestination> BuildMerger(Action<TDestination> beforeMapping,
                                                         Action<TDestination> afterMapping);
    }
}
