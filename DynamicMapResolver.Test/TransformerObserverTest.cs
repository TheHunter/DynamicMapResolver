using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DynamicMapResolver.Impl;
using DynamicMapResolver.Test.Pocos;
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
            Assert.AreEqual(st.Name, res.Name);
            Assert.AreEqual(st.Surname, res.Surname);
            Assert.IsNull(st.Father);
            Assert.IsNull(res.Parent);
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

            var resDto1 = observer.TryToMap(user1, typeof (UserDto));
            Assert.IsNotNull(resDto1);
            Assert.IsTrue(resDto1 is UserDto);

            var resDto2 = observer.TryToMap<User, UserDto>(user1, "mykey");
            Assert.IsNull(resDto2);

            var resDto3 = observer.TryToMap(user1, typeof(UserDto), "mykey");
            Assert.IsNull(resDto3);
        }

        [Test]
        [Description("Da completare... TryToMap non recupera il mapper.")]
        public void TestTransformerObserver2()
        {
            TransformerObserver observer = new TransformerObserver();
            var mapper = observer.MakeTransformerBuilder<IPersonHeader, PersonDetails>(BuilderType.DefaultMappers);
            mapper.BuildMapper();

            Assert.IsNotNull(mapper);

            IPersonHeader ps = new Person { Name = "name", Surname = "surname", AnnoNascita = 1980, Parent = null };

            var resDto = observer.TryToMap<IPersonHeader, PersonDetails>(ps);
            Assert.IsNotNull(resDto);

            var resDto1 = observer.TryToMap(ps, typeof(PersonDetails));
            Assert.IsNotNull(resDto1);
            Assert.IsTrue(resDto1 is PersonDetails);

            var resDto2 = observer.TryToMap<IPersonHeader, PersonDetails>(ps, "mykey");
            Assert.IsNull(resDto2);

            var resDto3 = observer.TryToMap(ps, typeof(PersonDetails), "mykey");
            Assert.IsNull(resDto3);
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

            Assert.IsTrue(observer.RegisterMapper(mapper1));
            Assert.IsTrue(observer.RegisterMapper(mapper1, KeyService.Type1));
            Assert.IsTrue(observer.RegisterMapper(mapper1, KeyService.Type2));
            Assert.IsFalse(observer.RegisterMapper(mapper1));
            Assert.IsFalse(observer.RegisterMapper(mapper1, "default"));

            Student st = new Student { Name = "mario", Surname = "monti", AnnoNascita = 19 };
            
            var res1 = observer.TryToMap<Student, Person>(st);
            Assert.IsNotNull(res1);

            var res11 = observer.TryToMap(st, typeof(Person));
            Assert.IsTrue(res11 is Person);
            Assert.IsNotNull(res11);


            var res2 = observer.TryToMap(st, typeof(Person), KeyService.Type1);
            Assert.IsNotNull(res2);

            var res22 = observer.TryToMap(st, typeof (Person), KeyService.Type1);
            Assert.IsNotNull(res22);


            var res0 = observer.TryToMap(st, typeof (Person), KeyService.Type3);
            Assert.IsNull(res0);

            var res00 = observer.TryToMap<Student, Person>(st, KeyService.Type3);
            Assert.IsNull(res00);

        }
        

        [Test]
        public void TestVerifySimpleMappers()
        {
            TransformerObserver observer = new TransformerObserver();
            var mapper = new SimpleMapper<int?, int>(i => i.HasValue ? i.Value : 0);

            Assert.IsTrue(observer.RegisterMapper(mapper));
            var res = observer.TryToMap<int?, int>(5);

            Assert.AreEqual(res, 5);

            var mapper1 =
                new SimpleMapper<KeyService, KeyServiceOther>(
                    service => (KeyServiceOther)Enum.ToObject(typeof(KeyServiceOther), service));

            Assert.IsTrue(observer.RegisterMapper(mapper1));
            var res1 = observer.TryToMap<KeyService, KeyServiceOther>(KeyService.Type2);
            Assert.AreEqual(res1, KeyServiceOther.Type2);

            var res2 = observer.TryToMap<KeyService, KeyServiceOther>(KeyService.Type3);
            Assert.AreEqual(res2, KeyServiceOther.Type3);
        }

    }
}
