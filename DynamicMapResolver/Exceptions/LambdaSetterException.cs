﻿using System;

namespace DynamicMapResolver.Exceptions
{
    /// <summary>
    /// Indicates an error when the action setter is null.
    /// </summary>
    [Serializable]
    public class LambdaSetterException
        : MapperException
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public LambdaSetterException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public LambdaSetterException(string message, Exception innerException)
            :base(message, innerException)
        {
        }
    }
}
