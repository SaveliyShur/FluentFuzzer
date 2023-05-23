namespace FuzzerRunner.FuzzerRun
{
    public interface IFuzzer
    {
        /// <summary>
        /// Start fuzzing.
        /// </summary>
        /// <typeparam name="T">Type for construction</typeparam>
        /// <param name="action">Function for fuzzing</param>
        /// <param name="timeInSec">Time to end fussing</param>
        /// <returns></returns>
        Task<IFuzzer> RunAsync<T>(Func<T, Task> action, int timeInSec = 30);

        /// <summary>
        /// Set parallel execution.
        /// </summary>
        /// <param name="threads">Threads number</param>
        /// <returns>Current instance</returns>

        IFuzzer MakeParallelExecution(int threads);

        /// <summary>
        /// Set custom constructor. Default is RandomTypeConstructor
        /// </summary>
        /// <param name="constructor">Constructor</param>
        /// <returns>Current instance</returns>
        IFuzzer UseConstructor(IConstructor constructor);

        /// <summary>
        /// Set folder for write error results.
        /// </summary>
        /// <param name="path">Path to folder</param>
        /// <returns>Current instance</returns>
        IFuzzer WriteResultToFolder(string path);

        /// <summary>
        /// Set test name. Test name will use for name subfolder for this test.
        /// </summary>
        /// <param name="testName">Test name or something</param>
        /// <returns>Current instance</returns>
        IFuzzer SetTestName(string testName);

        /// <summary>
        /// If any fuzzing retry were failed, would throw RunnerTestFailedException
        /// </summary>
        /// <returns>Current instance</returns>
        /// <exception cref="RunnerTestFailedException">Any fuzzing retry were failed</exception>
        IFuzzer ThrowErrorIfAnyFailed();

        /// <summary>
        /// WARNING! Change all string in object to string blocks titles. It may by use to AI generator object and run in fuzzer.
        /// </summary>
        /// <returns></returns>
        IFuzzer ChangeAllStringInObjectToStringSectionsTitles();

        IFuzzer AddNormalObjectToMutation();
    }
}
