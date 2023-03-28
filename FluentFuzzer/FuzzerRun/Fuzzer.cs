using FuzzerRunner.FuzzerRun;
using System.Diagnostics;

namespace FuzzerRunner
{
    public class Fuzzer : IFuzzer
    {
        private IConstructor _constructor = new RandomTypeConstructor();
        private int _threads = 1;

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
            throw new NotImplementedException();
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
            Console.WriteLine(error);
        }
    }
}
