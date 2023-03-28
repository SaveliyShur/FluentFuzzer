using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuzzerUnitTests.ConstructorTests.ConstructClasses
{
    public class ConstructClass2
    {
        public string A { get; set; }

        public ConstructClass2_Inner ConstructClass2_Inner { get; set; }

        public ConstructClass2_Inner ConstructClass2_Inner_2 { get; }
    }

    public class ConstructClass2_Inner
    {
        public string B { get; set; }

        public int C { get; set; }
    }
}
