using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynamicMapper
{
    /// <summary>
    /// 
    /// </summary>
    public interface ISourceFilter
    {
        /// <summary>
        /// 
        /// </summary>
        IEnumerable<IPropertyFilter> Filters { get; }
    }
}
