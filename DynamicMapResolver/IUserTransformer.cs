namespace DynamicMapResolver
{
    /// <summary>
    /// A custom definition for user transformers.
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TDestination"></typeparam>
    public interface IUserTransformer<in TSource, in TDestination>
        where TSource : class
        where TDestination : class
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="destination"></param>
        /// <returns></returns>
        void Transform(TSource source, TDestination destination);
    }
}
