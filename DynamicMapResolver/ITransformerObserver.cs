using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynamicMapResolver
{
    /// <summary>
    /// 
    /// </summary>
    public interface ITransformerObserver
        : ITransformerResolver, ITransformerRegister, ITransformerInitializer, IDynamicTransformerBuilder
    {
        /// <summary>
        /// Retrieves the mapper.
        /// </summary>
        /// <typeparam name="TMapper">The type of the mapper.</typeparam>
        /// <param name="keyService">The key service.</param>
        /// <returns></returns>
        TMapper RetrieveMapper<TMapper>(object keyService = null)
            where TMapper : class, ISourceMapper;

        /// <summary>
        /// Retrieves the merger.
        /// </summary>
        /// <typeparam name="TMerger">The type of the merger.</typeparam>
        /// <param name="keyService">The key service.</param>
        /// <returns></returns>
        TMerger RetrieveMerger<TMerger>(object keyService = null)
            where TMerger : class, ISourceMerger;
    }
}
