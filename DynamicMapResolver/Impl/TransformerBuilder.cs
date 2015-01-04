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
    /// <typeparam name="TSource">The type of the source.</typeparam>
    /// <typeparam name="TDestination">The type of the destination.</typeparam>
    public class TransformerBuilder<TSource, TDestination>
        : ITransformerBuilder<TSource, TDestination>
        where TSource : class
        where TDestination : class
    {
        private readonly ITransformerObserver observer;
        private readonly bool canBeUsedResolver = true;
        private readonly HashSet<IPropertyMapper<TSource, TDestination>> propertyMappers;

        /// <summary>
        /// Initializes a new instance of the <see cref="TransformerBuilder{TSource, TDestination}"/> class.
        /// </summary>
        public TransformerBuilder()
            : this(TransformerObserver.Default)
        {
            this.canBeUsedResolver = false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TransformerBuilder{TSource, TDestination}"/> class.
        /// </summary>
        /// <param name="observer">The observer.</param>
        internal TransformerBuilder(ITransformerObserver observer)
        {
            this.observer = observer;
            this.propertyMappers = new HashSet<IPropertyMapper<TSource, TDestination>>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TransformerBuilder{TSource, TDestination}"/> class.
        /// </summary>
        /// <param name="propertyMappers">The property mappers.</param>
        internal TransformerBuilder(IEnumerable<IPropertyMapper<TSource, TDestination>> propertyMappers)
            : this(TransformerObserver.Default, propertyMappers)
        {
            this.canBeUsedResolver = false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TransformerBuilder{TSource, TDestination}"/> class.
        /// </summary>
        /// <param name="observer">The observer.</param>
        /// <param name="propertyMappers">The property mappers.</param>
        internal TransformerBuilder(ITransformerObserver observer, IEnumerable<IPropertyMapper<TSource, TDestination>> propertyMappers)
        {
            this.observer = observer;

            if (propertyMappers != null && propertyMappers.Any())
                this.propertyMappers = new HashSet<IPropertyMapper<TSource, TDestination>>(propertyMappers);
            else
                this.propertyMappers = new HashSet<IPropertyMapper<TSource, TDestination>>();
        }

        /// <summary>
        /// Builds the mapper.
        /// </summary>
        /// <returns></returns>
        public ISourceMapper<TSource, TDestination> BuildMapper()
        {
            return this.BuildMapper(null, null);
        }

        /// <summary>
        /// Builds the mapper.
        /// </summary>
        /// <param name="beforeMapping">The before mapping.</param>
        /// <param name="afterMapping">The after mapping.</param>
        /// <returns></returns>
        public ISourceMapper<TSource, TDestination> BuildMapper(Action<TDestination> beforeMapping, Action<TDestination> afterMapping)
        {
            var mapper = new SourceMapper<TSource, TDestination>(this.propertyMappers, beforeMapping, afterMapping);
            if (this.canBeUsedResolver)
                this.observer.RegisterMapper<ISourceMapper<TSource, TDestination>>(mapper);

            return mapper;
        }

        /// <summary>
        /// Builds the merger.
        /// </summary>
        /// <returns></returns>
        public ISourceMerger<TSource, TDestination> BuildMerger()
        {
            return this.BuildMerger(null, null);
        }

        /// <summary>
        /// Builds the merger.
        /// </summary>
        /// <param name="beforeMapping">The before mapping.</param>
        /// <param name="afterMapping">The after mapping.</param>
        /// <returns></returns>
        public ISourceMerger<TSource, TDestination> BuildMerger(Action<TDestination> beforeMapping, Action<TDestination> afterMapping)
        {
            var merger = new SourceMerger<TSource, TDestination>(this.propertyMappers, beforeMapping, afterMapping);
            if (this.canBeUsedResolver)
                this.observer.RegisterMerger<ISourceMerger<TSource, TDestination>>(merger);

            return merger;
        }

        /// <summary>
        /// Includes the specified property mapper.
        /// </summary>
        /// <param name="propertyMapper">The property mapper.</param>
        /// <returns></returns>
        /// <exception cref="DynamicMapResolver.Exceptions.MapperParameterException">propertyMapper;The propertyMapper instance cannot be null.</exception>
        public ITransformerBuilder<TSource, TDestination> Include(IPropertyMapper<TSource, TDestination> propertyMapper)
        {
            if (propertyMapper == null)
                throw new MapperParameterException("propertyMapper", "The propertyMapper instance cannot be null.");

            this.propertyMappers.Add(propertyMapper);
            return this;
        }

        /// <summary>
        /// Includes the specified property function mapper.
        /// </summary>
        /// <param name="propertyFuncMapper">The property function mapper.</param>
        /// <returns></returns>
        /// <exception cref="DynamicMapResolver.Exceptions.MapperParameterException">propertyFuncMapper;The expression for making mapper property cannot be null.</exception>
        public ITransformerBuilder<TSource, TDestination> Include(Func<ITransformerResolver, IPropertyMapper<TSource, TDestination>> propertyFuncMapper)
        {
            if (propertyFuncMapper == null)
                throw new MapperParameterException("propertyFuncMapper", "The expression for making mapper property cannot be null.");

            this.propertyMappers.Add(propertyFuncMapper.Invoke(this.observer));
            return this;
        }

        /// <summary>
        /// Excludes the specified property name.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns></returns>
        public ITransformerBuilder<TSource, TDestination> Exclude(string propertyName)
        {
            if (!string.IsNullOrEmpty(propertyName))
                this.propertyMappers.RemoveWhere(n => n.PropertyDestination.Equals(propertyName));

            return this;
        }

        /// <summary>
        /// / Excludes the specified property./
        /// </summary>
        /// <param name="property">The property.</param>
        /// <returns></returns>
        /// / / /
        public ITransformerBuilder<TSource, TDestination> Exclude(PropertyInfo property)
        {
            if (property == null)
                return this;
            
            return this.Exclude(property.Name);
        }
    }
}
