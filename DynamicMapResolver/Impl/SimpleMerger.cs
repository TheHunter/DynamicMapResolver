using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DynamicMapResolver.Exceptions;

namespace DynamicMapResolver.Impl
{
    
    //public class SimpleMerger<TSource, TDestination>
    //    : ActionTransformer<TSource, TDestination>, ISimpleMerger<TSource, TDestination>
    //{
    //    private readonly Action<TSource, TDestination> converter;

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <param name="converter"></param>
    //    /// <param name="beforeMapping"></param>
    //    /// <param name="afterMapping"></param>
    //    public SimpleMerger(Action<TSource, TDestination> converter, Action<TDestination> beforeMapping, Action<TDestination> afterMapping)
    //        : base(typeof(TSource), typeof(TDestination), beforeMapping, afterMapping, converter)
    //    {
    //        if (converter == null)
    //            throw new MapperParameterException("converter", "The given lambda converter cannot be null.");

    //        this.converter = converter;
    //    }

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <param name="source"></param>
    //    /// <param name="destination"></param>
    //    /// <returns></returns>
    //    public TDestination Merge(TSource source, TDestination destination)
    //    {
    //        this.Transform(source, destination);
    //        return destination;
    //    }

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <param name="source"></param>
    //    /// <param name="destination"></param>
    //    /// <returns></returns>
    //    object ISourceMerger.Merge(object source, object destination)
    //    {
    //        if (!(source is TSource))
    //            throw new MapperParameterException("source", string.Format("Error before executing the Map method from ISimpleTransformer instance, caused by incorrect source type (of <{0}>), instead It's expected <{1}>", source.GetType().Name, typeof(TSource).Name));

    //        if (!(destination is TDestination))
    //            throw new MapperParameterException("destination", string.Format("Error before executing the Map method from ISimpleTransformer instance, caused by incorrect destination type (of <{0}>), instead It's expected <{1}>", source.GetType().Name, typeof(TDestination).Name));
            
    //        return this.Merge((TSource)source, (TDestination)destination);
    //    }

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <param name="obj"></param>
    //    /// <returns></returns>
    //    public override bool Equals(object obj)
    //    {
    //        if (obj == null)
    //            return false;

    //        if (obj is SimpleMerger<TSource, TDestination>)
    //            return this.GetHashCode() == obj.GetHashCode();

    //        return false;
    //    }

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <returns></returns>
    //    public override int GetHashCode()
    //    {
    //        return typeof(SimpleMerger<TSource, TDestination>).GetHashCode() + base.GetHashCode();
    //    }
    //}
}
