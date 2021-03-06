﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper.Internal;
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
        public void TestOnComplexDerivedClasses()
        {
            var mapper = FactoryMapper.DynamicResolutionMapper<MyDerivedClass, DerivedClassLast>();
            Assert.NotNull(mapper);
            Assert.AreEqual(4, mapper.PropertyMappers.Count());

            MyDerivedClass source = new MyDerivedClass(1, "mycomment", "mycomment2", 12.5);
            var res = mapper.Map(source);

            Assert.NotNull(res);
            Assert.AreEqual(source.Avarage, res.Avarage);
            Assert.AreEqual(source.Comment, res.Comment);
            Assert.AreEqual(source.CommentTwo, res.CommentTwo);
            Assert.AreEqual(source.Counter, res.Counter);
        }

        [Test]
        [Description("Mapping with the same source type saved into resolver.")]
        public void MakeTransformerBuilderTest1()
        {
            TransformerObserver observer = new TransformerObserver();
            var builder = observer.MakeTransformerBuilder<User, UserDto>(BuilderType.DefaultMappers);
            //builder.Include(
            //    resolver => new PropertyMapper<User, UserDto>((user, dto) => dto.Parent = resolver.TryToMap<User, UserDto>(user.Parent)));

            builder.Include(
                resolver => new PropertyMapper<User, UserDto>((user, dto) => dto.Parent = (UserDto)resolver.TryToMap(user.Parent, typeof(UserDto))));


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
            Assert.IsNotNull(resDto.Parent);

            var resDto1 = observer.TryToMap(user1, typeof (UserDto));
            Assert.IsNotNull(resDto1);
            Assert.IsTrue(resDto1 is UserDto);
            Assert.IsNotNull(resDto1.GetType().GetProperty("Parent").GetValue(resDto1, null));

            var resDto2 = observer.TryToMap<User, UserDto>(user1, "mykey");
            Assert.IsNull(resDto2);

            var resDto3 = observer.TryToMap(user1, typeof(UserDto), "mykey");
            Assert.IsNull(resDto3);
        }

        [Test]
        [Description("")]
        public void MakeTransformerBuilderTest2()
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
        [Description("")]
        public void MakeTransformerBuilderTest3()
        {
            TransformerObserver observer = new TransformerObserver();
            var mapper = observer.MakeTransformerBuilder<Person, Student>(BuilderType.DynamicResolver);
            /*
             * here It's possible to add / delete property mappers.
            */
            // before using the mapper object, It's needed to build it in order to associate into TransformerObserver
            mapper.BuildMapper();

            Assert.IsNotNull(mapper);

            Person ps = new Person
                {
                    Name = "name",
                    Surname = "surname",
                    AnnoNascita = 1980,
                    Parent = new Person
                        {
                            Name = "parent_name",
                            Surname = "parent_surname"
                        }
                };

            var resDto = observer.TryToMap<Person, Student>(ps);
            Assert.IsNotNull(resDto);
            Assert.IsNull(resDto.Father);

            var resDto1 = observer.TryToMap(ps, typeof(Student));
            Assert.IsNotNull(resDto1);
            Assert.IsNull(resDto1.GetType().GetProperty("Father").GetValue(resDto1, null));
            Assert.IsTrue(resDto1 is Student);

            var resDto2 = observer.TryToMap<Person, Student>(ps, "mykey");
            Assert.IsNull(resDto2);

            var resDto3 = observer.TryToMap(ps, typeof(Student), "mykey");
            Assert.IsNull(resDto3);
        }

        [Test]
        [Description("")]
        public void MakeTransformerBuilderTest4()
        {
            TransformerObserver observer = new TransformerObserver();
            var mapper = observer.MakeTransformerBuilder<Person, Student>(BuilderType.DynamicResolver);
            mapper.Include(new PropertyMapper<Person, Student>((person, student) => student.Father = person.Parent, "Father", "Parent"));
            mapper.BuildMapper();

            Assert.IsNotNull(mapper);

            Person ps = new Person
            {
                Name = "name",
                Surname = "surname",
                AnnoNascita = 1980,
                Parent = new Person
                {
                    Name = "parent_name",
                    Surname = "parent_surname"
                }
            };

            var resDto = observer.TryToMap<Person, Student>(ps);
            Assert.IsNotNull(resDto);
            Assert.IsNotNull(resDto.Father);

            var resDto1 = observer.TryToMap(ps, typeof(Student));
            Assert.IsNotNull(resDto1);
            Assert.IsNotNull(resDto1.GetType().GetProperty("Father").GetValue(resDto1, null));
            Assert.IsTrue(resDto1 is Student);

            var resDto2 = observer.TryToMap<Person, Student>(ps, "mykey");
            Assert.IsNull(resDto2);

            var resDto3 = observer.TryToMap(ps, typeof(Student), "mykey");
            var resDto4 = observer.TryToMap(ps, null, "mykey");

            Assert.IsNull(resDto3);
            Assert.IsNull(resDto4);
        }

        [Test]
        [Description("Mapping with the same source type saved into resolver.")]
        public void RegisterMapperTest1()
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
        public void RegisterMapperTest2()
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
        public void RegisterMapperTest3()
        {
            TransformerObserver observer = new TransformerObserver();
            IList<IPropertyMapper<Student, Person>> propMappers = new List<IPropertyMapper<Student, Person>>
                {
                    new PropertyMapper<Student, Person>( (student, person) => person.Name = student.Name, "Name", "Name")
                    ,new PropertyMapper<Student, Person>( (student, person) => person.AnnoNascita = student.AnnoNascita )
                    ,new PropertyMapper<Student, Person>( (student, person) => person.Parent = student.Father )
                };

            ISourceMapper mapper1 = new SourceMapper<Student, Person>(propMappers, null, null);
            ISourceMapper<Student, Person> mapper2 = new SourceMapper<Student, Person>(propMappers, null, null);

            Assert.IsTrue(observer.BuildAutoResolverMapper<User, UserDto>(null, null));     //register a mapper which transfomr User into UserDto (1)
            Assert.IsTrue(observer.BuildAutoResolverMapper<Student, Person>(null, null));   //register a mapper which transfomr Student into Person (1)

            Assert.IsFalse(observer.RegisterMapper(mapper1));           // this mapper cannot be registered 'cause It was registered another similar mapper ( as (1) )
            Assert.IsFalse(observer.RegisterMapper(mapper2));           // this mapper cannot be registered 'cause It was registered another similar mapper ( as (1) )

            Assert.IsFalse(observer.BuildAutoResolverMapper<User, UserDto>(null, null));    //It's equals to mapper (1), so It cannot be registered.
            Assert.IsFalse(observer.BuildAutoResolverMapper<User, UserDto>(null, Console.WriteLine));   //It's similar to mapper (2), so It cannot be registered.

            Assert.IsTrue(observer.RegisterMapper(mapper1, KeyService.Type1));           // this mapper can be registered 'cause It was registered with another KeyService nevertheless using a similar mapper ( as (1) )
            Assert.IsFalse(observer.RegisterMapper(mapper2, KeyService.Type1));           // this mapper cannot be registered 'cause It was registered another similar mapper ( as (1) ), using the same serviceKey like a previous registration.

        }

        [Test]
        public void RegisterSimpleMapperTest1()
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

        [Test]
        public void BuildAutoResolverMapperTest1()
        {
            TransformerObserver observer = new TransformerObserver();
            Assert.IsTrue(observer.BuildAutoResolverMapper<User, UserDto>(null, null));

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

            var res = observer.TryToMap<User, UserDto>(user1);
            Assert.IsNotNull(res);

            var res1 = observer.TryToMap(user1, typeof(UserDto));
            Assert.IsNotNull(res1);
        }

        [Test]
        public void BuildAutoResolverMapperTest2()
        {
            TransformerObserver observer = new TransformerObserver();
            Assert.IsTrue(observer.BuildAutoResolverMapper(typeof(User), typeof(UserDto)));

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

            var res = observer.TryToMap<User, UserDto>(user1);
            Assert.IsNotNull(res);

            var res1 = observer.TryToMap(user1, typeof(UserDto));
            Assert.IsNotNull(res1);
        }

        [Test]
        public void BuildAutoResolverMapperTest3()
        {
            TransformerObserver observer = new TransformerObserver();
            Assert.IsTrue(observer.BuildAutoResolverMapper(typeof(CustomComplexType), typeof(CustomComplexTypeDto)));
            Assert.IsTrue(observer.BuildAutoResolverMapper<User, UserDto>(null, null));
            Assert.IsTrue(observer.RegisterMapper(new SimpleMapper<KeyService, KeyServiceOther>(service => (KeyServiceOther)Enum.ToObject(typeof(KeyServiceOther), service))));

            CustomComplexType instance = new CustomComplexType
                {
                    MyKeyService = KeyService.Type2,
                    ComplexNaming = "complex_naming",
                    Owner = new User
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
                    }
                };

            var res = observer.TryToMap<CustomComplexType, CustomComplexTypeDto>(instance);
            Assert.IsNotNull(res);
            Assert.AreEqual(res.MyKeyService, KeyServiceOther.Type2);
            Assert.AreEqual(res.ComplexNaming, instance.ComplexNaming);
            Assert.IsNotNull(res.Owner);

            var res1 = observer.TryToMap(instance, typeof(CustomComplexTypeDto)) as CustomComplexTypeDto; 
            Assert.IsNotNull(res1);
            Assert.AreEqual(res1.MyKeyService, KeyServiceOther.Type2);
            Assert.AreEqual(res1.ComplexNaming, instance.ComplexNaming);
            Assert.IsNotNull(res1.Owner);

            var res2 = observer.TryToMap(KeyService.Type1, null);
            Assert.IsNull(res2);
        }

        /*
         TODO
         * try to convert example <int?, int>... by SimpleMapper..
         */

        [Test]
        public void BuildAutoResolverMapperTest4()
        {
            TransformerObserver observer = new TransformerObserver();
            Assert.IsTrue(observer.BuildAutoResolverMapper(typeof(CustomSimpleTypeDto), typeof(CustomSimpleType)));

            CustomSimpleTypeDto instance = new CustomSimpleTypeDto
            {
                Naming = "naming",
                Code = 10
            };

            var res = observer.TryToMap<CustomSimpleTypeDto, CustomSimpleType>(instance);
            Assert.IsNotNull(res);
            Assert.AreEqual(res.Naming, instance.Naming);
            Assert.AreEqual(res.Code, 0);

            var res1 = observer.TryToMap(instance, typeof(CustomSimpleType)) as CustomSimpleType;
            Assert.IsNotNull(res1);
            Assert.AreEqual(res1.Naming, instance.Naming);
            Assert.AreEqual(res1.Code, 0);
        }

        [Test]
        public void BuildAutoResolverMapperTest5()
        {
            TransformerObserver observer = new TransformerObserver();
            Assert.IsTrue(observer.BuildAutoResolverMapper(typeof(CustomSimpleTypeDto), typeof(CustomSimpleType)));
            Assert.IsTrue(observer.RegisterMapper(new SimpleMapper<int?, int>(i => i.GetValueOrDefault() )));

            CustomSimpleTypeDto instance = new CustomSimpleTypeDto
            {
                Naming = "naming",
                Code = 10
            };

            var res = observer.TryToMap<CustomSimpleTypeDto, CustomSimpleType>(instance);
            Assert.IsNotNull(res);
            Assert.AreEqual(res.Naming, instance.Naming);
            Assert.AreEqual(res.Code, instance.Code);

            var res1 = observer.TryToMap(instance, typeof(CustomSimpleType)) as CustomSimpleType;
            Assert.IsNotNull(res1);
            Assert.AreEqual(res1.Naming, instance.Naming);
            Assert.AreEqual(res1.Code, instance.Code);
        }

        [Test]
        public void RetreiveMapperFromObserver()
        {
            TransformerObserver observer = new TransformerObserver();
            var builder = observer.MakeTransformerBuilder<IPersonHeader, PersonDetails>(BuilderType.DefaultMappers);
            var mapper1 = builder.BuildMapper();
            var mapper2 = builder.BuildMapper("keymapper");

            builder.BuildMerger();
            builder.BuildMerger("merger");
            
            Assert.IsNotNull(builder);
            Assert.AreEqual(mapper1, mapper2);
            Assert.AreNotSame(mapper1, mapper2);
            
            //IPersonHeader ps = new Person { Name = "name", Surname = "surname", AnnoNascita = 1980, Parent = null };

            Assert.NotNull(observer.RetrieveMapper<ISourceMapper<IPersonHeader, PersonDetails>>());
            Assert.Null(observer.RetrieveMapper<ISourceMapper<IPersonHeader, PersonDetails>>("mykey"));
            Assert.NotNull(observer.RetrieveMapper<ISourceMapper<IPersonHeader, PersonDetails>>("keymapper"));
            Assert.Null(observer.RetrieveMapper<ISourceMapper<Person, PersonDetails>>());


            Assert.NotNull(observer.RetrieveMerger<ISourceMerger<IPersonHeader, PersonDetails>>());
            Assert.NotNull(observer.RetrieveMerger<ISourceMerger<IPersonHeader, PersonDetails>>("merger"));
            Assert.Null(observer.RetrieveMerger<ISourceMerger<IPersonHeader, PersonDetails>>("merger1"));
            
        }
    }
}
