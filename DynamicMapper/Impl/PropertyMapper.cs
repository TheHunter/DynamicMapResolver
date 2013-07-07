using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using DynamicMapper.Exceptions;

namespace DynamicMapper.Impl
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TDestination"></typeparam>
    public class PropertyMapper<TSource, TDestination>
        : IPropertyMapper<TSource, TDestination>
        where TSource : class
        where TDestination : class
    {
        private readonly string propertySource;
        private readonly string propertyDestination;
        private readonly Action<TSource, TDestination> setter;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="setter"></param>
        public PropertyMapper(Action<TSource, TDestination> setter)
        {
            if (setter == null)
                throw new LambdaSetterException("The setter action for property mapper cannot be null.");

            this.setter = setter;
            this.propertySource = "anonymous";
            this.propertyDestination = "anonymous";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="setter"></param>
        /// <param name="propertySrc"></param>
        /// <param name="propertyDest"></param>
        public PropertyMapper(Action<TSource, TDestination> setter, string propertySrc, string propertyDest)
        {
            if (setter == null)
                throw new LambdaSetterException("The setter action for property mapper cannot be null.");

            if (propertySrc == null || propertySrc.Trim().Equals(string.Empty))
                throw new MapperParameterException("propertySrc", "The getter property name cannot be null or empty, you have to use the suitable constructor without property names parameters.");

            if (propertyDest == null || propertyDest.Trim().Equals(string.Empty))
                throw new MapperParameterException("propertyDest", "The setter property name cannot be null or empty, you have to use the suitable constructor without property names parameters.");

            this.setter = setter;
            this.propertySource = propertySrc.Trim();
            this.propertyDestination = propertyDest.Trim();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="srcProperty"></param>
        /// <param name="destProperty"></param>
        public PropertyMapper(PropertyInfo srcProperty, PropertyInfo destProperty)
        {
            Action<TSource, TDestination> action = FactoryMapper.DynamicPropertyMap<TSource, TDestination>(srcProperty, destProperty);
            if (action == null)
                throw new LambdaSetterException("The setter action for property mapper cannot be null.");

            this.setter = action;
            this.propertySource = srcProperty.Name;
            this.propertyDestination = destProperty.Name;
        }

        /// <summary>
        /// 
        /// </summary>
        public string PropertySource
        {
            get { return this.propertySource; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string PropertyDestination
        {
            get { return this.propertyDestination; }
        }

        /// <summary>
        /// 
        /// </summary>
        public Action<TSource, TDestination> Setter
        {
            get
            {
                return setter;
            }
        }

    }
}
