using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;

namespace DynamicMapper.Test
{
    public class MapperTest
    {
        public void Test1()
        {
            
        }
    }

    public class Person
    {
        public string Name { get; set; }
        public string Surname { get; set; }
    }

    public class Student
    {
        public string Nome { get; set; }
        public string Cognome { get; set; }
    }
}
