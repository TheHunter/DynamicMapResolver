using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DynamicMapResolver.Impl;
using DynamicMapResolver.Test.Domain;
using NUnit.Framework;

namespace DynamicMapResolver.Test
{
    [TestFixture]
    public class ServiceResolverTest
    {
        [Test]
        public void Test()
        {
            ISourceMapper<Student, Person> mapper1 = FactoryMapper.DynamicResolutionMapper<Student, Person>();
            ISourceMapper mapper2 = FactoryMapper.DynamicResolutionMapper(typeof(PersonaGiuridica), typeof(PersonDetails));
            ISourceMapper mapper3 = FactoryMapper.DynamicResolutionMapper(typeof(IPersonHeader), typeof(PersonDetails));


            IList<IPropertyMapper<Student, Person>> propMappers = new List<IPropertyMapper<Student, Person>>
                {
                    new PropertyMapper<Student, Person>( (student, person) => person.Name = student.Name, "Name", "Name")
                    ,new PropertyMapper<Student, Person>( (student, person) => person.AnnoNascita = student.AnnoNascita )
                    ,new PropertyMapper<Student, Person>( (student, person) => person.Parent = student.Father )
                };

            SourceMapper<Student, Person> mapper4 = new SourceMapper<Student, Person>(propMappers, null, null);
            StudentDetails example = new StudentDetails();
            


            ServiceTransformer<ISourceMapper<Student, Person>> srv1 = new ServiceTransformer<ISourceMapper<Student, Person>>("default", mapper4);

            var res1 = srv1.Match<ISourceMapper<Student, Person>>("default");
            var res2 = srv1.Match<SourceMapper<Student, Person>>("default");

            var res3 = srv1.Match<ISourceMapper<Student, Person>>("ss");
            var res4 = srv1.Match<SourceMapper<Student, Person>>("ss");
            var res5 = srv1.Match("default", example.GetType(), typeof (Person));

            Assert.IsTrue(res1);
            Assert.IsFalse(res2);
            Assert.IsFalse(res3);
            Assert.IsFalse(res4);
            Assert.IsTrue(res5);
        }

        [Test]
        [Description("Mapping with the same source type saved into resolver.")]
        public void TestMapper()
        {
            TransformerResolver resolver = new TransformerResolver();
            ISourceMapper<Student, Person> mapper1 = FactoryMapper.DynamicResolutionMapper<Student, Person>();

            resolver.RegisterMapper(mapper1);
            Student st = new Student { Name = "mario", Surname = "monti", AnnoNascita = 19 };

            var res = resolver.TryToMap<Student, Person>(st);
            Assert.IsNotNull(res);

        }

        [Test]
        [Description("Mapping with the same source type saved into resolver.")]
        public void TestConcreteMapper()
        {
            TransformerResolver resolver = new TransformerResolver();
            IList<IPropertyMapper<Student, Person>> propMappers = new List<IPropertyMapper<Student, Person>>
                {
                    new PropertyMapper<Student, Person>( (student, person) => person.Name = student.Name, "Name", "Name")
                    ,new PropertyMapper<Student, Person>( (student, person) => person.AnnoNascita = student.AnnoNascita )
                    ,new PropertyMapper<Student, Person>( (student, person) => person.Parent = student.Father )
                };
            SourceMapper<Student, Person> mapper1 = new SourceMapper<Student, Person>(propMappers, null, null);

            resolver.RegisterMapper(mapper1);
            Student st = new Student { Name = "mario", Surname = "monti", AnnoNascita = 19 };
            StudentDetails st2 = new StudentDetails { Name = "mario", Surname = "monti", AnnoNascita = 19, CF = "CF"};

            var res = resolver.TryToMap<Student, Person>(st);
            var res2 = resolver.TryToMap<Student, Person>(st2);
            Assert.IsNotNull(res);
            Assert.IsNotNull(res2);
        }

        [Test]
        public void Test3()
        {
            
            Type t1 = typeof (ISimpleMapper<Student, Person>);
            Type t2 = typeof (ISourceMapper<Student, Person>);

            Assert.IsTrue(t1.IsAssignableFrom(t2));
            Assert.IsFalse(t2.IsSubclassOf(t1));
        }
    }
}
