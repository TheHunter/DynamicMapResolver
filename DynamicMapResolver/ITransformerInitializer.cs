using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynamicMapResolver
{
    /// <summary>
    /// 
    /// </summary>
    public interface ITransformerInitializer
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        ITransformerBuilder<TSource, TDestination> MakeTransformerBuilder<TSource, TDestination>(BuilderType type)
            where TSource : class
            where TDestination : class;

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="propertyMappers"></param>
        /// <returns></returns>
        ITransformerBuilder<TSource, TDestination> MakeTransformerBuilder<TSource, TDestination>(
            IEnumerable<IPropertyMapper<TSource, TDestination>> propertyMappers)
            where TSource : class
            where TDestination : class;


    }
}
