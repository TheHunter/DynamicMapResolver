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
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="service"></param>
        public ServiceTransformer(string key, TService service)
            :base (key, typeof(TService))
        {
            if (service == null)
                throw new ArgumentException("The service to resolve cannot be null.", "service");

            this.service = service;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="service"></param>
        public ServiceTransformer(object key, TService service)
            : base(key, typeof(TService))
        {
            if (service == null)
                throw new ArgumentException("The service to resolve cannot be null.", "service");

            this.service = service;
        }

        /// <summary>
        /// 
        /// </summary>
        public TService Service
        {
            get { return this.service; }
        }

        public override TComponent ServiceAs<TComponent>()
        {
            return this.service as TComponent;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keyService"></param>
        /// <param name="sourceType"></param>
        /// <param name="destinationType"></param>
        /// <returns></returns>
        public override bool Match(object keyService, Type sourceType, Type destinationType)
        {
            return this.KeyEquals(keyService)
                   && this.service.SourceType.IsAssignableFrom(sourceType)
                   && this.service.DestinationType == destinationType;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj is ServiceTransformer<TService>)
                return this.GetHashCode() == obj.GetHashCode();

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return 7 * (this.service.GetHashCode() - this.ServiceKey.GetHashCode());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("Key: {0}, ServiceType<{1}>", this.ServiceKey, service.GetType().Name);
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
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="serviceType"></param>
        protected ServiceTransformer(string key, Type serviceType)
        {
            if (key == null || key.Trim().Equals(string.Empty))
                throw new ArgumentException("The key of any kind of ServiveType cannot be empty or null.", "key");

            this.key = key.Trim();
            this.keyType = key.GetType();
            this.serviceType = serviceType;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="serviceType"></param>
        protected ServiceTransformer(object key, Type serviceType)
        {
            if (key == null)
                throw new ArgumentException("The key of any kind of ServiveType cannot be empty or null.", "key");

            this.key = key;
            this.keyType = key.GetType();
            this.serviceType = serviceType;
        }

        /// <summary>
        /// 
        /// </summary>
        public object ServiceKey
        {
            get { return this.key; }
        }

        /// <summary>
        /// 
        /// </summary>
        public Type KeyType
        {
            get { return this.keyType; }
        }

        /// <summary>
        /// 
        /// </summary>
        public Type ServiceType
        {
            get { return this.serviceType; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keyService"></param>
        /// <returns></returns>
        public bool KeyEquals(object keyService)
        {
            if (keyService == null)
                return false;

            return keyService.GetType() == this.KeyType
                   && keyService.GetHashCode() == this.key.GetHashCode();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keyService"></param>
        /// <returns></returns>
        public bool Match<TTransformer>(object keyService)
            where TTransformer : class, ISourceTransformer
        {
            return this.Match(keyService, typeof(TTransformer));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keyService"></param>
        /// <param name="transformerType"></param>
        /// <returns></returns>
        public bool Match(object keyService, Type transformerType)
        {
            return (this.serviceType.IsAssignableFrom(transformerType))
                   && this.serviceType == transformerType
                   && this.KeyEquals(keyService)
                ;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keyService"></param>
        /// <param name="sourceType"></param>
        /// <param name="destinationType"></param>
        /// <returns></returns>
        public abstract bool Match(object keyService, Type sourceType, Type destinationType);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TComponent"></typeparam>
        /// <returns></returns>
        public abstract TComponent ServiceAs<TComponent>() where TComponent : class, ISourceTransformer;

    }

}

