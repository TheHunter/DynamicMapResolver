﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DynamicMapResolver.Impl;
using DynamicMapResolver.Test.Pocos;
using NUnit.Framework;

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
    }
}
