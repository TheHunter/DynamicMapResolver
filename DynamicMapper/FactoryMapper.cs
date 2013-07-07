using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using DynamicMapper.Impl;

namespace DynamicMapper
{
    /// <summary>
    /// 
    /// </summary>
    public class FactoryMapper
    {
        private static readonly IDictionary<Type, HashSet<Type>> PrimitiveTypes;
        private static readonly Type FunctionGetter;
        private static readonly Type ActionSetter;

        static FactoryMapper()
        {
            FunctionGetter = typeof (Func<,>);
            ActionSetter = typeof (Action<,>);

            PrimitiveTypes = new Dictionary<Type, HashSet<Type>>
                {
                    {typeof (byte?), new HashSet<Type> {typeof (byte)}},
                    {typeof (short), new HashSet<Type> {typeof (byte)}},
                    {typeof (short?), new HashSet<Type> {typeof (byte), typeof (short), typeof (byte?)}},
                    {typeof (int), new HashSet<Type> {typeof (byte), typeof (short)}},
                    {
                        typeof (int?),
                        new HashSet<Type> {typeof (byte), typeof (short), typeof (int), typeof (byte?), typeof (short?)}
                    },
                    {typeof (long), new HashSet<Type> {typeof (byte), typeof (short), typeof (int)}},
                    {
                        typeof (long?),
                        new HashSet<Type>
                            {
                                typeof (byte),
                                typeof (short),
                                typeof (int),
                                typeof (long),
                                typeof (byte?),
                                typeof (short?),
                                typeof (int?)
                            }
                    },
                    {typeof (decimal), new HashSet<Type> {typeof (byte), typeof (short), typeof (int), typeof (long)}},
                    {
                        typeof (decimal?),
                        new HashSet<Type>
                            {
                                typeof (byte),
                                typeof (short),
                                typeof (int),
                                typeof (long),
                                typeof (decimal),
                                typeof (byte?),
                                typeof (short?),
                                typeof (int?),
                                typeof (long?)
                            }
                    },
                    {typeof (float), new HashSet<Type> {typeof (byte), typeof (short), typeof (int), typeof (long)}},
                    {
                        typeof (float?),
                        new HashSet<Type>
                            {
                                typeof (byte),
                                typeof (short),
                                typeof (int),
                                typeof (long),
                                typeof (float),
                                typeof (byte?),
                                typeof (short?),
                                typeof (int?),
                                typeof (long?)
                            }
                    },
                    {
                        typeof (double),
                        new HashSet<Type> {typeof (byte), typeof (short), typeof (int), typeof (long), typeof (float)}
                    },
                    {
                        typeof (double?),
                        new HashSet<Type>
                            {
                                typeof (byte),
                                typeof (short),
                                typeof (int),
                                typeof (long),
                                typeof (float),
                                typeof (double),
                                typeof (byte?),
                                typeof (short?),
                                typeof (int?),
                                typeof (long?),
                                typeof (float?)
                            }
                    }
                };
            
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
            return new SourceMapper<TSource, TDestination>(GetDefaultPropertyMappers<TSource, TDestination>(), null, null);
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
            return new SourceMerger<TSource, TDestination>(GetDefaultPropertyMappers<TSource, TDestination>(), null, null);
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

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDestination"></typeparam>
        /// <returns></returns>
        public static IEnumerable<IPropertyMapper<TSource, TDestination>> GetDefaultPropertyMappers
            <TSource, TDestination>()
            where TSource : class
            where TDestination : class
        {
            IDictionary<PropertyInfo, PropertyInfo> matchedProperties = FactoryMapper.GetSuitedPropertiesOf<TSource, TDestination>();

            HashSet<IPropertyMapper<TSource, TDestination>> mappers = new HashSet<IPropertyMapper<TSource, TDestination>>();

            matchedProperties.All
                (
                    current =>
                    {
                        try
                        {
                            Action<TSource, TDestination> action = DynamicPropertyMap<TSource, TDestination>(current.Key, current.Value);
                            mappers.Add(new PropertyMapper<TSource, TDestination>(action, current.Key.Name, current.Value.Name));
                        }
                        catch
                        {                        
                            // exceptions must be ignore..
                            // so in this case, the properties are not compatibles..
                        }
                        return true;
                    }
                );

            return mappers;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDestination"></typeparam>
        /// <returns></returns>
        public static IDictionary<PropertyInfo, PropertyInfo> GetSuitedPropertiesOf<TSource, TDestination>()
        {
            PropertyInfo[] source = GetPropertiesOf<TSource>();
            PropertyInfo[] destination = GetPropertiesOf<TDestination>();

            Dictionary<PropertyInfo, PropertyInfo> matches = new Dictionary<PropertyInfo, PropertyInfo>();

            destination.All
                (
                    currentProperty =>
                        {
                            PropertyInfo info = source.FirstOrDefault(n => n.Name == currentProperty.Name);
                            if (info != null)
                                matches.Add(info, currentProperty);
                            return true;
                        }
                );

            return matches;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="srcProperty"></param>
        /// <param name="destProperty"></param>
        /// <returns></returns>
        public static Action<TSource, TDestination> DynamicPropertyMap<TSource, TDestination>
            (PropertyInfo srcProperty, PropertyInfo destProperty)
        {
            Type srcPropType = srcProperty.PropertyType;
            Type dstPropType = destProperty.PropertyType;

            CheckProperties(srcProperty, destProperty);

            Type funcType = FunctionGetter.MakeGenericType(srcProperty.DeclaringType, srcPropType);
            Type ActType = ActionSetter.MakeGenericType(destProperty.DeclaringType, dstPropType);

            Delegate getter = Delegate.CreateDelegate(funcType, null, srcProperty.GetGetMethod());
            Delegate setter = Delegate.CreateDelegate(ActType, null, destProperty.GetSetMethod());

            Action<TSource, TDestination> action 
                = (source, destination) => setter.DynamicInvoke(destination, GetGetterValue(getter.DynamicInvoke(source), destProperty.PropertyType));

            return action;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="srcProperty"></param>
        /// <param name="destProperty"></param>
        private static void CheckProperties(PropertyInfo srcProperty, PropertyInfo destProperty)
        {
            if (srcProperty.PropertyType != destProperty.PropertyType)
            {
                if (PrimitiveTypes.ContainsKey(destProperty.PropertyType))
                {
                    if (!PrimitiveTypes[destProperty.PropertyType].Contains(srcProperty.PropertyType))
                        throw new Exception("Property getter is no compatible with property setter.");
                }
                else if (!destProperty.PropertyType.IsAssignableFrom(srcProperty.PropertyType))
                {
                    throw new Exception("References property getter and setter are incompatibles.");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="res"></param>
        /// <param name="propSetter"></param>
        /// <returns></returns>
        private static object GetGetterValue(object res, Type propSetter)
        {
            if (res == null)
                return null;

            if (res.GetType() == propSetter)
                return res;

            if (PrimitiveTypes.ContainsKey(propSetter))
            {
                if (propSetter.Name.StartsWith("Nullable`"))
                {
                    Type typeToConvert = propSetter.GetGenericArguments()[0];
                    res = Convert.ChangeType(res, typeToConvert);
                }
            }
                
            return res;
        }

    }
}
