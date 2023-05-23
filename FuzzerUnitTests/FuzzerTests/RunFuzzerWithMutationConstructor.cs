using FluentAssertions;
using FluentFuzzer.Constructors.Constructors;
using FuzzerRunner;
using FuzzerUnitTests.FuzzerTests.Helpers;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FuzzerUnitTests.FuzzerTests
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class RunFuzzerWithMutationConstructor
    {
        [Test]
        [Timeout(100000)]
        public async Task SetAddObject_WithMutationConstructor_ShouldBeOk()
        {
            var mutationConstructor = new MutationConstructor<ConstructClass>();
            mutationConstructor.SetCountMutation(1);
            mutationConstructor.SetNotNullMainObject();
            mutationConstructor.Add(new ConstructClass()
            {
                Age = 15,
                Name = "Alex",
                NameChildren = new System.Collections.Generic.List<string>()
                {
                    "Nona",
                }
            });

            var counter = 0;

            await Fuzzer.Instance
                .UseConstructor(mutationConstructor)
                .AddNormalObjectToMutation()
                .RunAsync<ConstructClass>(async testClass =>
                {
                    await Task.Delay(100);
                    if (testClass.Age == 15)
                    {
                        throw new Exception("Exception");
                    }

                    counter++;
                });

            counter.Should().BeGreaterThan(5);
            mutationConstructor.Objects.Should().NotHaveCount(1);
            mutationConstructor.Objects.Count(o => o.Age == 15).Should().Be(1);
        }
    }
}
