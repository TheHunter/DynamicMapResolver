using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;

namespace DynamicMapResolver.Test.Pocos
{
    public class BaseClassOne
    {
        protected BaseClassOne()
        {
        }

        public BaseClassOne(int counter, string comment)
        {
            this.Counter = counter;
            this.Comment = comment;
        }

        public int Counter { get; private set; }

        public string Comment { get; private set; }
    }

    public class BaseClassTwo
        : BaseClassOne
    {
        protected BaseClassTwo()
        {
        }

        public BaseClassTwo(int counter, string comment, string commentTwo)
            :base(counter, comment)
        {
            this.CommentTwo = commentTwo;
        }

        public string CommentTwo { get; private set; }
    }

    public class MyDerivedClass
        : BaseClassTwo
    {
        protected MyDerivedClass()
        {
        }

        public MyDerivedClass(int counter, string comment, string commentTwo, double avarage)
            : base(counter, comment, commentTwo)
        {
            this.Avarage = avarage;
        }

        public double Avarage { get; private set; }
    }
}
