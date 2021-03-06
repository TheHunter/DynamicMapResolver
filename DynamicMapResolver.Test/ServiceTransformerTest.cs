﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DynamicMapResolver.Impl;
using DynamicMapResolver.Test.Pocos;
using NUnit.Framework;

namespace DynamicMapResolver.Test
{
    [TestFixture]
    public class ServiceTransformerTest
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
            var res5 = srv1.Match("default", example.GetType(), typeof(Person));

            Assert.IsTrue(res1);
            Assert.IsFalse(res2);
            Assert.IsFalse(res3);
            Assert.IsFalse(res4);
            Assert.IsTrue(res5);

        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void NewServiceTransformer()
        {
            new ServiceTransformer<ISourceMapper<Student, Person>>("null", null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void NewServiceTransformer1()
        {
            IList<IPropertyMapper<Student, Person>> propMappers = new List<IPropertyMapper<Student, Person>>
                {
                    new PropertyMapper<Student, Person>( (student, person) => person.Name = student.Name, "Name", "Name")
                    ,new PropertyMapper<Student, Person>( (student, person) => person.AnnoNascita = student.AnnoNascita )
                    ,new PropertyMapper<Student, Person>( (student, person) => person.Parent = student.Father )
                };

            SourceMapper<Student, Person> mapper = new SourceMapper<Student, Person>(propMappers, null, null);
            new ServiceTransformer<ISourceMapper<Student, Person>>(null, mapper);
        }

        [Test]
        public void ServiceTransformerMembersTest()
        {
            object defaultKey = " ciao ";

            ISourceMerger mapper =
                new SourceMerger<Student, Person>(
                    FactoryMapper.GetDefaultPropertyMappers<Student, Person>(), null, null);
            
            var service = new ServiceTransformer<ISourceMerger>(defaultKey, mapper);

            Assert.IsNotNull(service);
            Assert.IsTrue(service.ServiceKey is string);
            Assert.AreNotEqual(service.ServiceKey, " ciao");
            Assert.AreNotEqual(service.ServiceKey, "ciao ");
            Assert.AreEqual(service.ServiceKey, "ciao");
        }

        [Test]
        public void ServiceTransformerMembersTest1()
        {
            object defaultKey = KeyService.Type1;

            ISourceMerger mapper =
                new SourceMerger<Student, Person>(
                    FactoryMapper.GetDefaultPropertyMappers<Student, Person>(), null, null);

            var service = new ServiceTransformer<ISourceMerger>(defaultKey, mapper);

            Assert.IsNotNull(service);
            Assert.IsTrue(service.ServiceKey is Enum);
            Assert.AreEqual(service.ServiceKey, KeyService.Type1);
        }
    }
}
