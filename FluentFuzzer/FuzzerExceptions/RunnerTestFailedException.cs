namespace FluentFuzzer.FuzzerExceptions
{
    public class RunnerTestFailedException : Exception
    {
        public RunnerTestFailedException(string exception)
            : base(exception)
        {
        }
    }
}
