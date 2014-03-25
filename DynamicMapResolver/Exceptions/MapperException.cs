using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynamicMapResolver.Exceptions
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class MapperException
        : Exception 
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public MapperException(string message)
            :base(message)
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public MapperException(string message, Exception innerException)
            :base(message, innerException)
        {
            
        }
    }
}
