using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynamicMapResolver
{
    /// <summary>
    /// Rappresents a simple mapper.
    /// </summary>
    /// <typeparam name="TSource">The type of the source.</typeparam>
    /// <typeparam name="TDestination">The type of the destination.</typeparam>
    public interface ISimpleMapper<in TSource, out TDestination>
        : ISourceMapper
    {
        /// <summary>
        /// Maps the source object transforming into destionation object type.
        /// </summary>
        /// <param name="source"></param>
        /// <returns>returns the source object transformed as destionation type.</returns>
        TDestination Map(TSource source);
    }
}
