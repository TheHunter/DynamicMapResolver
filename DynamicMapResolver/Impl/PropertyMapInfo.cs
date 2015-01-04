using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DynamicMapResolver.Exceptions;

namespace DynamicMapResolver.Impl
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class PropertyMapInfo
        : IPropertyMapInfo
    {
        private readonly string propertySource;
        private readonly string propertyDestination;

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyMapInfo"/> class.
        /// </summary>
        /// <param name="propertySrc">The property source.</param>
        /// <param name="propertyDest">The property dest.</param>
        /// <exception cref="MapperParameterException">
        /// propertySrc;The getter property name cannot be null or empty, you have to use the suitable constructor without property names parameters.
        /// or
        /// propertyDest;The setter property name cannot be null or empty, you have to use the suitable constructor without property names parameters.
        /// </exception>
        protected PropertyMapInfo(string propertySrc, string propertyDest)
        {
            if (propertySrc == null || propertySrc.Trim().Equals(string.Empty))
                throw new MapperParameterException("propertySrc", "The getter property name cannot be null or empty, you have to use the suitable constructor without property names parameters.");

            if (propertyDest == null || propertyDest.Trim().Equals(string.Empty))
                throw new MapperParameterException("propertyDest", "The setter property name cannot be null or empty, you have to use the suitable constructor without property names parameters.");


            this.propertySource = propertySrc;
            this.propertyDestination = propertyDest;
        }

        /// <summary>
        /// The property source which value will set the associated property destination.
        /// </summary>
        public string PropertySource
        {
            get { return propertySource; }
        }

        /// <summary>
        /// The property destination which value will be set from property source value.
        /// </summary>
        public string PropertyDestination
        {
            get { return propertyDestination; }
        }

    }
}
