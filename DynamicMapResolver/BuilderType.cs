using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynamicMapResolver
{
    /// <summary>
    /// 
    /// </summary>
    public enum BuilderType
    {
        /// <summary>
        /// Indicates an empty transformer builder which can be for adding custom property mapper. 
        /// </summary>
        Empty,

        /// <summary>
        /// Try to build all property mappers identifying all compatible properties between source type and destination type. 
        /// </summary>
        DefaultMappers,

        /// <summary>
        /// It's like DefaultMappers value, but It tries to inject a dynamic resolver for mapping incompatible properties.
        /// </summary>
        DynamicResolver
    }
}
