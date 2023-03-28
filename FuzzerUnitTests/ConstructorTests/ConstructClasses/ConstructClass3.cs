using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuzzerUnitTests.ConstructorTests.ConstructClasses
{
    public class ConstructClass3
    {
        public int? A { get; set; }

        public string? B { get; set; }

        public ConstructClass3_Inner? ConstructClass3_Inner { get; set; }
    }

    public class ConstructClass3_Inner
    {
        public int A { get; set; }
    }
}
