namespace FuzzerUnitTests.ConstructorTests.ConstructClasses
{
    public class ConstructClass4
    {
        public ConstructClass4_InnerEnum ConstructClass4_InnerEnum { get; set; }

        public ConstructClass4_InnerEnum? ConstructClass4_InnerEnum_Nullable { get; set; }

        public ConstructClass4_InnerEnum_2 ConstructClass4_InnerEnum_2 { get; set; }

        public ConstructClass4_InnerEnum_2? ConstructClass4_InnerEnum_2_Nullable { get; set; }

        public ConstructClass4_InnerEnum_3 ConstructClass4_InnerEnum_3 { get; set; }
    }

    public enum ConstructClass4_InnerEnum
    {
        None,
        Black,
        White,
    }

    public enum ConstructClass4_InnerEnum_2 : short
    {
        None,
        Hello,
        Olleh,
    }

    public enum ConstructClass4_InnerEnum_3 
    {
        None,
        Greko = 100000,
        Glass = 1000,
    }
}
