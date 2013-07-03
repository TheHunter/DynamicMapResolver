using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynamicMapper.Exceptions
{
    /// <summary>
    /// 
    /// </summary>
    public class MappingFailedActionException
        : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public MappingFailedActionException(string message)
            : base(message)
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public MappingFailedActionException(string message, Exception innerException)
            :base(message, innerException)
        {
            
        }
    }
}
