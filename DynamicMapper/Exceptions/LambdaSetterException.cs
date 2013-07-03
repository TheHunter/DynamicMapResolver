using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynamicMapper.Exceptions
{
    public class LambdaSetterException
        : Exception
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
