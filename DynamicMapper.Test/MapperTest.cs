using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using NUnit.Framework;

namespace DynamicMapper.Test
{
    [TestFixture]
    public class MapperTest
    {
        [Test]
        public void Test1()
        {
            var studente = new Student();
            studente.Name = "ciccio";

            var persona = new Person();
            

            Type TSource = typeof(Student);
            Type TDestination = typeof(Person);

            Func<Student, string> cc = (Func<Student, string>)Delegate.CreateDelegate(typeof(Func<Student, string>), null, TSource.GetProperty("Name").GetGetMethod());
            Action<Person, string> dd = (Action<Person, string>)Delegate.CreateDelegate(typeof(Action<Person, string>), null, TDestination.GetProperty("Name").GetSetMethod());
            

            Action<Student, Person> azione = (student, person) => dd.Invoke(person, cc.Invoke(student));

            Assert.AreNotEqual(studente.Name, persona.Name);

            azione.Invoke(studente, persona);

            Assert.AreEqual(studente.Name, persona.Name);

            Console.WriteLine(cc.ToString());
            Console.WriteLine(dd.ToString());
            

            Assert.IsTrue(true);
        }

        [Test]
        public void Test2()
        {
            Type funcType = typeof (Func<,>);
            Type ActType = typeof (Action<,>);

            Type TSource = typeof (Student);
            Type TDest = typeof (Person);

            var studente = new Student();
            studente.Name = "ciccio";

            var persona = new Person();


            string nameProperty = "Name";
            PropertyInfo propSource = TSource.GetProperty(nameProperty);
            PropertyInfo propDest = TDest.GetProperty(nameProperty);

            funcType = funcType.MakeGenericType(TSource, propSource.PropertyType);
            ActType = ActType.MakeGenericType(TDest, propDest.PropertyType);

            Delegate getter = Delegate.CreateDelegate(funcType, null, propSource.GetGetMethod());
            Delegate setter = Delegate.CreateDelegate(ActType, null, propDest.GetSetMethod());

            Action<Student, Person> azione = (student, person) => setter.DynamicInvoke(person, getter.DynamicInvoke(student));

            Assert.AreNotEqual(studente.Name, persona.Name);

            azione.Invoke(studente, persona);

            Assert.AreEqual(studente.Name, persona.Name);

            Console.WriteLine("ciao");

            


        }

    }

    public class Person
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public long AnnoNascita { get; set; }
    }

    public class PersonaGiuridica
        : Person
    {
        public string Code { get; set; }
    }

    public class Student
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public int AnnoNascita { get; set; }
    }
}
