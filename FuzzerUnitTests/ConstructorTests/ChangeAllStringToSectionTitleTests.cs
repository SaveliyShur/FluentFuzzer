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
    public class ChangeAllStringToSectionTitleTests
    {
        [Test]
        public void ChangeAllStringToSectuion_BaseTest_ShouldBeOk()
        {
            var randomConstructor = new RandomTypeConstructor();
            var oldString = randomConstructor.Construct<string>();
            var titleObject = randomConstructor.ChangeAllStringToSectionTitles(oldString);

            titleObject.Should().NotBeNull();
            var title = (string)titleObject;
            title.Should().NotBe(oldString);
        }

        [Test]
        [Timeout(10000)]
        public void ChangeAllStringToSectuion_DifficultTest_ShouldBeOk()
        {
            for (var i = 0; i < 100; i++)
            {
                var randomConstructor = new RandomTypeConstructor();
                var oldObject = randomConstructor.Construct<RandomClassWithInner>();
                var newObject = randomConstructor.ChangeAllStringToSectionTitles(oldObject);

                if (oldObject is null)
                    continue;
            }
        }
    }

    public class RandomClassWithInner
    { 
        public RandomClassInner Inner { get; set; }

        public string Str { get; set; }
    }

    public class RandomClassInner
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
