using System;

namespace DynamicMapResolver
{
    /// <summary>
    /// A simple definition for mapping object data source into object data destination.
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TDestination"></typeparam>
    public interface ISourceMapper<TSource, TDestination>
        : ISourceMapper, ISourceTransformer<TSource, TDestination>
        where TSource : class
        where TDestination : class
    {
        /// <summary>
        /// Maps the source object transforming into destionation object type.
        /// </summary>
        /// <param name="source"></param>
        /// <returns>returns the source object transformed as destionation type.</returns>
        TDestination Map(TSource source);
    }

    /// <summary>
    /// 
    /// </summary>
    public interface ISourceMapper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        object Map(object source);

        /// <summary>
        /// 
        /// </summary>
        Type SourceType { get; }

        /// <summary>
        /// 
        /// </summary>
        Type DestinationType { get; }

    }
}
