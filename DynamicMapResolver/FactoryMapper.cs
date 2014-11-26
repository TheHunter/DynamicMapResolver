using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DynamicMapResolver.Exceptions;
using DynamicMapResolver.Impl;

namespace DynamicMapResolver
{
    /// <summary>
    /// A factory class used for making default mappers.
    /// </summary>
    public static class FactoryMapper
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
        /// <returns></returns>
        public static ISourceMapper DynamicResolutionMapper(Type tSource, Type tDestination)
        {
            return new SourceMapper(tSource, tDestination, GetDefaultPropertyMappers(tSource, tDestination));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDestination"></typeparam>
        /// <returns></returns>
        public static ISourceMapper<TSource, TDestination> DynamicResolutionMapper<TSource, TDestination>()
            where TSource: class
            where TDestination: class
        {
            return FactoryMapper.DynamicResolutionMapper<TSource, TDestination>(null, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="beforeMapping"></param>
        /// <param name="afterMapping"></param>
        /// <returns></returns>
        public static ISourceMapper<TSource, TDestination> DynamicResolutionMapper<TSource, TDestination>(Action<TDestination> beforeMapping, Action<TDestination> afterMapping)
            where TSource : class
            where TDestination : class
        {
            return new SourceMapper<TSource, TDestination>(GetDefaultPropertyMappers<TSource, TDestination>(), beforeMapping, afterMapping);
        }

        /// <summary>
        /// Initialize a new transformer builder with default property mappers.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDestination"></typeparam>
        /// <returns></returns>
        public static ITransformerBuilder<TSource, TDestination> MakeDefaultBuilder<TSource, TDestination>()
            where TSource : class
            where TDestination : class
        {
            return new TransformerBuilder<TSource, TDestination>(GetDefaultPropertyMappers<TSource, TDestination>());
        }

        /// <summary>
        /// Get a default ISourceMerger for the given types.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDestination"></typeparam>
        /// <returns></returns>
        public static ISourceMerger<TSource, TDestination> DynamicResolutionMerger<TSource, TDestination>()
            where TSource : class
            where TDestination : class
        {
            return FactoryMapper.DynamicResolutionMerger<TSource, TDestination>(null, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="beforeMapping"></param>
        /// <param name="afterMapping"></param>
        /// <returns></returns>
        public static ISourceMerger<TSource, TDestination> DynamicResolutionMerger<TSource, TDestination>(Action<TDestination> beforeMapping, Action<TDestination> afterMapping)
            where TSource : class
            where TDestination : class
        {
            return new SourceMerger<TSource, TDestination>(GetDefaultPropertyMappers<TSource, TDestination>(), beforeMapping, afterMapping);
        }

        /// <summary>
        /// Gets all public properties for the given type.
        /// </summary>
        /// <returns></returns>
        public static PropertyInfo[] GetPropertiesOf(Type current, BindingFlags? flags = null)
        {
            //return current.GetProperties();

            BindingFlags fl = flags.HasValue ? flags.Value
                                : (BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.Instance);

            if (current.IsInterface)
            {
                var propertyInfos = new List<PropertyInfo>();

                var considered = new List<Type>();
                var queue = new Queue<Type>();
                considered.Add(current);
                queue.Enqueue(current);

                while (queue.Count > 0)
                {
                    var subType = queue.Dequeue();
                    foreach (var subInterface in subType.GetInterfaces())
                    {
                        if (considered.Contains(subInterface))
                            continue;

                        considered.Add(subInterface);
                        queue.Enqueue(subInterface);
                    }

                    var typeProperties = subType.GetProperties(fl);

                    var newPropertyInfos = typeProperties
                        .Where(x => !propertyInfos.Contains(x)
                                && !propertyInfos.Any(n => n.Name.Equals(x.Name))
                            );

                    propertyInfos.InsertRange(0, newPropertyInfos);
                }
                return propertyInfos.ToArray();
            }
            return current.GetProperties(fl);
        }

        /// <summary>
        /// Get a dictionary which every couple corrispond to a property from TSurce type (as Key) and a property from second given type (as value)
        /// with the same name.
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<KeyValuePair<PropertyInfo, PropertyInfo>> GetSuitedPropertiesOf(Type tSource, Type tDestination)
        {
            PropertyInfo[] source = GetPropertiesOf(tSource);
            PropertyInfo[] destination = GetPropertiesOf(tDestination);

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
        /// Get all default property mappers for the given types.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDestination"></typeparam>
        /// <returns></returns>
        public static IEnumerable<IPropertyMapper<TSource, TDestination>> GetDefaultPropertyMappers
            <TSource, TDestination>()
            where TSource : class
            where TDestination : class
        {
            return GetDefaultPropertyMappers<TSource, TDestination>(null);
        }

        /// <summary>
        /// Get all default property mappers for the given types.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDestination"></typeparam>
        /// <returns></returns>
        public static IEnumerable<IPropertyMapper<TSource, TDestination>> GetDefaultPropertyMappers
            <TSource, TDestination>(ITransformerResolver resolver)
            where TSource : class
            where TDestination : class
        {
            IEnumerable<KeyValuePair<PropertyInfo, PropertyInfo>> matchedProperties = FactoryMapper.GetSuitedPropertiesOf(typeof(TSource), typeof(TDestination));
            HashSet<IPropertyMapper<TSource, TDestination>> mappers = new HashSet<IPropertyMapper<TSource, TDestination>>();
            
            foreach (var current in matchedProperties)
            {
                try
                {
                    Action<TSource, TDestination> action = DynamicPropertyMap<TSource, TDestination>(current.Key,
                                                                                                     current.Value);
                    mappers.Add(new PropertyMapper<TSource, TDestination>(action, current.Key.Name, current.Value.Name));
                }
                catch (InconsistentMappingException)
                {
                    // exceptions must be managed..
                    // so in this means that properties are not compatibles..
                    try
                    {
                        if (resolver != null)
                        {
                            Action<TSource, TDestination> action = DynamicPropertyMap<TSource, TDestination>(current.Key, current.Value, resolver);
                            mappers.Add(new PropertyMapper<TSource, TDestination>(action));
                        }
                    }
                    catch
                    {
                    }
                }
                catch
                {
                }
            }
            return mappers;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceType"></param>
        /// <param name="destinationType"></param>
        /// <returns></returns>
        public static IEnumerable<IPropertyMapper> GetDefaultPropertyMappers(Type sourceType, Type destinationType)
        {
            return GetDefaultPropertyMappers(sourceType, destinationType, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceType"></param>
        /// <param name="destinationType"></param>
        /// <param name="resolver"></param>
        /// <returns></returns>
        public static IEnumerable<IPropertyMapper> GetDefaultPropertyMappers(Type sourceType, Type destinationType, ITransformerResolver resolver)
        {
            IEnumerable<KeyValuePair<PropertyInfo, PropertyInfo>> matchedProperties = FactoryMapper.GetSuitedPropertiesOf(sourceType, destinationType);
            HashSet<IPropertyMapper> mappers = new HashSet<IPropertyMapper>();

            foreach (var current in matchedProperties)
            {
                try
                {
                    mappers.Add(new PropertyMapper(current.Key, current.Value));
                }
                catch (InconsistentMappingException)
                {
                    // exceptions must be managed..
                    // so in this means that properties are not compatibles..
                    try
                    {
                        if (resolver != null)
                            mappers.Add(new PropertyMapper(current.Key, current.Value, resolver));
                    }
                    catch
                    {
                    }
                }
                catch
                {
                }
            }

            return mappers;
        }


        /// <summary>
        /// Makes an action which corrispond to set a destination property with the current TSource property value.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="srcProperty">The PropertyInfo which serves to get the Getter method.</param>
        /// <param name="destProperty">The PropertyInfo which serves to get the Setter method.</param>
        /// <returns></returns>
        public static Action<TSource, TDestination> DynamicPropertyMap<TSource, TDestination>
            (PropertyInfo srcProperty, PropertyInfo destProperty)
        {
            Type srcPropType = srcProperty.PropertyType;
            Type dstPropType = destProperty.PropertyType;

            CheckProperties(srcProperty, destProperty);

            Type funcType = FunctionGetter.MakeGenericType(srcProperty.DeclaringType, srcPropType);
            Type actType = ActionSetter.MakeGenericType(destProperty.DeclaringType, dstPropType);

            MethodInfo getterMethod = srcProperty.GetGetMethod() ?? srcProperty.GetGetMethod(true);
            MethodInfo setterMethod = destProperty.GetSetMethod() ?? destProperty.GetSetMethod(true);

            if (getterMethod == null)
                throw new MissingAccessorException("No getter method available for retrieving value for setting property destination.");

            if (setterMethod == null)
                throw new MissingAccessorException("No setter method available for setting property destination.");

            Delegate getter = Delegate.CreateDelegate(funcType, null, getterMethod);
            Delegate setter = Delegate.CreateDelegate(actType, null, setterMethod);

            Action<TSource, TDestination> action 
                = (source, destination) => setter.DynamicInvoke(destination, GetGetterValue(getter.DynamicInvoke(source), destProperty.PropertyType));

            return action;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="srcProperty"></param>
        /// <param name="destProperty"></param>
        /// <param name="resolver"></param>
        /// <returns></returns>
        public static Action<TSource, TDestination> DynamicPropertyMap<TSource, TDestination>
            (PropertyInfo srcProperty, PropertyInfo destProperty, ITransformerResolver resolver)
        {
            Type srcPropType = srcProperty.PropertyType;
            Type dstPropType = destProperty.PropertyType;

            Type funcType = FunctionGetter.MakeGenericType(srcProperty.DeclaringType, srcPropType);
            Type actType = ActionSetter.MakeGenericType(destProperty.DeclaringType, dstPropType);

            MethodInfo getterMethod = srcProperty.GetGetMethod() ?? srcProperty.GetGetMethod(true);
            MethodInfo setterMethod = destProperty.GetSetMethod() ?? destProperty.GetSetMethod(true);

            if (getterMethod == null)
                throw new MissingAccessorException("No getter method available for retrieving value for setting property destination.");

            if (setterMethod == null)
                throw new MissingAccessorException("No setter method available for setting property destination.");

            Delegate getter = Delegate.CreateDelegate(funcType, null, getterMethod);
            Delegate setter = Delegate.CreateDelegate(actType, null, setterMethod);

            Action<TSource, TDestination> action
                = (source, destination) => setter.DynamicInvoke(destination, resolver.TryToMap(getter.DynamicInvoke(source), destProperty.PropertyType));

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
                        throw new InconsistentMappingException("Property getter is no compatible with property setter.");
                }
                else if (!destProperty.PropertyType.IsAssignableFrom(srcProperty.PropertyType))
                {
                    throw new InconsistentMappingException("References property getter and setter are incompatibles.");
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
