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
    /// <typeparam name="TSource">The type of the source.</typeparam>
    /// <typeparam name="TDestination">The type of the destination.</typeparam>
    public class ActionTransformer<TSource, TDestination>
        : SourceTransformer
    {
        private readonly Action<TDestination> beforeMapping;
        private Action<TSource, TDestination> onTransforming;
        private readonly Action<TDestination> afterMapping;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionTransformer{TSource, TDestination}"/> class.
        /// </summary>
        /// <param name="sourceType">Type of the source.</param>
        /// <param name="destinationType">Type of the destination.</param>
        /// <param name="beforeMapping">The before mapping.</param>
        /// <param name="afterMapping">The after mapping.</param>
        protected ActionTransformer(Type sourceType, Type destinationType, Action<TDestination> beforeMapping, Action<TDestination> afterMapping)
            : base(sourceType, destinationType)
        {
            this.beforeMapping = beforeMapping;
            this.afterMapping = afterMapping;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionTransformer{TSource, TDestination}"/> class.
        /// </summary>
        /// <param name="sourceType">Type of the source.</param>
        /// <param name="destinationType">Type of the destination.</param>
        /// <param name="beforeMapping">The before mapping.</param>
        /// <param name="afterMapping">The after mapping.</param>
        /// <param name="onTransforming">The on transforming.</param>
        /// <exception cref="MapperParameterException">OnTransforming;Expression for transforming source instance cannot be null.</exception>
        public ActionTransformer(Type sourceType, Type destinationType, Action<TDestination> beforeMapping, Action<TDestination> afterMapping, Action<TSource, TDestination> onTransforming)
            : this(sourceType, destinationType, beforeMapping, afterMapping)
        {
            if (onTransforming == null)
                throw new MapperParameterException("OnTransforming", "Expression for transforming source instance cannot be null.");

            this.onTransforming = onTransforming;
        }

        /// <summary>
        /// Gets the before mapping action.
        /// </summary>
        /// <value>
        /// The before mapping.
        /// </value>
        protected Action<TDestination> BeforeMapping
        {
            get { return this.beforeMapping; }
        }

        /// <summary>
        /// Gets the after mapping action.
        /// </summary>
        /// <value>
        /// The after mapping.
        /// </value>
        protected Action<TDestination> AfterMapping
        {
            get { return this.afterMapping; }
        }

        /// <summary>
        /// Gets or sets the on transforming.
        /// </summary>
        /// <value>
        /// The on transforming.
        /// </value>
        protected Action<TSource, TDestination> OnTransforming
        {
            get { return this.onTransforming; }
            set { this.onTransforming = value; }
        }

        /// <summary>
        /// Transforms the specified source.
        /// </summary>
        /// <param name="source">The source instance.</param>
        /// <param name="destination">The destination instance.</param>
        /// <returns></returns>
        /// <exception cref="MappingFailedActionException">
        /// Error on executing BeforeMapping action.
        /// or
        /// Error on executing AfterMapping action.
        /// </exception>
        /// <exception cref="MapperParameterException">OnTransforming;Expression for transforming source instance cannot be null.</exception>
        /// <exception cref="MapperException"></exception>
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
                    throw new MapperException(
                        string.Format(
                            "Exception on transforming source instance (type of <{0}>) into destination type (of <{1}>).",
                            this.SourceType.Name, this.DestinationType.Name), ex);
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
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj is ActionTransformer<TSource, TDestination>)
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
            return typeof(ActionTransformer<TSource, TDestination>).GetHashCode() + base.GetHashCode();
        }

    }
}
