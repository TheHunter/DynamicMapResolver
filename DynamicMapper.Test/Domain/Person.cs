using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynamicMapper.Test.Domain
{
    /// <summary>
    /// 
    /// </summary>
    public class Person
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public double? AnnoNascita { get; set; }
        public Person Parent { get; set; }
    }
}
