using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using DynamicMapper.Impl;
using DynamicMapper.Test.Domain;
using NUnit.Framework;

namespace DynamicMapper.Test
{
    [TestFixture]
    public class MapperTest
    {
        [Test]
        public void TestDefaultMapper()
        {
            var mapper = FactoryMapper.DynamicResolutionMapper<Student, Person>();

            Student st = new Student{Name = "mario", Surname = "monti", AnnoNascita = 19};
            Person pr = mapper.Map(st);

            Assert.AreEqual(st.Name, pr.Name);
            Assert.AreEqual(st.Surname, pr.Surname);
            Assert.AreEqual(st.AnnoNascita, pr.AnnoNascita);
        }

        [Test]
        public void TestDefaultMerger()
        {
            var merger = FactoryMapper.DynamicResolutionMerger<Student, Person>();

            Assert.IsTrue(merger.PropertyMappers.Count() == 3);

            Student st = new Student { Name = "mario", Surname = "monti", AnnoNascita = null };
            Person pr = new Person();

            merger.Merge(st, pr);

            Assert.AreEqual(st.Name, pr.Name);
            Assert.AreEqual(st.Surname, pr.Surname);
            Assert.AreEqual(st.AnnoNascita, pr.AnnoNascita);
        }

        [Test]
        public void TestCustomMapper()
        {
            IList<IPropertyMapper<Student, Person>> propMappers = new BindingList<IPropertyMapper<Student, Person>>
                {
                    new PropertyMapper<Student, Person>((student, person) => person.Name = student.Name.ToUpper(), "Name", "Name")
                    , new PropertyMapper<Student, Person>( (student, person) => person.Surname = student.Surname.ToUpper() )
                    , new PropertyMapper<Student, Person>( (student, person) => person.AnnoNascita = student.AnnoNascita )
                };

            ISourceMapper<Student, Person> mapper = new SourceMapper<Student, Person>(propMappers, null, null);

            Student st = new Student { Name = "mario", Surname = "monti", AnnoNascita = 19 };
            Person pr = mapper.Map(st);

            Assert.AreEqual(st.Name.ToUpper(), pr.Name, "Property [Name] was not set.");
            Assert.AreEqual(st.Surname.ToUpper(), pr.Surname);
            Assert.AreEqual(st.AnnoNascita, pr.AnnoNascita);
        }
    }
    
}
