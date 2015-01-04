using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynamicMapResolver
{
    /// <summary>
    /// Rappresents basic information about a property mapper indicating the names of involved properties.
    /// </summary>
    public interface IPropertyMapInfo
    {
        /// <summary>
        /// The property source which value will set the associated property destination.
        /// </summary>
        string PropertySource { get; }

        /// <summary>
        /// The property destination which value will be set from property source value.
        /// </summary>
        string PropertyDestination { get; }
    }
}
