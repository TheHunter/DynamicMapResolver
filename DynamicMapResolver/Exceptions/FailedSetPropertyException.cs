using System;

namespace DynamicMapResolver.Exceptions
{
    /// <summary>
    /// Indicates an error when set operations failed.
    /// </summary>
    public class FailedSetPropertyException
        : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public FailedSetPropertyException(string message)
            :base(message)
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public FailedSetPropertyException(string message, Exception innerException)
            :base(message, innerException)
        {
            
        }


    }
}
