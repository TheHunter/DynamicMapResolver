using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DynamicMapResolver.Exceptions;
using DynamicMapResolver.Impl;

namespace DynamicMapResolver
{
    /// <summary>
    /// A factory class used for making default / advanced mappers.
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
        /// Build a dynamic mapper for all compatibles properties between source type with destination type.
        /// </summary>
        /// <param name="tSource">The t source.</param>
        /// <param name="tDestination">The t destination.</param>
        /// <returns></returns>
        public static ISourceMapper DynamicResolutionMapper(Type tSource, Type tDestination)
        {
            return new SourceMapper(tSource, tDestination, GetDefaultPropertyMappers(tSource, tDestination));
        }

        /// <summary>
        /// Build a dynamic mapper for all compatibles properties between source type with destination type.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TDestination">The type of the destination.</typeparam>
        /// <returns></returns>
        public static ISourceMapper<TSource, TDestination> DynamicResolutionMapper<TSource, TDestination>()
            where TSource: class
            where TDestination: class
        {
            return FactoryMapper.DynamicResolutionMapper<TSource, TDestination>(null, null);
        }

        /// <summary>
        /// Build a dynamic mapper for all compatibles properties between source type with destination type.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TDestination">The type of the destination.</typeparam>
        /// <param name="beforeMapping">The before mapping.</param>
        /// <param name="afterMapping">The after mapping.</param>
        /// <returns></returns>
        public static ISourceMapper<TSource, TDestination> DynamicResolutionMapper<TSource, TDestination>(Action<TDestination> beforeMapping, Action<TDestination> afterMapping)
            where TSource : class
            where TDestination : class
        {
            return new SourceMapper<TSource, TDestination>(GetDefaultPropertyMappers<TSource, TDestination>(), beforeMapping, afterMapping);
        }

        /// <summary>
        /// Makes the default builder.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TDestination">The type of the destination.</typeparam>
        /// <returns></returns>
        public static ITransformerBuilder<TSource, TDestination> MakeDefaultBuilder<TSource, TDestination>()
            where TSource : class
            where TDestination : class
        {
            return new TransformerBuilder<TSource, TDestination>(GetDefaultPropertyMappers<TSource, TDestination>());
        }

        /// <summary>
        /// Build a dynamic merger for all compatibles properties between source type with destination type.
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
        /// Build a dynamic merger for all compatibles properties between source type with destination type.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TDestination">The type of the destination.</typeparam>
        /// <param name="beforeMapping">The before mapping.</param>
        /// <param name="afterMapping">The after mapping.</param>
        /// <returns></returns>
        public static ISourceMerger<TSource, TDestination> DynamicResolutionMerger<TSource, TDestination>(Action<TDestination> beforeMapping, Action<TDestination> afterMapping)
            where TSource : class
            where TDestination : class
        {
            return new SourceMerger<TSource, TDestination>(GetDefaultPropertyMappers<TSource, TDestination>(), beforeMapping, afterMapping);
        }

        /// <summary>
        /// Gets all properties for the given type due to the given flags.
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

                    var typeProperties = subType.GetOriginalProperties(fl);

                    var newPropertyInfos = typeProperties
                        .Where(x => !propertyInfos.Contains(x)
                                && !propertyInfos.Any(n => n.Name.Equals(x.Name))
                            );

                    propertyInfos.InsertRange(0, newPropertyInfos);
                }
                return propertyInfos.ToArray();
            }
            //return current.GetProperties(fl);
            return current.GetOriginalProperties(fl);
        }

        /// <summary>
        /// Gets the original properties from declared types whenever they are found by derived types.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="flags">The flags.</param>
        /// <returns></returns>
        public static PropertyInfo[] GetOriginalProperties(this Type type, BindingFlags? flags = null)
        {
            BindingFlags fl = flags.HasValue ? flags.Value
                                : (BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.Instance);

            IEnumerable<PropertyInfo> properties = type.GetProperties(fl);

            return properties.Select(info =>
            {
                if (info.DeclaringType == null || info.DeclaringType == info.ReflectedType)
                    return info;

                return info.DeclaringType.GetProperty(info.Name, fl);

            }
                )
                .ToArray()
                ;
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
        /// Gets the default property mappers.
        /// </summary>
        /// <param name="sourceType">Type of the source.</param>
        /// <param name="destinationType">Type of the destination.</param>
        /// <returns></returns>
        public static IEnumerable<IPropertyMapper> GetDefaultPropertyMappers(Type sourceType, Type destinationType)
        {
            return GetDefaultPropertyMappers(sourceType, destinationType, null);
        }

        /// <summary>
        /// Gets the default property mappers.
        /// </summary>
        /// <param name="sourceType">Type of the source.</param>
        /// <param name="destinationType">Type of the destination.</param>
        /// <param name="resolver">The resolver.</param>
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
        /// Makes a dynamic action in order to associate property source with property destination using a dynamic resolver.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TDestination">The type of the destination.</typeparam>
        /// <param name="srcProperty">The source property.</param>
        /// <param name="destProperty">The dest property.</param>
        /// <param name="resolver">The resolver.</param>
        /// <returns></returns>
        /// <exception cref="MissingAccessorException">
        /// No getter method available for retrieving value for setting property destination.
        /// or
        /// No setter method available for setting property destination.
        /// </exception>
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
        /// Checks the properties.
        /// </summary>
        /// <param name="srcProperty">The source property.</param>
        /// <param name="destProperty">The dest property.</param>
        /// <exception cref="InconsistentMappingException">
        /// Property getter is no compatible with property setter.
        /// or
        /// References property getter and setter are incompatibles.
        /// </exception>
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
        /// Gets the getter value.
        /// </summary>
        /// <param name="res">The resource.</param>
        /// <param name="propSetter">The property setter.</param>
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
