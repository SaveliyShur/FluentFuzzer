using System.Collections.Generic;

namespace FuzzerUnitTests.ConstructorTests.ConstructClasses
{
    public class IntClass
    {
        public int Int { get; set; } = 1;
    }

    public class OneListClass
    {
        public List<string> List { get; set; }
    }

    public class OneListWithInnerObject
    {
        public List<ConstructClass1> List { get; set; }
    }

    public class OneDictionary
    {
        public Dictionary<string, string> Dictionary { get; set; }
    }
}
