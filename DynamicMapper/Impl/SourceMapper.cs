using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DynamicMapper.Exceptions;

namespace DynamicMapper.Impl
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TDestination"></typeparam>
    public class SourceMapper<TSource, TDestination>
        : ISourceMapper<TSource, TDestination>
        where TSource : class
        where TDestination : class, new()
    {
        private readonly Action<TDestination> beforeMapping;
        private readonly Action<TDestination> afterMapping;
        private readonly HashSet<IPropertyMapper<TSource, TDestination>> propertyMappers;


        public SourceMapper(IEnumerable<IPropertyMapper<TSource, TDestination>> propertyMappers, Action<TDestination> beforeMapping, Action<TDestination> afterMapping)
        {
            this.propertyMappers = new HashSet<IPropertyMapper<TSource, TDestination>>(propertyMappers);

            if (this.propertyMappers.Count != propertyMappers.Count())
                throw new NonUniqueSetterException("property mappers must be unique, so verify the lambda setter expressions.");

            this.beforeMapping = beforeMapping;
            this.afterMapping = afterMapping;
        }


        public TDestination Map(TSource source)
        {
            TDestination dest = new TDestination();

            try
            {
                propertyMappers.All
                    (
                        mapper =>
                            {
                                mapper.Setter.Invoke(source, dest);
                                return true;
                            }
                    );
            }
            catch (Exception ex)
            {
                if (!this.IgnoreExceptionOnMapping)
                    throw new FailedSetPropertyException("Exception on execution lambda setter action.", ex);
            }

            return dest;
        }

        public Action<TDestination> BeforeMapping
        {
            get { return this.beforeMapping; }
        }

        public Action<TDestination> AfterMapping
        {
            get { return this.afterMapping; }
        }


        public bool IgnoreExceptionOnMapping { get; set; }


        public IEnumerable<IPropertyMapper<TSource, TDestination>> PropertyMappers
        {
            get { return this.propertyMappers; }
        }
    }
}
