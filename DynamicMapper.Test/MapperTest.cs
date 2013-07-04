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
        private Dictionary<Type, HashSet<Type>> primitiveTypes;

        [TestFixtureSetUp]
        public void TestOnSetUp()
        {
            primitiveTypes = new Dictionary<Type, HashSet<Type>>();

            primitiveTypes.Add
                (
                    typeof(byte?),
                    new HashSet<Type> {typeof (byte)}
                );

            primitiveTypes.Add
                (
                    typeof(short),
                    new HashSet<Type> { typeof(byte) }
                );

            primitiveTypes.Add
                (
                    typeof(short?),
                    new HashSet<Type> { typeof(byte), typeof(short), typeof(byte?) }
                );

            primitiveTypes.Add
                (
                    typeof(int),
                    new HashSet<Type> { typeof(byte), typeof(short) }
                );

            primitiveTypes.Add
                (
                    typeof(int?),
                    new HashSet<Type> { typeof(byte), typeof(short), typeof(int), typeof(byte?), typeof(short?) }
                );

            primitiveTypes.Add
                (
                    typeof(long),
                    new HashSet<Type> { typeof(byte), typeof(short), typeof(int) }
                );

            primitiveTypes.Add
                (
                    typeof(long?),
                    new HashSet<Type> { typeof(byte), typeof(short), typeof(int), typeof(long), typeof(byte?), typeof(short?), typeof(int?) }
                );

            //primitiveTypes.Add
            //    (
            //        typeof(decimal),
            //        new HashSet<Type> { typeof(byte), typeof(short), typeof(int), typeof(long) }
            //    );

            //primitiveTypes.Add
            //    (
            //        typeof(decimal?),
            //        new HashSet<Type> { typeof(byte), typeof(short), typeof(int), typeof(long), typeof(decimal), typeof(byte?), typeof(short?), typeof(int?), typeof(long?) }
            //    );

            primitiveTypes.Add
                (
                    typeof(decimal?),
                    new HashSet<Type> { typeof(decimal) }
                );

            primitiveTypes.Add
                (
                    typeof(float),
                    new HashSet<Type> { typeof(byte), typeof(short), typeof(int), typeof(long) }
                );

            primitiveTypes.Add
                (
                    typeof(float?),
                    new HashSet<Type> { typeof(byte), typeof(short), typeof(int), typeof(long), typeof(float), typeof(byte?), typeof(short?), typeof(int?), typeof(long?) }
                );

            primitiveTypes.Add
                (
                    typeof(double),
                    new HashSet<Type> { typeof(byte), typeof(short), typeof(int), typeof(long), typeof(float) }
                );

            primitiveTypes.Add
                (
                    typeof(double?),
                    new HashSet<Type> { typeof(byte), typeof(short), typeof(int), typeof(long), typeof(float), typeof(double), typeof(byte?), typeof(short?), typeof(int?), typeof(long?), typeof(float?) }
                );
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
            var studente = new Student();
            studente.Name = "ciccio";
            studente.AnnoNascita = 1;

            var persona = new Person();

            //Assert.AreNotEqual(studente.Name, persona.Name);
            Assert.AreNotEqual(studente.AnnoNascita, persona.AnnoNascita);
            var azione = PropertyMap<Student, Person>("AnnoNascita");
            azione.Invoke(studente, persona);
            Assert.AreEqual(studente.AnnoNascita, persona.AnnoNascita);
        }

        public Action<TSource, TDest> PropertyMap<TSource, TDest>(string propertyName)

        {
            Type funcType = typeof(Func<,>);
            Type ActType = typeof(Action<,>);

            Type Ts = typeof(TSource);
            Type Td = typeof(TDest);

            PropertyInfo propSource = Ts.GetProperty(propertyName);
            PropertyInfo propDest = Td.GetProperty(propertyName);

            if (propSource.PropertyType != propDest.PropertyType)
            {
                if (this.primitiveTypes.ContainsKey(propDest.PropertyType))
                {
                    if (!this.primitiveTypes[propDest.PropertyType].Contains(propSource.PropertyType))
                        throw new Exception("property getter is no compatible with property setter.");
                }
                else if (!propDest.PropertyType.IsAssignableFrom(propSource.PropertyType))
                {
                    throw new Exception("references property getter and setter are imcopatibles.");
                }
            }

            funcType = funcType.MakeGenericType(Ts, propSource.PropertyType);
            ActType = ActType.MakeGenericType(Td, propDest.PropertyType);

            Delegate getter = Delegate.CreateDelegate(funcType, null, propSource.GetGetMethod());
            Delegate setter = Delegate.CreateDelegate(ActType, null, propDest.GetSetMethod());

            // questa espression compila.. ma non garantisce che funzioni quando viene eseguita...
            Action<TSource, TDest> azione =
                (student, person) => setter.DynamicInvoke(person, getter.DynamicInvoke(student));


            //Expression.Lambda()
            var aa = Expression.Convert(null, null, propSource.GetGetMethod());

            return azione;

            
            

        }

        [Test]
        public void TestTypes()
        {
            // impazzito il compilatore???

            long a = 19801514514;
            decimal b = a;

            Assert.AreEqual(a, b);

            //Action<Student, Person> act = (student, person) => person.AnnoNascita = student.AnnoNascita;
            Action<Student, Person> act = delegate(Student student, Person person)
                {
                    person.AnnoNascita = student.AnnoNascita;
                };

            Student st = new Student();
            st.AnnoNascita = 10;

            Person pr = new Person();

            act.Invoke(st, pr);

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

    /// <summary>
    /// classe sorgente.
    /// </summary>
    public class Student
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        //public int AnnoNascita { get; set; }
        public byte AnnoNascita { get; set; }
    }

    public class PersonaGiuridica
        : Person
    {
        public string Code { get; set; }
    }
}
