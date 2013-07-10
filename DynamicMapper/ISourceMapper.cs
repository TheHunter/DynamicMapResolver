﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynamicMapper
{
    /// <summary>
    /// A simple definition for mapping object data source into object data destination.
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TDestination"></typeparam>
    public interface ISourceMapper<TSource, TDestination>
        : ISourceTransformer<TSource, TDestination>
        where TSource : class
        where TDestination : class, new()
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        TDestination Map(TSource source);
    }
}
