using FuzzerRunner;
using FuzzerUnitTests.FuzzerTests.Helpers;
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
    public class FuzzerRunnerWriteResultToFolderTests
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
            var testName = "TestName";

            _fullTestFolder = Path.Combine(_testDir, testName);
        }

        [Test]
        [Repeat(100)]
        public async Task FuzzerRunnerWriteToFolderTest()
        {
            // Arrange

            ConstructClass? constructClass = null;

            // Act
            await Fuzzer.Instance
                .WriteResultToFolder(_testDir)
                .SetTestName(_testName)
                .RunAsync<ConstructClass>(async construction =>
                {
                    constructClass = construction;
                    await Task.Delay(1000);
                    if (construction != null)
                        throw new Exception("Inner exceprion");
                },
                timeInSec: 1);

            if (constructClass is null)
                return;

            // Assert
            var files = Directory.GetFiles(_fullTestFolder);
            Assert.AreEqual(files.Count(), 2);
            var objectFile = files.FirstOrDefault(f => f.EndsWith("object.log"));
            Assert.IsNotNull(objectFile);
            var objectInFile = await File.ReadAllTextAsync(objectFile);
            Assert.IsNotNull(objectInFile);
            Assert.IsNotEmpty(objectInFile);
            ConstructClass inFile = JsonConvert.DeserializeObject<ConstructClass>(objectInFile);
            Assert.IsNotNull(inFile);

            Assert.IsTrue(inFile.Equals(constructClass));
        }

        [TearDown]
        public void TearDown()
        {
            if (Directory.Exists(_fullTestFolder))
                Directory.Delete(_fullTestFolder, true);
        }
    }
}
