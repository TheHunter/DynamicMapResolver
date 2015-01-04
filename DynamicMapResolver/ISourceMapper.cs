using System;

namespace DynamicMapResolver
{
    /// <summary>
    /// A simple definition for mapping object data source into object data destination.
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TDestination"></typeparam>
    public interface ISourceMapper<TSource, TDestination>
        : ISourceTransformer<TSource, TDestination>, ISimpleMapper<TSource, TDestination>
        where TSource : class
        where TDestination : class
    {
    }

    /// <summary>
    /// A simple mapper which transform source instance into output destination.
    /// </summary>
    public interface ISourceMapper
        : ISourceTransformer
    {
        /// <summary>
        /// Transform the given instance into destination type.
        /// </summary>
        /// <param name="source">the instance to transform.</param>
        /// <returns></returns>
        object Map(object source);
    }

}
