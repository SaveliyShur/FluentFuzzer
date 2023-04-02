using FluentAssertions;
using FluentFuzzer.FuzzerExceptions;
using FuzzerRunner;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace FuzzerUnitTests.FuzzerTests
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class FuzzerRunnerTestsThrowError
    {
        [Test]
        public async Task RunFuzzer_ThrowErrorIfAnyTestException()
        {
            var request = async () => await Fuzzer.Instance
                .ThrowErrorIfAnyFailed()
                .MakeParallelExecution(4)
                .RunAsync<int>(async i =>
                {
                    await Task.Delay(100);
                    if (i == 0)
                        throw new Exception("Exception");
                }, timeInSec: 20);

            await request.Should().ThrowAsync<RunnerTestFailedException>();
        }

        [Test]
        public async Task RunFuzzer_ThrowErrorIfAnyTestException_RunInManyThreads()
        {
            var request = async () => await Fuzzer.Instance
                .ThrowErrorIfAnyFailed()
                .RunAsync<int>(async i =>
                {
                    await Task.Delay(100);
                    if (i == 0)
                        throw new Exception("Exception");
                }, timeInSec: 20);

            await request.Should().ThrowAsync<RunnerTestFailedException>();
        }

        [Test]
        public async Task RunFuzzer_DontThrowExceptionByDefault()
        {
            var request = async () => await Fuzzer.Instance
                .RunAsync<int>(async i =>
                {
                    await Task.Delay(100);
                    if (i == 0)
                        throw new Exception("Exception");
                }, timeInSec: 20);

            await request.Should().NotThrowAsync<RunnerTestFailedException>();
        }
    }
}
