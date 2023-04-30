namespace FluentFuzzer.Constructors.ConstructorExceptions
{
    public class ConstructorBreakException : Exception
    {
        public ConstructorBreakException(string exception)
            : base(exception)
        {
        }
    }
}
