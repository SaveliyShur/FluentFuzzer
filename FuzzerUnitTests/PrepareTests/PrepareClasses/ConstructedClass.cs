namespace TestApp
{
    public class ConstructedClass
    {
        public int Int { get; set; }

        public string StrintInMainClass { get; set; }

        public InnerClass InnerClass { get; set; }
    }

    public class InnerClass
    {
        public string StringInInnerClass { get; set; }

        public InnerEnum InnerEnum { get; set; }
    }

    public enum InnerEnum
    {
        One,
        Two,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Nine,
        Ten,
    }
}
