using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynamicMapResolver
{
    /// <summary>
    /// 
    /// </summary>
    public interface ITransformerObserver
        : ITransformerResolver, ITransformerRegister
    {
    }
}
