using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynamicMapResolver
{
    /// <summary>
    /// Rappresents a generic resolver used for transforming source into destination type.
    /// </summary>
    public interface ITransformerResolver
    {
        #region using mappers

        /// <summary>
        /// Tries to map the given instance into destination type.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="destinationType">Type of the destination.</param>
        /// <returns></returns>
        object TryToMap(object source, Type destinationType);

        /// <summary>
        /// Tries to map the given instance into destination type using an specific key service.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="destinationType">Type of the destination.</param>
        /// <param name="keyService">The key service.</param>
        /// <returns></returns>
        object TryToMap(object source, Type destinationType, object keyService);

        /// <summary>
        /// Tries to map the given instance into generic destination type.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TDestination">The type of the destination.</typeparam>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        TDestination TryToMap<TSource, TDestination>(TSource source);

        /// <summary>
        /// Tries to map the given instance into generic destination type using an specific key service.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TDestination">The type of the destination.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="keyService">The key service.</param>
        /// <returns></returns>
        TDestination TryToMap<TSource, TDestination>(TSource source, object keyService);

        #endregion

        #region using mergers

        /// <summary>
        /// Tries to merge the given instance with the destination instance.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="destination">The destination.</param>
        /// <returns></returns>
        object TryToMerge(object source, object destination);

        /// <summary>
        /// Tries to merge the given instance with the destination instance using an specific key service.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="destination">The destination.</param>
        /// <param name="keyService">The key service.</param>
        /// <returns></returns>
        object TryToMerge(object source, object destination, object keyService);

        /// <summary>
        /// Tries to merge the given instance with the destination instance.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TDestination">The type of the destination.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="destination">The destination.</param>
        /// <returns></returns>
        TDestination TryToMerge<TSource, TDestination>(TSource source, TDestination destination);

        /// <summary>
        ///  given instance with the destination instance using an specific key service
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TDestination">The type of the destination.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="destination">The destination.</param>
        /// <param name="keyService">The key service.</param>
        /// <returns></returns>
        TDestination TryToMerge<TSource, TDestination>(TSource source, TDestination destination, object keyService);

        #endregion
    }
}
