using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DynamicMapResolver.Exceptions;

namespace DynamicMapResolver.Impl
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TDestination"></typeparam>
    public class ActionTransformer<TSource, TDestination>
        : SourceTransformer
    {
        private readonly Action<TDestination> beforeMapping;
        private Action<TSource, TDestination> onTransforming;
        private readonly Action<TDestination> afterMapping;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceType"></param>
        /// <param name="destinationType"></param>
        /// <param name="beforeMapping"></param>
        /// <param name="afterMapping"></param>
        protected ActionTransformer(Type sourceType, Type destinationType, Action<TDestination> beforeMapping, Action<TDestination> afterMapping)
            : base(sourceType, destinationType)
        {
            this.beforeMapping = beforeMapping;
            this.afterMapping = afterMapping;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceType"></param>
        /// <param name="destinationType"></param>
        /// <param name="beforeMapping"></param>
        /// <param name="afterMapping"></param>
        /// <param name="onTransforming"></param>
        public ActionTransformer(Type sourceType, Type destinationType, Action<TDestination> beforeMapping, Action<TDestination> afterMapping, Action<TSource, TDestination> onTransforming)
            : this(sourceType, destinationType, beforeMapping, afterMapping)
        {
            if (onTransforming == null)
                throw new MapperParameterException("OnTransforming", "Expression for transforming source instance cannot be null.");

            this.onTransforming = onTransforming;
        }

        /// <summary>
        /// 
        /// </summary>
        protected Action<TDestination> BeforeMapping
        {
            get { return this.beforeMapping; }
        }

        /// <summary>
        /// 
        /// </summary>
        protected Action<TDestination> AfterMapping
        {
            get { return this.afterMapping; }
        }

        /// <summary>
        /// 
        /// </summary>
        protected Action<TSource, TDestination> OnTransforming
        {
            get { return this.onTransforming; }
            set { this.onTransforming = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <returns></returns>
        public virtual TDestination Transform(TSource source, TDestination destination)
        {
            #region Executing BeforeMappingAction
            try
            {
                if (this.BeforeMapping != null)
                    this.BeforeMapping.Invoke(destination);
            }
            catch (Exception ex)
            {
                throw new MappingFailedActionException("Error on executing BeforeMapping action.", ex);
            }
            #endregion

            try
            {
                if (onTransforming == null)
                    throw new MapperParameterException("OnTransforming", "Expression for transforming source instance cannot be null.");

                this.onTransforming.Invoke(source, destination);
            }
            catch (Exception ex)
            {
                if (!this.IgnoreExceptionOnMapping)
                    throw new FailedSetPropertyException("Exception on executing lambda setter action.", ex);
            }

            #region Executing AfterMapping Action.
            try
            {
                if (this.AfterMapping != null)
                    this.AfterMapping.Invoke(destination);
            }
            catch (Exception ex)
            {
                throw new MappingFailedActionException("Error on executing AfterMapping action.", ex);
            }
            #endregion

            return destination;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj is ActionTransformer<TSource, TDestination>)
                return this.GetHashCode() == obj.GetHashCode();

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return typeof(ActionTransformer<TSource, TDestination>).GetHashCode() + base.GetHashCode();
        }

    }
}
