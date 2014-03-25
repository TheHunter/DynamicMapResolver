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
    public class TransformerObserverTest
    {
        
        [Test]
        [Description("Mapping with the same source type saved into resolver.")]
        public void TestMapper()
        {
            TransformerObserver observer = new TransformerObserver();
            ISourceMapper<Student, Person> mapper1 = FactoryMapper.DynamicResolutionMapper<Student, Person>();

            observer.RegisterMapper(mapper1);
            Student st = new Student { Name = "mario", Surname = "monti", AnnoNascita = 19 };

            var res = observer.TryToMap<Student, Person>(st);
            Assert.IsNotNull(res);

        }

        [Test]
        [Description("Mapping with the same source type saved into resolver.")]
        public void TestTransformerObserver()
        {
            TransformerObserver observer = new TransformerObserver();
            var builder = observer.MakeTransformerBuilder<User, UserDto>(BuilderType.DefaultMappers);
            builder.Include(
                resolver => new PropertyMapper<User, UserDto>((user, dto) => dto.Parent = resolver.TryToMap<User, UserDto>(user.Parent)));

            builder.BuildMapper();

            User user1 = new User
                {
                    Name = "name1",
                    Surname = "surname1",
                    Parent = new User
                        {
                            Name = "parteName1",
                            Surname = "parentSurname1",
                            Parent = new User
                                {
                                    Name = "parentParentName1",
                                    Surname = "parentParentSurname1",
                                    Parent = new User()
                                }
                        }
                };

            var resDto = observer.TryToMap<User, UserDto>(user1);
            Assert.IsNotNull(resDto);
        }

        [Test]
        [Description("Mapping with the same source type saved into resolver.")]
        public void TestConcreteMapper()
        {
            TransformerObserver observer = new TransformerObserver();
            IList<IPropertyMapper<Student, Person>> propMappers = new List<IPropertyMapper<Student, Person>>
                {
                    new PropertyMapper<Student, Person>( (student, person) => person.Name = student.Name, "Name", "Name")
                    ,new PropertyMapper<Student, Person>( (student, person) => person.AnnoNascita = student.AnnoNascita )
                    ,new PropertyMapper<Student, Person>( (student, person) => person.Parent = student.Father )
                };
            SourceMapper<Student, Person> mapper1 = new SourceMapper<Student, Person>(propMappers, null, null);

            observer.RegisterMapper(mapper1);
            Student st = new Student { Name = "mario", Surname = "monti", AnnoNascita = 19 };
            StudentDetails st2 = new StudentDetails { Name = "mario", Surname = "monti", AnnoNascita = 19, CF = "CF"};

            var res = observer.TryToMap<Student, Person>(st);
            var res2 = observer.TryToMap<Student, Person>(st2);
            Assert.IsNotNull(res);
            Assert.IsNotNull(res2);
        }

        [Test]
        public void TestVerifyRegistration()
        {
            TransformerObserver observer = new TransformerObserver();
            IList<IPropertyMapper<Student, Person>> propMappers = new List<IPropertyMapper<Student, Person>>
                {
                    new PropertyMapper<Student, Person>( (student, person) => person.Name = student.Name, "Name", "Name")
                    ,new PropertyMapper<Student, Person>( (student, person) => person.AnnoNascita = student.AnnoNascita )
                    ,new PropertyMapper<Student, Person>( (student, person) => person.Parent = student.Father )
                };
            SourceMapper<Student, Person> mapper1 = new SourceMapper<Student, Person>(propMappers, null, null);

            observer.RegisterMapper(mapper1);

            
        }

        
    }
}
