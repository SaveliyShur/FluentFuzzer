using FluentAssertions;
using FluentFuzzer.Constructors.ConstructorExceptions;
using FuzzerRunner;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FuzzerUnitTests.ConstructorTests
{
    [TestFixture]
    [NonParallelizable]
    public class SomeStringCorpusesTests
    {
        private readonly static string PathToMiscFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ConstructorTests", "Misc");
        
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

        [Test]
        public void AddUserStringCorpus_CheckString()
        {
            var userPath = Path.Combine(PathToMiscFolder, "User");
            var randomConstructor = new RandomTypeConstructor();
            randomConstructor.UseStringCorpus(userPath);

            var list = new List<string>(10000);

            for (int i = 0; i < 10000; i++)
            {
                var obj = randomConstructor.Construct<string>();
                list.Add(obj);
            }

            list.Any(c => c.Contains("USER_CORPUSE")).Should().BeTrue();
        }

        [Test]
        public async Task AddUserStringCorpus_CheckStringAsync()
        {
            var userPath = Path.Combine(PathToMiscFolder, "User");
            var randomConstructor = new RandomTypeConstructor();
            await randomConstructor.UseStringCorpusAsync(userPath);

            var list = new List<string>(10000);

            for (int i = 0; i < 10000; i++)
            {
                var obj = randomConstructor.Construct<string>();
                list.Add(obj);
            }

            list.Any(c => c.Contains("USER_CORPUSE")).Should().BeTrue();
        }

        [Test]
        public void OffStandartCorpus_Construct_Error()
        {
            var randomConstructor = new RandomTypeConstructor();
            randomConstructor.UseStandartStringCorpus(false);

            var constrRequest = () => randomConstructor.Construct<string>();
            constrRequest.Should().Throw<ConstructException>();
        }
    }
}
