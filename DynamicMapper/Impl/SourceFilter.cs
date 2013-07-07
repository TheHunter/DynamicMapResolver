using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynamicMapper.Impl
{
    /// <summary>
    /// 
    /// </summary>
    public class SourceFilter
        : ISourceFilter
    {
        private readonly IEnumerable<IPropertyFilter> filters;

        /// <summary>
        /// 
        /// </summary>
        public SourceFilter()
        {
            this.filters = new HashSet<IPropertyFilter>();
        }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<IPropertyFilter> Filters
        {
            get { return this.filters; }
        }
    }
}
