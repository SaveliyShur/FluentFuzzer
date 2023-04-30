using FluentAssertions;
using FluentFuzzer.Constructors.Constructors;
using FluentFuzzer.DataPreparation;
using FuzzerRunner;
using FuzzerUnitTests.FuzzerTests.Helpers;
using FuzzerUnitTests.PrepareTests.PrepareClasses;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FuzzerUnitTests.FuzzerTests
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class FuzzerRunWithSimpleConstructor
    {
        private static readonly string _testName = "TestName";
        private string _testDir;
        private string _fullTestFolder;

        [SetUp]
        public void SetUp()
        {
            var tmpPath = Path.GetTempPath();
            _testDir = Path.Combine(tmpPath, Guid.NewGuid().ToString());
            Directory.CreateDirectory(_testDir);

            _fullTestFolder = Path.Combine(_testDir, _testName);
        }

        [TearDown]
        public void TearDown()
        {
            if (Directory.Exists(_fullTestFolder))
                Directory.Delete(_fullTestFolder, true);
        }

        [Test]
        public async Task SimpleConstructor_RunFuzzerWithRandomConstructor_ChangeStringOnTitle_RunSimpleShouldBeOk()
        {
           await Fuzzer.Instance
                .WriteResultToFolder(_testDir)
                .SetTestName(_testName)
                .ChangeAllStringInObjectToStringSectionsTitles()
                .RunAsync<PrepareClass1>(async prep =>
                {
                    if (prep == null)
                        return;

                    await Task.Delay(100);
                    if (prep.PrepareClass1_Inner is null)
                        throw new Exception("1");
                    if (prep.PrepareClass1_Enum == PrepareClass1_Enum.Lena)
                        throw new Exception("2");
                }, timeInSec: 10);

            var dataPrepare = new DataPrepare<PrepareClass1>();
            await dataPrepare.UploadDataAsync(_fullTestFolder, false);

            var simpleConstructor = new SimpleConstructor<PrepareClass1>();
            simpleConstructor.UploadWithStringsChangedOnSectionTitles(dataPrepare.Data.Select(d => d.DataObject).ToList());

            await Fuzzer.Instance
                .UseConstructor(simpleConstructor)
                .WriteResultToFolder(_testDir)
                .SetTestName(_testName + "_Simple")
                .RunAsync<PrepareClass1>(async prep =>
                {
                    if (prep == null)
                        return;

                    await Task.Delay(100);
                    if (prep.PrepareClass1_Inner is null)
                        throw new Exception("1");
                    if (prep.PrepareClass1_Enum == PrepareClass1_Enum.Lena)
                        throw new Exception("2");
                }, timeInSec: 10);

            // Assert
            var newPath = Path.Combine(_testDir, _testName + "_Simple");
            var files = Directory.GetFiles(newPath).Where(f => f.EndsWith("object.log")).ToArray();
            dataPrepare.Data.Count.Should().Be(files.Length);
        }
    }
}
