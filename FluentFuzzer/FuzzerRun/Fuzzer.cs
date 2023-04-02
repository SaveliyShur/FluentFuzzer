using FluentFuzzer.FuzzerExceptions;
using FuzzerRunner.FuzzerRun;
using Newtonsoft.Json;
using System.Diagnostics;

namespace FuzzerRunner
{
    public class Fuzzer : IFuzzer
    {
        public static readonly JsonSerializerSettings JsonSerializerSettings = new();

        static Fuzzer()
        {
            JsonSerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
        }

        private IConstructor _constructor = new RandomTypeConstructor();
        private int _threads = 1;

        private string? _folder = null;
        private string _testName = Guid.NewGuid().ToString();
        private bool _throwErrorIfAnyFailed = false;

        public static Fuzzer Instance => new ();

        public bool RunnerFailed { get; private set; } = false;

        public IFuzzer MakeParallelExecution(int threads)
        {
            _threads = threads;

            return this;
        }

        public async Task<IFuzzer> RunAsync<T>(Func<T, Task> action, int timeInSec = 30)
        {
            var taskList = new List<Task>(_threads);

            for (int i = 0; i < _threads; i++)
            {
                var task = RunOneAsync(action, timeInSec);
                taskList.Add(task);
            }

            await Task.WhenAll(taskList);

            if (RunnerFailed && _throwErrorIfAnyFailed)
                throw new RunnerTestFailedException("Test failed because any fuzzer run was fail.");

            return this;

        }

        public IFuzzer UseConstructor(IConstructor constructor)
        {
            _constructor = constructor;

            return this;
        }

        public IFuzzer WriteResultToFolder(string path)
        {
            if (!Directory.Exists(path))
                throw new DirectoryNotFoundException($"Directory {path} not found");

            _folder = path;

            return this;
        }

        public IFuzzer SetTestName(string testName)
        {
            _testName = testName;

            return this;
        }

        private async Task RunOneAsync<T>(Func<T, Task> action, int timeInSec)
        {
            var timer = new Stopwatch();
            timer.Start();
            while (timer.ElapsedMilliseconds < timeInSec * 1000)
            {
                T? input = default;
                try
                {
                    input = _constructor.Construct<T>();
                    await action(input);
                }
                catch (Exception ex)
                {
                    RunnerFailed = true;
                    await WriteResultAsync(input, ex.Message);
                }
            }

            timer.Stop();
        }

        private async Task WriteResultAsync<T>(T? input, string error)
        {
            string obj = string.Empty;
            if (typeof(T).IsClass)
                obj = JsonConvert.SerializeObject(input, JsonSerializerSettings);
            else if (input is not null)
                obj = input.ToString();

            if (_folder is null)
            {
                Console.WriteLine(error);
            }
            else
            {
                try
                {
                    var testFolder = Path.Combine(_folder, _testName);
                    if (!Directory.Exists(testFolder))
                        Directory.CreateDirectory(testFolder);

                    var commonPart = Guid.NewGuid().ToString();
                    var objectFileName = Path.Combine(testFolder, commonPart + "object.log");
                    var errorFileName = Path.Combine(testFolder, commonPart + "error.log");

                    await File.WriteAllTextAsync(objectFileName, obj);
                    await File.WriteAllTextAsync(errorFileName, error);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Cannot write result to folder. Exception: {ex.Message}");
                }
            }
        }

        public IFuzzer ThrowErrorIfAnyFailed()
        {
            _throwErrorIfAnyFailed = true;

            return this;
        }
    }
}
