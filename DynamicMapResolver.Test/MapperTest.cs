using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DynamicMapResolver.Exceptions;
using DynamicMapResolver.Impl;
using DynamicMapResolver.Test.Domain;
using NUnit.Framework;

namespace DynamicMapResolver.Test
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

            Student st = new Student { Name = "mario", Surname = "monti", AnnoNascita = 1 };
            Person pr = new Person();

            merger.Merge(st, pr);

            Assert.AreEqual(st.Name, pr.Name);
            Assert.AreEqual(st.Surname, pr.Surname);
            Assert.AreEqual(st.AnnoNascita, pr.AnnoNascita);
        }

        [Test]
        public void TestCustomMapper()
        {
            IList<IPropertyMapper<Student, Person>> propMappers = new List<IPropertyMapper<Student, Person>>
                {
                    new PropertyMapper<Student, Person>( (student, person) => person.Name = student.Name.ToUpper(), "Name", "Name")
                    , new PropertyMapper<Student, Person>( (student, person) => person.AnnoNascita = student.AnnoNascita )
                };

            ISourceMapper<Student, Person> mapper = new SourceMapper<Student, Person>(propMappers, null, null);

            Student st = new Student { Name = "mario", Surname = "monti", AnnoNascita = 19 };
            Person pr = mapper.Map(st);

            Assert.AreEqual(st.Name.ToUpper(), pr.Name, "Property [Name] was not set.");
            Assert.AreNotEqual(st.Surname.ToUpper(), pr.Surname);
            Assert.AreEqual(st.AnnoNascita, pr.AnnoNascita);
        }

        [Test]
        public void TestCustomMerger()
        {
            IList<IPropertyMapper<Student, Person>> propMappers = new List<IPropertyMapper<Student, Person>>
                {
                    new PropertyMapper<Student, Person>( (student, person) => person.Name = student.Name.ToUpper(), "Name", "Name")
                    , new PropertyMapper<Student, Person>( (student, person) => person.AnnoNascita = student.AnnoNascita )
                };

            ISourceMerger<Student, Person> mapper = new SourceMerger<Student, Person>(propMappers, null, null);

            Student st = new Student { Name = "mario", Surname = "monti", AnnoNascita = 19 };
            Person pr = mapper.Merge(st, new Person());

            Assert.AreEqual(st.Name.ToUpper(), pr.Name, "Property [Name] was not set.");
            Assert.AreNotEqual(st.Surname.ToUpper(), pr.Surname);
            Assert.AreEqual(st.AnnoNascita, pr.AnnoNascita);
        }

        [Test]
        public void TestDefaultMergerFilter()
        {
            var merger = FactoryMapper.DynamicResolutionMerger<Student, Person>();

            Assert.IsTrue(merger.PropertyMappers.Count() == 3);

            Student st = new Student { Name = "mario", Surname = "monti", AnnoNascita = 1990 };
            Person pr = new Person();

            merger.Merge(st, pr, new List<string>{ "Name", "Surname" });

            Assert.AreEqual(st.Name, pr.Name);
            Assert.AreEqual(st.Surname, pr.Surname);
            Assert.AreNotEqual(st.AnnoNascita, pr.AnnoNascita);
        }

        [Test]
        public void TestReferenceProperty()
        {
            IList<IPropertyMapper<Student, Person>> propMappers = new List<IPropertyMapper<Student, Person>>
                {
                    new PropertyMapper<Student, Person>( (student, person) => person.Name = student.Name, "Name", "Name")
                    ,new PropertyMapper<Student, Person>( (student, person) => person.AnnoNascita = student.AnnoNascita )
                    ,new PropertyMapper<Student, Person>( (student, person) => person.Parent = student.Father )
                };

            ISourceMapper<Student, Person> mapper = new SourceMapper<Student, Person>(propMappers, null, null);

            Student st = new Student { Name = "mario", Surname = "monti", AnnoNascita = 1990, Father = new PersonaGiuridica { Name = "father", Surname = "father_surname", AnnoNascita = 1970, Code = "nick"} };
            Person pr = mapper.Map(st);

            Assert.AreEqual(st.Name, pr.Name);
            Assert.AreNotEqual(st.Surname, pr.Surname);
            Assert.AreEqual(st.AnnoNascita, pr.AnnoNascita);
            Assert.AreEqual(st.Father, pr.Parent);
            Assert.AreSame(st.Father, pr.Parent);
            Assert.AreEqual(st.Father.GetType(), pr.Parent.GetType());
        }

        [Test]
        public void TestUserTransformer()
        {
            CustomTransformer transformer = new CustomTransformer();
            Student st = new Student { Name = "mario", Surname = "monti", AnnoNascita = 1980 };
            Person pr = new Person();

            transformer.Transform(st, pr);

            Assert.AreEqual(st.Name, pr.Name);
            Assert.AreEqual(st.Surname, pr.Surname);
            Assert.AreEqual(st.AnnoNascita, pr.AnnoNascita);
        }

        [Test]
        [ExpectedException(typeof(MapperParameterException))]
        public void TestFailedInstanceMapper()
        {
            new SourceMapper<Student, Person>(null, null, null);
        }

        [Test]
        [ExpectedException(typeof(MapperParameterException))]
        public void TestFailedInstanceMerger()
        {
            new SourceMerger<Student, Person>(null, null, null);
        }

        [Test]
        public void TestDefaultMapper2()
        {
            StringBuilder buffer = new StringBuilder();

            var mapper = FactoryMapper.DynamicResolutionMapper<Student, Person>
                (
                    n => buffer.AppendLine(string.Format("ToString before mapping: {0}", n.ToString())),
                    r => buffer.AppendLine(string.Format("ToString after mapping: {0}", r.ToString()))
                );

            Student st = new Student { Name = "mario", Surname = "monti", AnnoNascita = 19 };
            Person pr = mapper.Map(st);

            Assert.AreEqual(st.Name, pr.Name);
            Assert.AreEqual(st.Surname, pr.Surname);
            Assert.AreEqual(st.AnnoNascita, pr.AnnoNascita);
        }

        [Test]
        [ExpectedException(typeof(MappingFailedActionException))]
        public void FailedTestDefaultMapper()
        {
            StringBuilder buffer = new StringBuilder();

            var mapper = FactoryMapper.DynamicResolutionMapper<Student, Person>
                (
                    n => buffer.AppendLine(string.Format("ToString before mapping: {0}", n.ToString())),
                    r => { throw new Exception("error"); }
                );

            Student st = new Student { Name = "mario", Surname = "monti", AnnoNascita = 19 };
            mapper.Map(st);
        }

        [Test]
        public void TestDefaultMerger2()
        {
            StringBuilder buffer = new StringBuilder();

            var merger = FactoryMapper.DynamicResolutionMerger<Student, Person>
                (
                    n => buffer.AppendLine(string.Format("ToString before mapping: {0}", n.ToString())),
                    r => buffer.AppendLine(string.Format("ToString after mapping: {0}", r.ToString()))
                );

            Student st = new Student { Name = "mario", Surname = "monti", AnnoNascita = 1 };
            Person pr = new Person();

            merger.Merge(st, pr);

            Assert.AreEqual(st.Name, pr.Name);
            Assert.AreEqual(st.Surname, pr.Surname);
            Assert.AreEqual(st.AnnoNascita, pr.AnnoNascita);
        }

        [Test]
        [ExpectedException(typeof(MappingFailedActionException))]
        public void FailedTestDefaultMerger()
        {
            StringBuilder buffer = new StringBuilder();

            var merger = FactoryMapper.DynamicResolutionMerger<Student, Person>
                (
                    r => { throw new Exception("error"); },
                    r => buffer.AppendLine(string.Format("ToString after mapping: {0}", r.ToString()))
                );

            Student st = new Student { Name = "mario", Surname = "monti", AnnoNascita = 1 };
            Person pr = new Person();

            merger.Merge(st, pr);
        }

        [Test]
        public void TestMapperNonPublicMembers()
        {
            ISourceMapper<PersonaGiuridica, PersonDetails> mapper = FactoryMapper.DynamicResolutionMapper<PersonaGiuridica, PersonDetails>();
            PersonaGiuridica person = new PersonaGiuridica
                {
                    Code = "150",
                    Name = "Sergio",
                    Surname = "Hill",
                    AnnoNascita = 1980,
                    Parent = new Person {Name = "fatherName", Surname = "fatherSurname", AnnoNascita = 1950}
                };

            var result = mapper.Map(person);
            Assert.IsNotNull(result);
        }

        [Test]
        public void TestMapperMembersFromInputTypes()
        {
            ISourceMapper mapper = FactoryMapper.DynamicResolutionMapper(typeof(PersonaGiuridica), typeof(PersonDetails));
            PersonaGiuridica person = new PersonaGiuridica
                {
                    Code = "150",
                    Name = "Sergio",
                    Surname = "Hill",
                    AnnoNascita = 1980,
                    Parent = new Person { Name = "fatherName", Surname = "fatherSurname", AnnoNascita = 1950 }
                };

            var result = mapper.Map(person);
            Assert.IsNotNull(result);
        }

        [Test]
        public void TestNonGenericMapper1()
        {
            ISourceMapper mapper = FactoryMapper.DynamicResolutionMapper(typeof(PersonaGiuridica), typeof(PersonDetails));
            PersonaGiuridica person = new PersonaGiuridica
                {
                    Code = "150",
                    Name = "Sergio",
                    Surname = "Hill",
                    AnnoNascita = 1980,
                    Parent = new Person { Name = "fatherName", Surname = "fatherSurname", AnnoNascita = 1950 }
                };

            var result = mapper.Map(person);
            Assert.IsNotNull(result);
        }

        [Test]
        [ExpectedException(typeof(MapperParameterException))]
        public void FailedTestDestinationInterfaceType1()
        {
            FactoryMapper.DynamicResolutionMapper(typeof(PersonaGiuridica), typeof(IPersonHeader));
        }

        [Test]
        [ExpectedException(typeof(MapperParameterException))]
        public void FailedTestDestinationInterfaceType2()
        {
            FactoryMapper.DynamicResolutionMapper<PersonaGiuridica, IPersonHeader>();
        }

        [Test]
        public void TestInterfaceToClass1()
        {
            var mapper = FactoryMapper.DynamicResolutionMapper<IPersonHeader, PersonDetails>();
            Assert.IsNotNull(mapper);

            IPersonHeader ps = new Person{ Name = "name", Surname = "surname", AnnoNascita = 1980, Parent = null };
            PersonDetails res = mapper.Map(ps);
            Assert.IsNotNull(res);
        }

        [Test]
        public void TestInterfaceToClass2()
        {
            var mapper = FactoryMapper.DynamicResolutionMapper(typeof(IPersonHeader), typeof(PersonDetails));
            Assert.IsNotNull(mapper);

            IPersonHeader ps = new Person { Name = "name", Surname = "surname", AnnoNascita = 1980, Parent = null };
            object res = mapper.Map(ps);
            Assert.IsNotNull(res);
        }
    }
    
}
