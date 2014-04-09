using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynamicMapResolver.Exceptions
{
    /// <summary>
    /// 
    /// </summary>
    public class MissingAccessorException
        : MapperException
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public MissingAccessorException(string message) : base(message)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public MissingAccessorException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
