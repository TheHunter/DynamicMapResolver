using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using DynamicMapper.Test.Domain;
using NUnit.Framework;

namespace DynamicMapper.Test
{
    [TestFixture]
    public class AutoMapperTest
    {
        [Test]
        public void TestAutoMap()
        {
            Mapper.CreateMap<Student, Person>();

            Student st = new Student { Name = "mario", Surname = "monti", AnnoNascita = 19 };
            Person pr = Mapper.Map<Student, Person>(st);

            Assert.AreEqual(st.Name, pr.Name);
            Assert.AreEqual(st.Surname, pr.Surname);
            Assert.AreEqual(st.AnnoNascita, pr.AnnoNascita);
        }

    }
}
