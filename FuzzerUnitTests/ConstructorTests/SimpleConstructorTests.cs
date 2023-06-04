using FluentAssertions;
using FluentFuzzer.Constructors.ConstructorExceptions;
using FluentFuzzer.Constructors.Constructors;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuzzerUnitTests.ConstructorTests.ConstructClasses
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class SimpleConstructorTests
    {
        [Test]
        [Timeout(10000)]
        public void SimpleConstruct_ShouldBeOk()
        {
            var list = new List<string>();

            var randomConstructor = new SimpleConstructor<string>();
            randomConstructor.Add("1");
            randomConstructor.Add("2");
            randomConstructor.Add("3");
            randomConstructor.Add("4");
            randomConstructor.Add("5");

            for (int i = 0; i < 100; i++)
            {
                try
                {
                    var obj = randomConstructor.Construct<string>();
                    list.Add(obj);
                }
                catch (ConstructorBreakException)
                {
                    continue;
                }
            }

            list.Any(s => s == "1").Should().BeTrue();
            list.Any(s => s == "2").Should().BeTrue();
            list.Any(s => s == "3").Should().BeTrue();
            list.Any(s => s == "4").Should().BeTrue();
            list.Any(s => s == "5").Should().BeTrue();

            list.Should().HaveCount(5);
        }

        [Test]
        public void SimpleConstructor_ConstructOtherType_ShouldBeError()
        {
            var randomConstructor = new SimpleConstructor<string>();
            randomConstructor.Add("1");
            randomConstructor.Add("2");

            var constructIntResponse = () => randomConstructor.Construct<int>();

            constructIntResponse.Should().Throw<InvalidOperationException>();
        }
    }
}
