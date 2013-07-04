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
        private Dictionary<Type, HashSet<Type>> primiteTypes;

        [TestFixtureSetUp]
        public void TestOnSetUp()
        {
            primiteTypes = new Dictionary<Type, HashSet<Type>>();

            Type currentKey;
            HashSet<Type> col;

            currentKey = typeof(byte);
            col = new HashSet<Type>();
            col.Add(currentKey);
            primiteTypes.Add(currentKey, col);

            currentKey = typeof(byte?);
            col = new HashSet<Type>(col);
            col.Add(currentKey);
            primiteTypes.Add(currentKey, col);



            currentKey = typeof(short);
            col = new HashSet<Type>();
            col.Add(typeof(byte));
            col.Add(currentKey);
            primiteTypes.Add(currentKey, col);

            currentKey = typeof(short?);
            col = new HashSet<Type>(col);
            col.Add(typeof(byte?));
            col.Add(currentKey);
            primiteTypes.Add(currentKey, col);



            currentKey = typeof(int);
            col = new HashSet<Type>();
            col.Add(typeof(byte));
            col.Add(typeof(short));
            col.Add(currentKey);
            primiteTypes.Add(currentKey, col);

            currentKey = typeof(int?);
            col = new HashSet<Type>(col);
            col.Add(typeof(byte?));
            col.Add(typeof(short?));
            col.Add(currentKey);
            primiteTypes.Add(currentKey, col);
        }

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
            studente.AnnoNascita = 1980;

            var persona = new Person();

            string nameProperty = "Name";
            nameProperty = "AnnoNascita";

            PropertyInfo propSource = TSource.GetProperty(nameProperty);
            PropertyInfo propDest = TDest.GetProperty(nameProperty);

            funcType = funcType.MakeGenericType(TSource, propSource.PropertyType);
            ActType = ActType.MakeGenericType(TDest, propDest.PropertyType);

            Delegate getter = Delegate.CreateDelegate(funcType, null, propSource.GetGetMethod());
            Delegate setter = Delegate.CreateDelegate(ActType, null, propDest.GetSetMethod());

            // questa espression compila.. ma non garantisce che funzioni quando viene eseguita...
            Action<Student, Person> azione =
                (student, person) => setter.DynamicInvoke(person, getter.DynamicInvoke(student));

            //Assert.AreNotEqual(studente.Name, persona.Name);
            Assert.AreNotEqual(studente.AnnoNascita, persona.AnnoNascita);

            azione.Invoke(studente, persona);

            //Assert.AreEqual(studente.Name, persona.Name);
            Assert.AreEqual(studente.AnnoNascita, persona.AnnoNascita);

            Console.WriteLine("ciao");

        }

        [Test]
        public void Test3()
        {
            
        }

    }

    /// <summary>
    /// classe destinazione
    /// </summary>
    public class Person
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        //public long AnnoNascita { get; set; }
        public int AnnoNascita { get; set; }
    }

    public class PersonaGiuridica
        : Person
    {
        public string Code { get; set; }
    }

    /// <summary>
    /// classe sorgente.
    /// </summary>
    public class Student
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        //public int AnnoNascita { get; set; }
        public long AnnoNascita { get; set; }
    }
}
