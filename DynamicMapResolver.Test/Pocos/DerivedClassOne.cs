using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynamicMapResolver.Test.Pocos
{
    public class DerivedClassOne
    {
        
        protected DerivedClassOne()
        {
        }
        public DerivedClassOne(int counter, string comment)
        {
            this.Counter = counter;
            this.Comment = comment;
        }

        public int Counter { get; private set; }

        public string Comment { get; private set; }
    }

    public class DerivedClassTwo
        : DerivedClassOne
    {
        protected DerivedClassTwo()
        {
        }

        public DerivedClassTwo(int counter, string comment, string commentTwo)
            :base(counter, comment)
        {
            this.CommentTwo = commentTwo;
        }

        public string CommentTwo { get; private set; }
    }

    public class DerivedClassLast
        : DerivedClassTwo
    {
        protected DerivedClassLast()
        {
        }

        public DerivedClassLast(int counter, string comment, string commentTwo, double avarage)
            : base(counter, comment, commentTwo)
        {
            this.Avarage = avarage;
        }

        public double Avarage { get; private set; }
    }
}
