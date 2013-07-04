using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace DynamicMapper
{
    /// <summary>
    /// 
    /// </summary>
    public class FactoryMapper
    {
        private IDictionary<Type, HashSet<Type>> primiteTypes;

        static FactoryMapper()
        {
            //Dictionary<Type, HashSet<Type>> primiteTypes = new Dictionary<Type, HashSet<Type>>();

            //Type currentKey;
            //HashSet<Type> col;

            //currentKey = typeof (byte);
            //col = new HashSet<Type>();
            //col.Add(currentKey);
            //primiteTypes.Add(currentKey, col);

            //currentKey = typeof(byte?);
            //col = new HashSet<Type>(col);
            //col.Add(currentKey);
            //primiteTypes.Add(currentKey, col);



            //currentKey = typeof(short);
            //col = new HashSet<Type>();
            //col.Add(typeof (byte));
            //col.Add(currentKey);
            //primiteTypes.Add(currentKey, col);

            //currentKey = typeof(short?);
            //col = new HashSet<Type>(col);
            //col.Add(typeof(byte?));
            //col.Add(currentKey);
            //primiteTypes.Add(currentKey, col);



            //currentKey = typeof(int);
            //col = new HashSet<Type>();
            //col.Add(typeof(byte));
            //col.Add(typeof (short));
            //col.Add(currentKey);
            //primiteTypes.Add(currentKey, col);

            //currentKey = typeof(int?);
            //col = new HashSet<Type>(col);
            //col.Add(typeof(byte?));
            //col.Add(typeof(short?));
            //col.Add(currentKey);
            //primiteTypes.Add(currentKey, col);



        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDestination"></typeparam>
        /// <returns></returns>
        public static ISourceMapper<TSource, TDestination> DynamicResolutionMapper<TSource, TDestination>()
            where TSource: class
            where TDestination: class, new()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDestination"></typeparam>
        /// <returns></returns>
        public static ISourceMerger<TSource, TDestination> DynamicResolutionMerger<TSource, TDestination>()
            where TSource : class
            where TDestination : class
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TCurrent"></typeparam>
        /// <returns></returns>
        public static PropertyInfo[] GetPropertiesOf<TCurrent>()
        {
            return typeof (TCurrent).GetProperties();
        }

        public static IDictionary<PropertyInfo, PropertyInfo> GetSuitedPropertiesOf<TSource, TDestination>()
        {
            PropertyInfo[] source = GetPropertiesOf<TSource>();


            return null;
        }
    }
}
