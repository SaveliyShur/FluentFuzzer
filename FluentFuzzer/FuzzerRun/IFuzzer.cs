using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuzzerRunner.FuzzerRun
{
    public interface IFuzzer
    {
        Task RunAsync<T>(Func<T, Task> action, int timeInSec = 30);

        IFuzzer MakeParallelExecution(int threads);

        IFuzzer UseConstructor(IConstructor constructor);

        IFuzzer WriteResultToFolder(string path);
    }
}
