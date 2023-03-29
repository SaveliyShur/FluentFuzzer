using FuzzerRunner.FuzzerRun;
using Newtonsoft.Json;
using System.Diagnostics;

namespace FuzzerRunner
{
    public class Fuzzer : IFuzzer
    {
        private static readonly JsonSerializerSettings _jsonSerializerSettings = new();

        static Fuzzer()
        {
            _jsonSerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
        }

        private IConstructor _constructor = new RandomTypeConstructor();
        private int _threads = 1;

        private string? _folder = null;

        public static Fuzzer Instance => new ();

        public IFuzzer MakeParallelExecution(int threads)
        {
            _threads = threads;

            return this;
        }

        public async Task RunAsync<T>(Func<T, Task> action, int timeInSec = 30)
        {
            var taskList = new List<Task>(_threads);

            for (int i = 0; i < _threads; i++)
            {
                var task = RunOneAsync(action, timeInSec);
                taskList.Add(task);
            }

            await Task.WhenAll(taskList);

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

        private async Task RunOneAsync<T>(Func<T, Task> action, int timeInSec)
        {
            var timer = new Stopwatch();
            timer.Start();
            while (timer.ElapsedMilliseconds < timeInSec * 1000)
            {
                T? input = default(T);
                try
                {
                    input = _constructor.Construct<T>();
                    await action(input);
                }
                catch (Exception ex)
                {
                    await WriteResultAsync(input, ex.Message);
                }
            }

            timer.Stop();
        }

        private async Task WriteResultAsync<T>(T? input, string error)
        {
            string obj = string.Empty;
            if (typeof(T).IsClass)
                obj = JsonConvert.SerializeObject(input, _jsonSerializerSettings);
            else if (input is not null)
                obj = input.ToString();

            if (_folder is null)
            {
                Console.WriteLine(error);
            }
            else
            {
                var commonPart = Guid.NewGuid().ToString();
                var objectFileName = commonPart + "object.log";
                var errorFileName = commonPart + "error.log";

                using var fileObject = File.CreateText(objectFileName);
                await fileObject.WriteLineAsync(obj);

                using var errorFile = File.CreateText(errorFileName);
                await errorFile.WriteLineAsync(error);
            }
        }
    }
}
