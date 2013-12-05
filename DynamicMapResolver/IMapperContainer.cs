using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace DynamicMapResolver
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TDestination"></typeparam>
    public interface IMapperContainer<TSource, TDestination>
        : IContainerBuilder<TSource, TDestination>
        where TSource : class
        where TDestination : class
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyMapper"></param>
        /// <returns></returns>
        IMapperContainer<TSource, TDestination> Include(IPropertyMapper<TSource, TDestination> propertyMapper);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        IMapperContainer<TSource, TDestination> Exclude(string propertyName);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        IMapperContainer<TSource, TDestination> Exclude(PropertyInfo property);
    }
}
