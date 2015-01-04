using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynamicMapResolver
{
    /// <summary>
    /// Rappresents a way for registering transformers into particolar container.
    /// </summary>
    public interface ITransformerRegister
    {
        /// <summary>
        /// Registers the mapper.
        /// </summary>
        /// <typeparam name="TMapper">The type of the mapper.</typeparam>
        /// <param name="mapper">The mapper.</param>
        /// <returns></returns>
        bool RegisterMapper<TMapper>(TMapper mapper) where TMapper : class, ISourceMapper;

        /// <summary>
        /// Registers the mapper.
        /// </summary>
        /// <typeparam name="TMapper">The type of the mapper.</typeparam>
        /// <param name="mapper">The mapper.</param>
        /// <param name="keyService">The key service.</param>
        /// <returns></returns>
        bool RegisterMapper<TMapper>(TMapper mapper, object keyService) where TMapper : class, ISourceMapper;

        /// <summary>
        /// Registers the merger.
        /// </summary>
        /// <typeparam name="TMerger">The type of the merger.</typeparam>
        /// <param name="merger">The merger.</param>
        /// <returns></returns>
        bool RegisterMerger<TMerger>(TMerger merger) where TMerger : class, ISourceMerger;

        /// <summary>
        /// Registers the merger.
        /// </summary>
        /// <typeparam name="TMerger">The type of the merger.</typeparam>
        /// <param name="merger">The merger.</param>
        /// <param name="keyService">The key service.</param>
        /// <returns></returns>
        bool RegisterMerger<TMerger>(TMerger merger, object keyService) where TMerger : class, ISourceMerger;
    }
}
