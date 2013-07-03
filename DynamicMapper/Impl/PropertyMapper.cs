using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using DynamicMapper.Exceptions;

namespace DynamicMapper.Impl
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TDestination"></typeparam>
    public class PropertyMapper<TSource, TDestination>
        : IPropertyMapper<TSource, TDestination>
        where TSource : class
        where TDestination : class
    {

        private readonly Action<TSource, TDestination> setter;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="setter"></param>
        public PropertyMapper(Action<TSource, TDestination> setter)
        {
            if (setter == null)
                throw new LambdaSetterException("The setter action for property mapper cannot be null.");

            this.setter = setter;
        }

        /// <summary>
        /// 
        /// </summary>
        public Action<TSource, TDestination> Setter
        {
            get
            {
                return setter;
            }
        }

        
    }
}
