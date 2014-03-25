using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynamicMapResolver
{
    /// <summary>
    /// 
    /// </summary>
    public interface ITransformerRegister
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TMapper"></typeparam>
        /// <param name="mapper"></param>
        /// <returns></returns>
        bool RegisterMapper<TMapper>(TMapper mapper) where TMapper : class, ISourceMapper;

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TMapper"></typeparam>
        /// <param name="mapper"></param>
        /// <param name="keyService"></param>
        /// <returns></returns>
        bool RegisterMapper<TMapper>(TMapper mapper, string keyService) where TMapper : class, ISourceMapper;

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TMapper"></typeparam>
        /// <param name="mapper"></param>
        /// <param name="keyService"></param>
        /// <returns></returns>
        bool RegisterMapper<TMapper>(TMapper mapper, object keyService) where TMapper : class, ISourceMapper;

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TMerger"></typeparam>
        /// <param name="merger"></param>
        /// <returns></returns>
        bool RegisterMerger<TMerger>(TMerger merger) where TMerger : class, ISourceMerger;

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TMerger"></typeparam>
        /// <param name="merger"></param>
        /// <param name="keyService"></param>
        /// <returns></returns>
        bool RegisterMerger<TMerger>(TMerger merger, string keyService) where TMerger : class, ISourceMerger;

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TMerger"></typeparam>
        /// <param name="merger"></param>
        /// <param name="keyService"></param>
        /// <returns></returns>
        bool RegisterMerger<TMerger>(TMerger merger, object keyService) where TMerger : class, ISourceMerger;
    }
}
