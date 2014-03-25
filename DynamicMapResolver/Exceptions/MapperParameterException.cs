using System;

namespace DynamicMapResolver.Exceptions
{
    /// <summary>
    /// Indicates when a wrong parameter is used.
    /// </summary>
    [Serializable]
    public class MapperParameterException
        : MapperException
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
