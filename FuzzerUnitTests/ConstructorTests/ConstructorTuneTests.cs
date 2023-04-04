using FluentAssertions;
using FuzzerRunner;
using FuzzerUnitTests.ConstructorTests.ConstructClasses;
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
    public class ConstructorTuneTests
    {
        [Test]
        [Timeout(10000)]
        public void ConstructTuneClass_StringMaxLenght_ShouldBeOk()
        {
            for (int i = 0; i < 1000; i++)
            {
                var randomConstructor = new RandomTypeConstructor();
                randomConstructor.SetMaxStringLenght(5);
                var construction = randomConstructor.Construct<ConstructClass2>();

                if (construction is null)
                    continue;

                construction.A?.Length.Should().BeLessThanOrEqualTo(5);
            }
        }

        [Test]
        [Timeout(10000)]
        public void ConstructTuneInt_NotNull_ShouldBeOk()
        {
            for (int i = 0; i < 1000; i++)
            {
                var randomConstructor = new RandomTypeConstructor();
                randomConstructor.SetNotNullMainObject();
                var construction = randomConstructor.Construct<int>();
            }
        }

        [Test]
        [Timeout(10000)]
        public void ConstructTuneConstructClass2_NotNull_ShouldBeOk()
        {
            for (int i = 0; i < 1000; i++)
            {
                var randomConstructor = new RandomTypeConstructor();
                randomConstructor.SetNotNullMainObject();
                var construction = randomConstructor.Construct<ConstructClass2>();

                construction.Should().NotBeNull();
            }
        }
    }
}
