using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynamicMapper
{
    public class FactoryMapper
    {
        public static ISourceMapper<TSource, TDestination> MakeSourceMapper<TSource, TDestination>()
            where TSource: class
            where TDestination: class, new()
        {
            return null;
        }
    }
}
