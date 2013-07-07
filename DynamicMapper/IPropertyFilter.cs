using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynamicMapper
{
    /// <summary>
    /// 
    /// </summary>
    public interface IPropertyFilter
    {
        /// <summary>
        /// 
        /// </summary>
        string PropertyName { get; }

        /// <summary>
        /// 
        /// </summary>
        bool IsActive { get; set; }
    }
}
