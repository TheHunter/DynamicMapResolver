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
    /// 
    /// </summary>
    public interface ISourceMapper
        : ISourceTransformer
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        object Map(object source);
    }

}
