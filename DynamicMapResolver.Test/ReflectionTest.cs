using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DynamicMapResolver.Test.Domain;
using NUnit.Framework;

namespace DynamicMapResolver.Test
{
    [TestFixture]
    class ReflectionTest
    {
        [Test]
        public void Test3()
        {

            Type t1 = typeof(ISimpleMapper<Student, Person>);
            Type t2 = typeof(ISourceMapper<Student, Person>);

            Assert.IsTrue(t1.IsAssignableFrom(t2));
            Assert.IsFalse(t2.IsSubclassOf(t1));
        }

        [Test]
        public void Test4()
        {
            Type t1 = typeof(int);
            Type t2 = typeof(int?);

            Assert.IsTrue(t2.IsAssignableFrom(t1));
            Assert.IsFalse(t1.IsAssignableFrom(t2));
        }
    }
}
