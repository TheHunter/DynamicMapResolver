using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using DynamicMapResolver.Exceptions;

namespace DynamicMapResolver.Impl
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TDestination"></typeparam>
    public class MapperContainer<TSource, TDestination>
        : IMapperContainer<TSource, TDestination>
        where TSource : class
        where TDestination : class
    {
        private readonly HashSet<IPropertyMapper<TSource, TDestination>> propertyMappers;

        /// <summary>
        /// Creates an empty container.
        /// </summary>
        public MapperContainer()
        {
            this.propertyMappers = new HashSet<IPropertyMapper<TSource, TDestination>>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyMappers"></param>
        internal MapperContainer(IEnumerable<IPropertyMapper<TSource, TDestination>> propertyMappers)
        {
            if (propertyMappers != null && propertyMappers.Any())
                this.propertyMappers = new HashSet<IPropertyMapper<TSource, TDestination>>(propertyMappers);
            else
                this.propertyMappers = new HashSet<IPropertyMapper<TSource, TDestination>>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ISourceMapper<TSource, TDestination> BuildMapper()
        {
            return this.BuildMapper(null, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="beforeMapping"></param>
        /// <param name="afterMapping"></param>
        /// <returns></returns>
        public ISourceMapper<TSource, TDestination> BuildMapper(Action<TDestination> beforeMapping, Action<TDestination> afterMapping)
        {
            return new SourceMapper<TSource, TDestination>(this.propertyMappers, beforeMapping, afterMapping);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ISourceMerger<TSource, TDestination> BuildMerger()
        {
            return this.BuildMerger(null, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="beforeMapping"></param>
        /// <param name="afterMapping"></param>
        /// <returns></returns>
        public ISourceMerger<TSource, TDestination> BuildMerger(Action<TDestination> beforeMapping, Action<TDestination> afterMapping)
        {
            return new SourceMerger<TSource, TDestination>(this.propertyMappers, beforeMapping, afterMapping);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyMapper"></param>
        /// <returns></returns>
        public IMapperContainer<TSource, TDestination> Include(IPropertyMapper<TSource, TDestination> propertyMapper)
        {
            if (propertyMapper != null)
                this.propertyMappers.Add(propertyMapper);
            
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public IMapperContainer<TSource, TDestination> Exclude(string propertyName)
        {
            if (!string.IsNullOrEmpty(propertyName))
                this.propertyMappers.RemoveWhere(n => n.PropertyDestination.Equals(propertyName));

            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public IMapperContainer<TSource, TDestination> Exclude(PropertyInfo property)
        {
            if (property == null)
                return this;
            
            return this.Exclude(property.Name);
        }
    }
}
