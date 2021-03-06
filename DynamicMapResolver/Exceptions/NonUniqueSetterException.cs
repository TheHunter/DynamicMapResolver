﻿using System;

namespace DynamicMapResolver.Exceptions
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class NonUniqueSetterException
        : MapperException
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
