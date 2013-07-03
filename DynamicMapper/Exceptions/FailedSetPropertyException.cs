using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynamicMapper.Exceptions
{

    public class FailedSetPropertyException
        : Exception
    {

        public FailedSetPropertyException(string message)
            :base(message)
        {
            
        }

        public FailedSetPropertyException(string message, Exception innerException)
            :base(message, innerException)
        {
            
        }


    }
}
