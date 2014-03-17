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
    public class TransformerBuilder<TSource, TDestination>
        : ITransformerBuilder<TSource, TDestination>
        where TSource : class
        where TDestination : class
    {
        private readonly ITransformerResolver resolver;
        private readonly HashSet<IPropertyMapper<TSource, TDestination>> propertyMappers;

        /// <summary>
        /// 
        /// </summary>
        public TransformerBuilder()
            : this(TransformerResolver.Default)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="resolver"></param>
        internal TransformerBuilder(ITransformerResolver resolver)
        {
            this.resolver = resolver;
            this.propertyMappers = new HashSet<IPropertyMapper<TSource, TDestination>>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyMappers"></param>
        internal TransformerBuilder(IEnumerable<IPropertyMapper<TSource, TDestination>> propertyMappers)
            : this(TransformerResolver.Default, propertyMappers)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="resolver"></param>
        /// <param name="propertyMappers"></param>
        internal TransformerBuilder(ITransformerResolver resolver, IEnumerable<IPropertyMapper<TSource, TDestination>> propertyMappers)
        {
            this.resolver = resolver;

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
            var mapper = new SourceMapper<TSource, TDestination>(this.propertyMappers, beforeMapping, afterMapping);
            if (this.resolver != null)
                this.resolver.ObserveMapper(mapper);

            return mapper;
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
            var merger = new SourceMerger<TSource, TDestination>(this.propertyMappers, beforeMapping, afterMapping);
            if (this.resolver != null)
                this.resolver.ObserveMerger(merger);

            return merger;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyMapper"></param>
        /// <returns></returns>
        public ITransformerBuilder<TSource, TDestination> Include(IPropertyMapper<TSource, TDestination> propertyMapper)
        {
            if (propertyMapper == null)
                throw new MapperParameterException("propertyMapper", "The propertyMapper instance cannot be null.");

            this.propertyMappers.Add(propertyMapper);
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyFuncMapper"></param>
        /// <returns></returns>
        public ITransformerBuilder<TSource, TDestination> Include(Func<ITransformerResolver, IPropertyMapper<TSource, TDestination>> propertyFuncMapper)
        {
            if (propertyFuncMapper == null)
                throw new MapperParameterException("propertyFuncMapper", "The expression for making mapper property cannot be null.");

            this.propertyMappers.Add(propertyFuncMapper.Invoke(this.resolver));
            return this;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public ITransformerBuilder<TSource, TDestination> Exclude(string propertyName)
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
        public ITransformerBuilder<TSource, TDestination> Exclude(PropertyInfo property)
        {
            if (property == null)
                return this;
            
            return this.Exclude(property.Name);
        }
    }
}
