using FuzzerRunner;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FuzzerUnitTests.FuzzerTests
{
    [TestFixture]
    public class FuzzerRunnerTests
    {

        [Test]
        public async Task FuzzerRunnerTimeoutTest()
        {
            var timer = new Stopwatch();
            timer.Start();

            await Fuzzer.Instance.RunAsync<string>(async text =>
            {
                await Task.Delay(100);
                if (DateTime.TryParse(text, out var dt1))
                {
                    var s = dt1.ToString("O");
                    var dt2 = DateTime.Parse(s, null, DateTimeStyles.RoundtripKind);

                    if (dt1 != dt2)
                    {
                        throw new Exception();
                    }
                }
            });

            timer.Stop();
            Assert.LessOrEqual(timer.ElapsedMilliseconds, 35000);
        }

        [Test]
        public async Task FuzzerRunnerParallelTimeoutTest()
        {
            var timer = new Stopwatch();
            timer.Start();

            await Fuzzer.Instance.MakeParallelExecution(2).RunAsync<string>(async text =>
            {
                await Task.Delay(100);
                if (DateTime.TryParse(text, out var dt1))
                {
                    var s = dt1.ToString("O");
                    var dt2 = DateTime.Parse(s, null, DateTimeStyles.RoundtripKind);

                    if (dt1 != dt2)
                    {
                        throw new Exception();
                    }
                }
            });

            timer.Stop();
            Assert.LessOrEqual(timer.ElapsedMilliseconds, 35000);
        }

        [Test]
        public async Task FuzzerRunnerParallel_CheckThreadsCount()
        {
            int counter = 0;
            int directCounter = 0;

            await Fuzzer.Instance.MakeParallelExecution(1).RunAsync<string>(async text =>
            {
                await Task.Delay(500);
                Interlocked.Increment(ref directCounter);
            }, 10);

            await Fuzzer.Instance.MakeParallelExecution(2).RunAsync<string>(async text =>
            {
                await Task.Delay(500);
                Interlocked.Increment(ref counter);
            }, 10);

            var dot = (double) counter / directCounter;

            Assert.IsTrue(dot > 1.9);
        }
    }
}
