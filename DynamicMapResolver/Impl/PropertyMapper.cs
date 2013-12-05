using System;
using System.Reflection;
using DynamicMapResolver.Exceptions;

namespace DynamicMapResolver.Impl
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TDestination"></typeparam>
    public class PropertyMapper<TSource, TDestination>
        : PropertyMapInfo, IPropertyMapper<TSource, TDestination>
        where TSource : class
        where TDestination : class
    {
        private readonly Action<TSource, TDestination> setter;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="setter"></param>
        public PropertyMapper(Action<TSource, TDestination> setter)
            : this(setter, "anonymous", "anonymous")
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="setter"></param>
        /// <param name="propertySrc"></param>
        /// <param name="propertyDest"></param>
        public PropertyMapper(Action<TSource, TDestination> setter, string propertySrc, string propertyDest)
            : base(propertySrc, propertyDest)
        {
            if (setter == null)
                throw new LambdaSetterException("The setter action for property mapper cannot be null.");

            this.setter = setter;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="srcProperty"></param>
        /// <param name="destProperty"></param>
        public PropertyMapper(PropertyInfo srcProperty, PropertyInfo destProperty)
            : base(srcProperty.Name, destProperty.Name)
        {
            Action<TSource, TDestination> action = FactoryMapper.DynamicPropertyMap<TSource, TDestination>(srcProperty, destProperty);
            if (action == null)
                throw new LambdaSetterException("The setter action for property mapper cannot be null.");

            this.setter = action;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertySrc"></param>
        /// <param name="propertyDest"></param>
        public PropertyMapper(string propertySrc, string propertyDest)
            : base(propertySrc, propertyDest)
        {
            PropertyInfo srcProperty;
            PropertyInfo destProperty;
            try
            {
                srcProperty = typeof(TSource).GetProperty(propertySrc);
                destProperty = typeof(TSource).GetProperty(propertyDest);
            }
            catch (Exception ex)
            {
                throw new MapperParameterException("Unknown", string.Format("An error occurs when property source (name: {0}) or property destination (name: {1}) was used", propertySrc, propertyDest), ex);
            }

            if (srcProperty == null)
                throw new MissingMemberException(typeof(TSource).Name, propertySrc);

            if (destProperty == null)
                throw new MissingMemberException(typeof(TSource).Name, propertyDest);

            Action<TSource, TDestination> action = FactoryMapper.DynamicPropertyMap<TSource, TDestination>(srcProperty, destProperty);
            if (action == null)
                throw new LambdaSetterException("The setter action for property mapper cannot be null.");

            this.setter = action;
        }

        /// <summary>
        /// 
        /// </summary>
        public Action<TSource, TDestination> Setter
        {
            get { return setter; }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("Property setter: {0}, Property getter: {1}, Action: {2}", this.PropertyDestination, this.PropertyDestination, this.Setter);
        }
    }


    /// <summary>
    /// 
    /// </summary>
    internal class PropertyMapper
        : PropertyMapper<object, object>, IPropertyMapper
    {
        private readonly PropertyInfo srcProperty;
        private readonly PropertyInfo destProperty;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="srcProperty"></param>
        /// <param name="destProperty"></param>
        internal PropertyMapper(PropertyInfo srcProperty, PropertyInfo destProperty)
            : base(srcProperty, destProperty)
        {
            this.srcProperty = srcProperty;
            this.destProperty = destProperty;
        }


        public PropertyInfo SrcPropertyInfo { get { return this.srcProperty; } }

        
        public PropertyInfo destPropertyInfo { get { return this.destProperty; } }

    }

}
