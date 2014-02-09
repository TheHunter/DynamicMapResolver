using System;
using System.Collections.Generic;
using System.Diagnostics;
using AutoMapper;
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

        [Test]
        public void VerifyElapsedTime()
        {
            int count = 1000000;

            var timeSpanMapResolver = GetElapsedFromMapResolver(count);
            var timeSpanAutomapper = GetElapsedTimeFromAutoMapper(count);

            Assert.IsTrue(timeSpanMapResolver.TotalMilliseconds < timeSpanAutomapper.TotalMilliseconds);
        }


        public TimeSpan GetElapsedTimeFromAutoMapper(int count)
        {
            Mapper.CreateMap<Person, PersonaGiuridica>();

            Stopwatch meter = new Stopwatch();
            List<Person> lista = GetPersons(count);

            meter.Start();
            foreach (var person in lista)
            {
                Mapper.Map<Person, PersonaGiuridica>(person);
            }
            meter.Stop();
            return meter.Elapsed;
        }


        public TimeSpan GetElapsedFromMapResolver(int count)
        {
            var mapper = FactoryMapper.DynamicResolutionMapper<Person, PersonaGiuridica>();

            Stopwatch meter = new Stopwatch();       
            List<Person> lista = GetPersons(count);

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
    }
}
