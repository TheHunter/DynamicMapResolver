using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynamicMapResolver.Test.Domain
{
    public class PersonDetails
    {
        private PersonDetails()
        {
        }

        public PersonDetails(string code)
        {
            this.Code = code;
        }

        public string Name { get; protected set; }

        public string Surname { get; protected set; }

        public double? AnnoNascita { get; protected set; }

        public Person Parent { get; protected set; }

        public string Code { get; private set; }

        public void UpdateNome(string name)
        {
            this.Name = name;
        }
    }
}
