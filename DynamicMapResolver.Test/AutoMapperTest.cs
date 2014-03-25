using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using AutoMapper;
using DynamicMapResolver.Impl;
using DynamicMapResolver.Test.Domain;
using NUnit.Framework;

namespace DynamicMapResolver.Test
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

        [Test]
        public void TestAutoMapper2()
        {
            Mapper.CreateMap<Person, PersonaGiuridica>();

            Person p = new Person
                {
                    AnnoNascita = 1980,
                    Name = "ciccio",
                    Surname = "pasticcio",
                    Parent = new Person { Name = "fatherCiccio", Surname = "surnamefather", AnnoNascita = 1940 }
                };

            PersonaGiuridica giu = Mapper.Map<Person, PersonaGiuridica>(p);

            Assert.IsNotNull(giu);
        }

        //[Test]
        public void VerifyElapsedTime()
        {
            int count = 1000000;
            List<Person> lista = GetPersons(count);

            var timeSpanMapResolver = GetElapsedFromMapResolver(lista);
            var timeSpanAutomapper = GetElapsedTimeFromAutoMapper(lista);

            Assert.IsTrue(timeSpanMapResolver.TotalMilliseconds < timeSpanAutomapper.TotalMilliseconds);
        }


        public TimeSpan GetElapsedTimeFromAutoMapper(List<Person> lista)
        {
            Mapper.CreateMap<Person, PersonaGiuridica>();

            Stopwatch meter = new Stopwatch();

            meter.Start();
            foreach (var person in lista)
            {
                Mapper.Map<Person, PersonaGiuridica>(person);
            }
            meter.Stop();
            return meter.Elapsed;
        }


        public TimeSpan GetElapsedFromMapResolver(List<Person> lista)
        {
            var mapper = FactoryMapper.DynamicResolutionMapper<Person, PersonaGiuridica>();

            Stopwatch meter = new Stopwatch();       

            meter.Start();
            foreach (var person in lista)
            {
                mapper.Map(person);
            }
            meter.Stop();
            return meter.Elapsed;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        internal List<Person> GetPersons(int count)
        {
            List<Person> lista = new List<Person>();
            for (int i = 0; i < count; i++)
            {
                Person p = new Person
                {
                    AnnoNascita = i,
                    Name = string.Format("ciccio_{0}", i),
                    Surname = string.Format("pasticcio_{0}", i),
                    Parent = new Person
                    {
                        Name = string.Format("fatherCiccio_{0}", i),
                        Surname = string.Format("surnamefather_{0}", i),
                        AnnoNascita = i + 100
                    }
                };
                lista.Add(p);
            }
            return lista;
        }


        //[Test]
        public void TestReflection()
        {
            TransformerObserver a = new TransformerObserver();
            ITransformerResolver aa = a;

            ISourceMapper<Person, PersonaGiuridica> mapper0 = FactoryMapper.DynamicResolutionMapper<Person, PersonaGiuridica>();
            SourceMapper<Person, PersonaGiuridica> mapper = new SourceMapper<Person, PersonaGiuridica>(new List<IPropertyMapper<Person, PersonaGiuridica>>(), null, null);
            //aa.Register<ISourceMapper<Person, PersonaGiuridica>>(mapper);
            //aa.Register(mapper0);
            

            object obj1 = new Person();
            object obj2 = new Person();

            Type t1 = typeof(IPersonHeader);
            Type t2 = typeof(Person);

            try
            {
                object instance = 5.5;
                //int i = (int) instance;

                byte bb = Convert.ToByte(instance);

                byte b = (byte)instance;
                double d = (double) instance;
            }
            catch (Exception ex)
            {
                Assert.IsFalse(true, "Cast invalid");
            }

            Compare<IPersonHeader>(obj1, obj2);
            //Compare<long>(1, 10);
        }

        
        public void Compare<TObj>(object obj1, object obj2)
        {
            Assert.IsTrue(obj1 is TObj);

            Assert.IsTrue(obj2 is TObj);
        }
    }
}
