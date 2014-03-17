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
    public class ServiceType<TService>
        where TService : class, ISourceTransformer
    {
        private readonly object key;
        private readonly Type keyType;
        private readonly TService service;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="service"></param>
        public ServiceType(string key, TService service)
        {
            if (key == null || key.Trim().Equals(string.Empty))
                throw new ArgumentException("The key of any kind of ServiveType cannot be empty or null.", "key");

            if (service == null)
                throw new ArgumentException("The service to resolve cannot be null.", "service");

            this.key = key.Trim();
            this.service = service;
            this.keyType = key.GetType();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="service"></param>
        public ServiceType(object key, TService service)
        {
            if (key == null)
                throw new ArgumentException("The key of any kind of ServiveType cannot null.", "key");

            if (service == null)
                throw new ArgumentException("The service to resolve cannot be null.", "service");

            this.key = key;
            this.service = service;
            this.keyType = key.GetType();
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
        /// <param name="keyService"></param>
        /// <returns></returns>
        public bool Match(object keyService)
        {
            if (keyService == null)
                return false;

            return keyService.GetType() == this.KeyType
                   && keyService.GetHashCode() == this.GetHashCode();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj is ServiceType<TService>)
                return this.GetHashCode() == obj.GetHashCode();

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return this.key.GetHashCode();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("Key: {0}, ServiceType<{1}>", this.key, typeof(TService).Name);
        }
    }
}

