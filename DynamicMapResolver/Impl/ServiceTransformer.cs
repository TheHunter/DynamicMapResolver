using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynamicMapResolver.Impl
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TService"></typeparam>
    public class ServiceTransformer<TService>
        : ServiceTransformer
        where TService : class, ISourceTransformer
    {
        private readonly TService service;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceTransformer{TService}"/> class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="service">The service.</param>
        /// <exception cref="System.ArgumentException">The service transformer cannot be null.;service</exception>
        public ServiceTransformer(object key, TService service)
            : base(key, typeof(TService))
        {
            if (service == null)
                throw new ArgumentException("The service transformer cannot be null.", "service");

            this.service = service;
        }

        /// <summary>
        /// Gets the service component.
        /// </summary>
        /// <value>
        /// The service.
        /// </value>
        public TService Service
        {
            get { return this.service; }
        }

        /// <summary>
        /// Compares the given transformer with this one.
        /// </summary>
        /// <param name="keyService">The key service.</param>
        /// <param name="transformer">The transformer.</param>
        /// <returns></returns>
        public override bool ComparableTo(object keyService, ISourceTransformer transformer)
        {
            if (keyService == null || transformer == null)
                return false;

            Type origServiceType = this.service.GetType();
            Type parServiceType = transformer.GetType();

            return this.KeyEquals(keyService)
                && (origServiceType.IsAssignableFrom(parServiceType) || parServiceType.IsAssignableFrom(origServiceType))
                ;
        }

        /// <summary>
        /// Cast this transformer into right / compatible TComponent.
        /// </summary>
        /// <typeparam name="TComponent">The type of the component.</typeparam>
        /// <returns></returns>
        public override TComponent ServiceAs<TComponent>()
        {
            return this.service as TComponent;
        }

        /// <summary>
        /// Matches the specified key service using a fuzzy research on source / destination type.
        /// </summary>
        /// <param name="keyService">The key service.</param>
        /// <param name="sourceType">Type of the source.</param>
        /// <param name="destinationType">Type of the destination.</param>
        /// <returns></returns>
        public override bool Match(object keyService, Type sourceType, Type destinationType)
        {
            return this.KeyEquals(keyService)
                   && this.service.SourceType.IsAssignableFrom(sourceType)
                   && this.service.DestinationType == destinationType;
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (obj is ServiceTransformer<TService>)
                return this.GetHashCode() == obj.GetHashCode();

            return false;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return 7 * (this.service.GetHashCode() - this.ServiceKey.GetHashCode());
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            //return string.Format("Key: {0}, ServiceType<{1}>", this.ServiceKey, service.GetType().Name);
            return string.Format("Key: {0}, ServiceType: {1}", this.ServiceKey, service.GetType().FullName);
        }
    }


    /// <summary>
    /// 
    /// </summary>
    public abstract class ServiceTransformer
    {
        private readonly object key;
        private readonly Type keyType;
        private readonly Type serviceType;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceTransformer"/> class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="serviceType">Type of the service.</param>
        /// <exception cref="System.ArgumentException">
        /// The key of any kind of ServiveType cannot be empty or null.;key
        /// or
        /// The key of any kind of ServiveType cannot be empty or null.;key
        /// </exception>
        protected ServiceTransformer(object key, Type serviceType)
        {
            string strKey = key as string;
            object localKey = key;

            if (key == null)
                throw new ArgumentException("The key of any kind of ServiveType cannot be empty or null.", "key");

            if (strKey != null)
            {
                strKey = strKey.Trim();
                if (strKey.Equals(string.Empty))
                    throw new ArgumentException("The key of any kind of ServiveType cannot be empty or null.", "key");

                localKey = strKey;
            }

            this.key = localKey;
            this.keyType = localKey.GetType();
            this.serviceType = serviceType;
        }

        /// <summary>
        /// Gets the service key associated to this transformer.
        /// </summary>
        /// <value>
        /// The service key.
        /// </value>
        public object ServiceKey
        {
            get { return this.key; }
        }

        /// <summary>
        /// Gets the type of the key.
        /// </summary>
        /// <value>
        /// The type of the key.
        /// </value>
        public Type KeyType
        {
            get { return this.keyType; }
        }

        /// <summary>
        /// Gets the type of the service.
        /// </summary>
        /// <value>
        /// The type of the service.
        /// </value>
        public Type ServiceType
        {
            get { return this.serviceType; }
        }

        /// <summary>
        /// Compares the given key service with the key service managed by this transformer service.
        /// </summary>
        /// <param name="keyService">The key service.</param>
        /// <returns></returns>
        public bool KeyEquals(object keyService)
        {
            if (keyService == null)
                return false;

            return keyService.GetType() == this.KeyType
                   && keyService.GetHashCode() == this.key.GetHashCode();
        }

        /// <summary>
        /// Matches the specified key service.
        /// </summary>
        /// <typeparam name="TTransformer">The type of the transformer.</typeparam>
        /// <param name="keyService">The key service.</param>
        /// <returns></returns>
        public bool Match<TTransformer>(object keyService)
            where TTransformer : class, ISourceTransformer
        {
            return this.Match(keyService, typeof(TTransformer));
        }

        /// <summary>
        /// Matches the specified key service.
        /// </summary>
        /// <param name="keyService">The key service.</param>
        /// <param name="transformerType">Type of the transformer.</param>
        /// <returns></returns>
        public bool Match(object keyService, Type transformerType)
        {
            return (this.KeyEquals(keyService)
                   && this.serviceType == transformerType
                   && this.serviceType.IsAssignableFrom(transformerType))
                ;
        }

        /// <summary>
        /// Matches the specified key service using a fuzzy research on source / destination type.
        /// </summary>
        /// <param name="keyService">The key service.</param>
        /// <param name="sourceType">Type of the source.</param>
        /// <param name="destinationType">Type of the destination.</param>
        /// <returns></returns>
        public abstract bool Match(object keyService, Type sourceType, Type destinationType);

        /// <summary>
        /// Compares the given transformer with this one.
        /// </summary>
        /// <param name="keyService">The key service.</param>
        /// <param name="transformer">The transformer.</param>
        /// <returns></returns>
        public abstract bool ComparableTo(object keyService, ISourceTransformer transformer);

        /// <summary>
        /// Cast this transformer into right / compatible TComponent.
        /// </summary>
        /// <typeparam name="TComponent">The type of the component.</typeparam>
        /// <returns></returns>
        public abstract TComponent ServiceAs<TComponent>() where TComponent : class, ISourceTransformer;

    }

}

