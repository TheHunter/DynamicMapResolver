using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynamicMapResolver.Test.Pocos
{
    public class CustomComplexType
    {
        public KeyService MyKeyService { get; set; }

        public string ComplexNaming { get; set; }

        public User Owner { get; set; }
    }

    public class CustomComplexTypeDto
    {
        public KeyServiceOther MyKeyService { get; set; }

        public string ComplexNaming { get; set; }

        public UserDto Owner { get; set; }
    }


    public class CustomSimpleType
    {
        public string Naming { get; set; }

        public int Code { get; set; }
    }


    public class CustomSimpleTypeDto
    {
        public string Naming { get; set; }

        public int? Code { get; set; }
    }
}
