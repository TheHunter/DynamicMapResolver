using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynamicMapper.Exceptions
{
    /// <summary>
    /// 
    /// </summary>
    public class MapperParameterException
        : Exception
    {
        private readonly string parameterName;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameterName"></param>
        /// <param name="message"></param>
        public MapperParameterException(string parameterName, string message)
            :base(message)
        {
            this.parameterName = parameterName;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameterName"></param>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public MapperParameterException(string parameterName, string message, Exception innerException)
            : base(message, innerException)
        {
            this.parameterName = parameterName;
        }

        /// <summary>
        /// 
        /// </summary>
        public string ParameterName
        {
            get { return this.parameterName; }
        }
    }
}
