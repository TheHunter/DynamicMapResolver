using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using AutoMapper;
using NUnit.Framework;

namespace DynamicMapper.Test
{
    [TestFixture]
    public class MapperTest
    {
        [Test]
        public void Test1()
        {
            Type t = typeof (Person);
            var properties = t.GetProperties();




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
