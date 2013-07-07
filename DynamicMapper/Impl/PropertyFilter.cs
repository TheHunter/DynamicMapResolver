using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynamicMapper.Impl
{
    /// <summary>
    /// 
    /// </summary>
    public class PropertyFilter
        : IPropertyFilter
    {
        private readonly string propertyName;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyName"></param>
        internal protected PropertyFilter(string propertyName)
        {
            this.propertyName = propertyName;
            this.IsActive = true;
        }

        /// <summary>
        /// 
        /// </summary>
        public string PropertyName
        {
            get { return this.propertyName; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsActive { get; set; }
    }

}
