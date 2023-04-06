namespace FluentFuzzer.Constructors.ConstructorExceptions
{
    public class ConstructException : Exception
    {
        public ConstructException(string exception)
            : base(exception)
        {
        }
    }
}
