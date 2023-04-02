using FuzzerRunner;
using FuzzerUnitTests.FuzzerTests.Helpers;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace FuzzerUnitTests.FuzzerTests
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class FuzzerRunnerConstructTests
    {
        [Test]
        public async Task FuzzerTestConstructObject()
        {
            await Fuzzer.Instance.RunAsync<ConstructClass>(async construction =>
            {
                await Task.Delay(100);
                if (construction is not null)
                    Console.WriteLine(construction.Name + " " + construction.NameChildren?.Count + " " + construction.Age);
            },
            timeInSec: 15);
        }
    }
}
