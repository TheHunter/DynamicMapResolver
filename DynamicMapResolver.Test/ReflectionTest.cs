using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DynamicMapResolver.Impl;
using DynamicMapResolver.Test.Pocos;
using NUnit.Framework;
using System.Reflection;

namespace DynamicMapResolver.Test
{
    [TestFixture]
    class ReflectionTest
    {
        [Test]
        public void Test1()
        {
            Type t1 = typeof(ISourceMapper<Student, Person>);
            Type t2 = typeof(SourceMapper<Student, Person>);

            Assert.IsTrue(t1.IsAssignableFrom(t2));
        }

        [Test]
        public void Test3()
        {

            Type t1 = typeof(ISimpleMapper<Student, Person>);
            Type t2 = typeof(ISourceMapper<Student, Person>);

            Assert.IsTrue(t1.IsAssignableFrom(t2));
            Assert.IsFalse(t2.IsSubclassOf(t1));

            Person p1 = new Person();
            PersonaGiuridica p2 = new PersonaGiuridica();

            Type t0 = p1.GetType();
            Assert.IsTrue(t0.IsInstanceOfType(p1));
            Assert.IsTrue(t0.IsInstanceOfType(p2));

        }

        [Test]
        public void Test4()
        {
            Type t1 = typeof(int);
            Type t2 = typeof(int?);

            Assert.IsTrue(t2.IsAssignableFrom(t1));
            Assert.IsFalse(t1.IsAssignableFrom(t2));
        }

        [Test]
        public void TestOnBasePropertiesInterface()
        {
            TransformerObserver observer = new TransformerObserver();
            observer.BuildAutoResolverMapper<IClassC, Demo2>(null, null);

            Demo1 demo = new Demo1
            {
                MyAProp = "myprop1",
                MyBProp = "myprop2",
                MyBBProp = "myprop3",
                MyCProp = "myprop4"
            };

            var res = observer.TryToMap<IClassC, Demo2>(demo);
            Assert.AreEqual(demo.MyAProp, res.MyAProp);
            Assert.AreEqual(demo.MyBProp, res.MyBProp);
            Assert.AreEqual(demo.MyBBProp, res.MyBBProp);
            Assert.AreEqual(demo.MyCProp, res.MyCProp);
        }

        [Test]
        public void TestA()
        {
            
            Type t2 = typeof(IClassB);
            Type t3 = typeof(IClassBB);

            var t2Prop = t2.GetProperty("MyBProp");
            var t3Prop = t3.GetProperty("MyBProp");

            var demo = new Demo1();
            demo.MyBProp = "ciao";

            var val1 = t2Prop.GetValue(demo, null);
            var val2 = t3Prop.GetValue(demo, null);

            Assert.AreNotEqual(t2Prop, t3Prop);

            Assert.AreEqual(val1, val2);
            Assert.AreEqual(val1, demo.MyBProp);
            Assert.AreEqual(val2, demo.MyBProp);
        }

        [Test]
        public void TestOnAllProperties()
        {
            
            Type c = typeof(IClassC);
            var res = FactoryMapper.GetPropertiesOf(c);
            
            Assert.AreEqual(4, res.Length);
            
        }


        public interface IClassA
        {
            string MyAProp { get; }
        }


        public interface IClassB
            : IClassA
        {
            string MyBProp { get; }
        }


        public interface IClassBB
            : IClassA
        {
            string MyBProp { get; set; }
            string MyBBProp { get; }
        }


        public interface IClassC
            : IClassB, IClassBB, IClassA
        {
            string MyCProp { get; }
        }


        public class Demo1
            : IClassC
        {
            public string MyCProp { get; set; }
            public string MyBProp { get; set; }
            public string MyAProp { get; set; }
            public string MyBBProp { get; set; }
        }


        public class Demo2
        {
            public string MyCProp { get; set; }
            public string MyBProp { get; set; }
            public string MyAProp { get; set; }
            public string MyBBProp { get; set; }
        }
    }
}
