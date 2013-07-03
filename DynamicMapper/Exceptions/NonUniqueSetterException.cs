using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynamicMapper.Exceptions
{
    /// <summary>
    /// 
    /// </summary>
    public class NonUniqueSetterException
        : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public NonUniqueSetterException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public NonUniqueSetterException(string message, Exception innerException)
            :base(message, innerException)
        {
            
        }
    }
}
