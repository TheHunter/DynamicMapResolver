using System;
using System.Reflection;
using DynamicMapResolver.Exceptions;

namespace DynamicMapResolver.Impl
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TSource">The type of the source.</typeparam>
    /// <typeparam name="TDestination">The type of the destination.</typeparam>
    public class PropertyMapper<TSource, TDestination>
        : PropertyMapInfo, IPropertyMapper<TSource, TDestination>
        where TSource : class
        where TDestination : class
    {
        private readonly Action<TSource, TDestination> setter;

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyMapper{TSource, TDestination}"/> class.
        /// </summary>
        /// <param name="setter">The setter.</param>
        public PropertyMapper(Action<TSource, TDestination> setter)
            : this(setter, "anonymous", "anonymous")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyMapper{TSource, TDestination}"/> class.
        /// </summary>
        /// <param name="setter">The setter.</param>
        /// <param name="propertySrc">The property source.</param>
        /// <param name="propertyDest">The property dest.</param>
        /// <exception cref="LambdaSetterException">The setter action for property mapper cannot be null.</exception>
        public PropertyMapper(Action<TSource, TDestination> setter, string propertySrc, string propertyDest)
            : base(propertySrc, propertyDest)
        {
            if (setter == null)
                throw new LambdaSetterException("The setter action for property mapper cannot be null.");

            this.setter = setter;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyMapper{TSource, TDestination}"/> class.
        /// </summary>
        /// <param name="srcProperty">The source property.</param>
        /// <param name="destProperty">The dest property.</param>
        /// <exception cref="LambdaSetterException">The setter action for property mapper cannot be null.</exception>
        public PropertyMapper(PropertyInfo srcProperty, PropertyInfo destProperty)
            : base(srcProperty.Name, destProperty.Name)
        {
            Action<TSource, TDestination> action = FactoryMapper.DynamicPropertyMap<TSource, TDestination>(srcProperty, destProperty);
            if (action == null)
                throw new LambdaSetterException("The setter action for property mapper cannot be null.");

            this.setter = action;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyMapper{TSource, TDestination}"/> class.
        /// </summary>
        /// <param name="srcProperty">The source property.</param>
        /// <param name="destProperty">The dest property.</param>
        /// <param name="resolver">The resolver.</param>
        /// <exception cref="LambdaSetterException">The setter action for property mapper cannot be null.</exception>
        public PropertyMapper(PropertyInfo srcProperty, PropertyInfo destProperty, ITransformerResolver resolver)
            : base("[resolver]", destProperty.Name)
        {
            Action<TSource, TDestination> action = FactoryMapper.DynamicPropertyMap<TSource, TDestination>(srcProperty, destProperty, resolver);
            if (action == null)
                throw new LambdaSetterException("The setter action for property mapper cannot be null.");

            this.setter = action;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyMapper{TSource, TDestination}"/> class.
        /// </summary>
        /// <param name="propertySrc">The property source.</param>
        /// <param name="propertyDest">The property dest.</param>
        /// <exception cref="MapperParameterException">Unknown</exception>
        /// <exception cref="System.MissingMemberException">
        /// </exception>
        /// <exception cref="LambdaSetterException">The setter action for property mapper cannot be null.</exception>
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
        /// The action to execute for setting the destionation property value.
        /// </summary>
        public Action<TSource, TDestination> Setter
        {
            get { return setter; }
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
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
        /// Initializes a new instance of the <see cref="PropertyMapper"/> class.
        /// </summary>
        /// <param name="srcProperty">The source property.</param>
        /// <param name="destProperty">The dest property.</param>
        internal PropertyMapper(PropertyInfo srcProperty, PropertyInfo destProperty)
            : base(srcProperty, destProperty)
        {
            this.srcProperty = srcProperty;
            this.destProperty = destProperty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyMapper"/> class.
        /// </summary>
        /// <param name="srcProperty">The source property.</param>
        /// <param name="destProperty">The dest property.</param>
        /// <param name="resolver">The resolver.</param>
        internal PropertyMapper(PropertyInfo srcProperty, PropertyInfo destProperty, ITransformerResolver resolver)
            : base(srcProperty, destProperty, resolver)
        {
            this.destProperty = destProperty;
        }

        /// <summary>
        /// Gets the source property information.
        /// </summary>
        /// <value>
        /// The source property information.
        /// </value>
        public PropertyInfo SrcPropertyInfo { get { return this.srcProperty; } }

        /// <summary>
        /// Gets the dest property information.
        /// </summary>
        /// <value>
        /// The dest property information.
        /// </value>
        public PropertyInfo destPropertyInfo { get { return this.destProperty; } }

    }

}
