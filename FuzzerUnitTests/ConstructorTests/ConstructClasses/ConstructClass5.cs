using System.Collections.Generic;

namespace FuzzerUnitTests.ConstructorTests.ConstructClasses
{
    public class ConstructClass5
    {
        public List<string> List { get; set; }

        public IList<int> List2 { get; set; }

        public List<string>? List3 { get; set; }

        public List<ConstructClass5_InnerList> List4 { get; set; }
    }

    public class ConstructClass5_InnerList
    {
        public int Int { get; set; }
    }
}
