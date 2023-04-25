namespace FuzzerUnitTests.PrepareTests.PrepareClasses
{
    public class PrepareClass1
    {
        public string A { get; set; }
        public int B { get; set; }
        public PrepareClass1_Enum PrepareClass1_Enum { get; set; }
        public PrepareClass1_Inner? PrepareClass1_Inner { get; set; }

    }

    public enum PrepareClass1_Enum
    {
        Alex,
        Dora,
        Lena,
    }

    public class PrepareClass1_Inner
    {
        public string InnerString { get; set; }

        public int? InnerInt { get; set; }
    }
}
