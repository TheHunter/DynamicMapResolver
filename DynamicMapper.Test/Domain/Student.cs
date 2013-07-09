using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynamicMapper.Test.Domain
{
    /// <summary>
    /// 
    /// </summary>
    public class Student
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public int AnnoNascita { get; set; }
        public Person Father { get; set; }
    }
}
