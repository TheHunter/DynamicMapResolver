using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynamicMapResolver.Test.Domain
{
    public interface IPersonHeader
    {
        string Name { get; set; }
        string Surname { get; set; }
        double? AnnoNascita { get; set; }
    }
}
