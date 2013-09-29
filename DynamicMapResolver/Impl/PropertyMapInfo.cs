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
        /// 
        /// </summary>
        /// <param name="propertySrc"></param>
        /// <param name="propertyDest"></param>
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
        /// 
        /// </summary>
        public string PropertySource
        {
            get { return propertySource; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string PropertyDestination
        {
            get { return propertyDestination; }
        }

    }
}
