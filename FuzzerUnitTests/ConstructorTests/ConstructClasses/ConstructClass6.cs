using System.Collections.Generic;

namespace FuzzerUnitTests.ConstructorTests.ConstructClasses
{
    public class ConstructClass6
    {
        public Dictionary<string, int> A { get; set; }

        public IDictionary<int, ConstructClass6_Inner> ConstructClass6_Inner { get; set; }

        public Dictionary<ConstructClass6_InnerEnum, string> ConstructClass6_InnerEnum { get; set; }
    }

    public class ConstructClass6_Inner
    {
        public int Inner { get; set; }
    }

    public enum ConstructClass6_InnerEnum
    {
        None,
        Enum,
        Class,
    }
}
