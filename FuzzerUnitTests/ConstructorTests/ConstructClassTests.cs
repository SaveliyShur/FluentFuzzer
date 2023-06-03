using FluentAssertions;
using FuzzerRunner;
using FuzzerUnitTests.ConstructorTests.ConstructClasses;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FuzzerUnitTests.ConstructorTests
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class ConstructClassTests
    {
        [Test]
        [Timeout(10000)]
        public void ConstructClass1_ShouldBeOk()
        {
            for (int i = 0; i < 100; i++)
            {
                var randomConstructor = new RandomTypeConstructor();
                var obj = randomConstructor.Construct<ConstructClass1>();
                if (obj is not null)
                {
                    Assert.AreEqual(obj.C, 0);
                }
            }
        }

        [Test]
        [Timeout(10000)]
        public void ConstructClass2_ShouldBeOk()
        {
            for (int i = 0; i < 100; i++)
            {
                var randomConstructor = new RandomTypeConstructor();
                randomConstructor.Construct<ConstructClass2>();
            }
        }

        [Test]
        [Timeout(10000)]
        public void ConstructClass3_ShouldBeOk()
        {
            for (int i = 0; i < 100; i++)
            {
                var randomConstructor = new RandomTypeConstructor();
                randomConstructor.Construct<ConstructClass3>();
            }
        }

        [Test]
        [Timeout(10000)]
        public void ConstructClass4_ShouldBeOk()
        {
            for (int i = 0; i < 100; i++)
            {
                var randomConstructor = new RandomTypeConstructor();
                randomConstructor.Construct<ConstructClass4>();
            }
        }

        [Test]
        [Timeout(10000)]
        public void ConstructClass5_ShouldBeOk()
        {
            for (int i = 0; i < 100; i++)
            {
                var randomConstructor = new RandomTypeConstructor();
                randomConstructor.Construct<ConstructClass5>();
            }
        }

        [Test]
        [Timeout(10000)]
        public void ConstructClass6_ShouldBeOk()
        {
            for (int i = 0; i < 100; i++)
            {
                var randomConstructor = new RandomTypeConstructor();
                randomConstructor.Construct<ConstructClass6>();
            }
        }

        [Test]
        [Timeout(10000)]
        public void ConstructDateTimeOffset_ShouldBeOk()
        {
            for (int i = 0; i < 10000; i++)
            {
                var randomConstructor = new RandomTypeConstructor();
                randomConstructor.Construct<DateTimeOffset>();
            }
        }

        [Test]
        [Timeout(10000)]
        public void ConstructDateTimet_ShouldBeOk()
        {
            for (int i = 0; i < 10000; i++)
            {
                var randomConstructor = new RandomTypeConstructor();
                randomConstructor.Construct<DateTime>();
            }
        }

        [Test]
        [Timeout(10000)]
        public void ConstructorGuid_WithoutStringCorpus_ShouldBeOk()
        {
            var guids = new List<Guid>();

            for (int i = 0; i < 10000; i++)
            {
                var randomConstructor = new RandomTypeConstructor();
                guids.Add(randomConstructor.Construct<Guid>());
            }

            guids.Any(g => g == Guid.Empty).Should().BeTrue();
            guids.Any(g => g != Guid.Empty).Should().BeTrue();
        }

        [Test]
        [Timeout(10000)]
        public void ConstructorGuid_WithStringCorpus_ShouldBeOk()
        {
            var guids = new List<Guid>();
            var guid = Guid.NewGuid();

            for (int i = 0; i < 10000; i++)
            {
                var randomConstructor = new RandomTypeConstructor();
                randomConstructor.AddStringToTestStringCorpus(guid.ToString());
                guids.Add(randomConstructor.Construct<Guid>());
            }

            guids.Any(g => g == Guid.Empty).Should().BeTrue();
            guids.Any(g => g != Guid.Empty).Should().BeTrue();
            guids.Any(g => g == guid).Should().BeTrue();
        }
    }
}
