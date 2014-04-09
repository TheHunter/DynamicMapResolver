using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynamicMapResolver.Exceptions
{
    /// <summary>
    /// Indicates an error when It tries to map two inconsistent properties.
    /// </summary>
    public class InconsistentMappingException
        : MapperException
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public InconsistentMappingException(string message) : base(message)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public InconsistentMappingException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
