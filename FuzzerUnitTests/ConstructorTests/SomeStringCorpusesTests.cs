using FluentAssertions;
using FuzzerRunner;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuzzerUnitTests.ConstructorTests
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class SomeStringCorpusesTests
    {
        [Test]
        public void AddTestStringCorpus_CheckAnyStringFromTestCopruse_ShouldBeOk()
        {
            var randomConstructor = new RandomTypeConstructor();

            for (var i = 0; i < 100; i++)
            {
                randomConstructor.AddStringToTestStringCorpus("TEST_STRING_CORPUSE");
            }

            var list = new List<string>(5000);

            for (int i = 0; i < 5000; i++)
            {
                var obj = randomConstructor.Construct<string>();
                list.Add(obj);
            }

            list.Should().Contain("TEST_STRING_CORPUSE");
        }
    }
}
