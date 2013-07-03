using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynamicMapper
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TDestination"></typeparam>
    public interface ISourceParser<TSource, TDestination>
        where TSource : class
        where TDestination : class
    {
        /// <summary>
        /// 
        /// </summary>
        Action<TDestination> BeforeMapping { get; }

        /// <summary>
        /// 
        /// </summary>
        Action<TDestination> AfterMapping { get; }

        /// <summary>
        /// 
        /// </summary>
        bool IgnoreExceptionOnMapping { get; set; }

        /// <summary>
        /// 
        /// </summary>
        IEnumerable<IPropertyMapper<TSource, TDestination>> PropertyMappers { get; }

    }
}
