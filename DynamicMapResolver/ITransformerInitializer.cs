using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynamicMapResolver
{
    /// <summary>
    /// A generic interface for precompiling transformers
    /// </summary>
    public interface ITransformerInitializer
    {
        /// <summary>
        /// Makes the transformer builder following the specified builder type.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TDestination">The type of the destination.</typeparam>
        /// <param name="type">The builder type to use.</param>
        /// <returns></returns>
        ITransformerBuilder<TSource, TDestination> MakeTransformerBuilder<TSource, TDestination>(BuilderType type)
            where TSource : class
            where TDestination : class;

        /// <summary>
        /// Makes the transformer builder with the given property mappers.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TDestination">The type of the destination.</typeparam>
        /// <param name="propertyMappers">The property mappers.</param>
        /// <returns></returns>
        ITransformerBuilder<TSource, TDestination> MakeTransformerBuilder<TSource, TDestination>(
            IEnumerable<IPropertyMapper<TSource, TDestination>> propertyMappers)
            where TSource : class
            where TDestination : class;


    }
}
