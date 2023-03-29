using FuzzerRunner;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FuzzerUnitTests.FuzzerTests
{
    public class ConstructClass
    {
        public string Name { get; set; }

        public List<string> NameChildren { get; set; }

        public int Age { get; set; }
    }

    [TestFixture]
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
