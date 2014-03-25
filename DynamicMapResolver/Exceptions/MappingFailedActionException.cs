using System;

namespace DynamicMapResolver.Exceptions
{
    /// <summary>
    /// Indicates an error when BeforeMapping / AfterMapping action is executed.
    /// </summary>
    [Serializable]
    public class MappingFailedActionException
        : MapperException
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
