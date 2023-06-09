﻿using FluentAssertions;
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
    public class StaticStringCorpuseTests
    {
        private readonly static string PathToMiscFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ConstructorTests", "Misc");

        [Test]
        public void AddStaticAndUserStringCorpus_CheckStringUser_NotStringStatic()
        {
            var staticPath = Path.Combine(PathToMiscFolder, "Static");
            var userPath = Path.Combine(PathToMiscFolder, "User");

            var randomConstructor = new RandomTypeConstructor();
            randomConstructor.UseStaticStringCorpus(staticPath);
            randomConstructor.UseStringCorpus(userPath);

            var list = new List<string>(10000);

            for (int i = 0; i < 10000; i++)
            {
                var obj = randomConstructor.Construct<string>();
                list.Add(obj);
            }

            list.Any(c => c.Contains("USER_CORPUSE")).Should().BeTrue();
            list.Any(c => c.Contains("STATIC_CORPUSE")).Should().BeFalse();
        }

        [Test]
        public void AddUserAndStaticStringCorpus_AddTestStrings_CheckString()
        {
            var staticPath = Path.Combine(PathToMiscFolder, "Static");
            var userPath = Path.Combine(PathToMiscFolder, "User");

            var randomConstructor = new RandomTypeConstructor();
            randomConstructor.UseStaticStringCorpus(staticPath);
            randomConstructor.UseStringCorpus(userPath);

            for (var i = 0; i < 100; i++)
            {
                randomConstructor.AddStringToTestStringCorpus("TEST_STRING_CORPUSE");
            }

            var list = new List<string>(50000);

            for (int i = 0; i < 50000; i++)
            {
                var obj = randomConstructor.Construct<string>();
                list.Add(obj);
            }

            list.Any(c => c.Contains("USER_CORPUSE")).Should().BeTrue();
            list.Any(c => c.Contains("TEST_STRING_CORPUSE")).Should().BeTrue();
            list.Any(c => c.Contains("STATIC_CORPUSE")).Should().BeFalse();
        }

        [Test]
        public void OffStandartCorpus_AddStaticCorpus_Construct_CheckString()
        {
            var staticPath = Path.Combine(PathToMiscFolder, "Static");

            var randomConstructor = new RandomTypeConstructor();
            randomConstructor.UseStaticStringCorpus(staticPath);
            randomConstructor.UseStandartStringCorpus(false);

            var list = new List<string>(10000);

            for (int i = 0; i < 10000; i++)
            {
                var obj = randomConstructor.Construct<string>();
                list.Add(obj);
            }

            list.All(c => c.Contains("STATIC_CORPUSE")).Should().BeTrue();
        }


        [Test]
        public void AddStaticStringCorpus_CheckString()
        {
            var staticPath = Path.Combine(PathToMiscFolder, "Static");
            var randomConstructor = new RandomTypeConstructor();
            randomConstructor.UseStaticStringCorpus(staticPath);

            var list = new List<string>(10000);

            for (int i = 0; i < 10000; i++)
            {
                var obj = randomConstructor.Construct<string>();
                list.Add(obj);
            }

            list.Any(c => c.Contains("STATIC_CORPUSE")).Should().BeTrue();
        }

        [Test]
        public async Task AddStaticStringCorpus_CheckString_Async()
        {
            var staticPath = Path.Combine(PathToMiscFolder, "Static");
            var randomConstructor = new RandomTypeConstructor();
            await randomConstructor.UseStaticStringCorpusAsync(staticPath);

            var list = new List<string>(10000);

            for (int i = 0; i < 10000; i++)
            {
                var obj = randomConstructor.Construct<string>();
                list.Add(obj);
            }

            list.Any(c => c.Contains("STATIC_CORPUSE")).Should().BeTrue();
        }
    }
}
