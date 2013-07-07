﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynamicMapper
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TDestination"></typeparam>
    public interface IPropertyMapper<TSource, TDestination>
        where TSource : class
        where TDestination : class
    {
        /// <summary>
        /// 
        /// </summary>
        string PropertySource { get; }

        /// <summary>
        /// 
        /// </summary>
        string PropertyDestination { get; }

        /// <summary>
        /// 
        /// </summary>
        Action<TSource, TDestination> Setter { get; }
    }
}
