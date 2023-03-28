using FuzzerRunner;
using FuzzerUnitTests.ConstructorTests.ConstructClasses;
using NUnit.Framework;
using System;

namespace FuzzerUnitTests.ConstructorTests
{
    [TestFixture]
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
    }
}
