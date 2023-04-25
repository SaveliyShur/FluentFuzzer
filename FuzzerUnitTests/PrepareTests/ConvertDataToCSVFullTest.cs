using FluentAssertions;
using FluentFuzzer.DataPreparation;
using FuzzerRunner;
using FuzzerUnitTests.PrepareTests.PrepareClasses;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuzzerUnitTests.PrepareTests
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class ConvertDataToCSVFullTest
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

        [Test]
        public async Task ConvertToCsv_CheckCsv_ShouldBeOk()
        {
            var request = await Fuzzer.Instance
                .MakeParallelExecution(4)
                .WriteResultToFolder(_testDir)
                .SetTestName(_testName)
                .RunAsync<PrepareClass1>(async prep =>
                {
                    if (prep == null)
                        return;

                    await Task.Delay(100);
                    if (prep.PrepareClass1_Inner is null)
                        throw new Exception("1");
                    if (prep.PrepareClass1_Enum == PrepareClass1_Enum.Lena)
                        throw new Exception("2");
                    throw new Exception("3");
                }, timeInSec: 2);

            var files = Directory.GetFiles(_fullTestFolder).Where(f => f.EndsWith("error.log")).ToArray();
            foreach (var file in files)
            {
                var newFileName = file.Replace("error.log", "") + DataPrepare<PrepareClass1>.END_CLASS_LABEL_FILE_NAME;
                File.Move(file, newFileName);
            }

            var dataPrepare = new DataPrepare<PrepareClass1>();
            await dataPrepare.UploadDataAsync(_fullTestFolder);
            var pathToTable = await dataPrepare.PrepareDataToDataTableAsync(_fullTestFolder);

            var dataAfterCsv = await dataPrepare.UploadDataTableAsync(pathToTable);
            dataAfterCsv.Should().HaveCount(files.Length);
        }

        [Test]
        public async Task ConvertToCsv_CheckCsv_InnerObjectIsNull_ShouldBeOk()
        {
            var path = Path.Combine(_testDir, _testName);
            var obj = new PrepareClass1()
            {
                A = "1010",
                B = 1,
                PrepareClass1_Inner = null,
            };

            var objJson = JsonConvert.SerializeObject(obj);

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            var commonPart = Guid.NewGuid().ToString();
            var objectFileName = Path.Combine(path, commonPart + DataPrepare<PrepareClass1>.END_OBJECT_FILE_NAME);
            var errorFileName = Path.Combine(path, commonPart + DataPrepare<PrepareClass1>.END_CLASS_LABEL_FILE_NAME);

            await File.WriteAllTextAsync(objectFileName, objJson);
            await File.WriteAllTextAsync(errorFileName, "1");

            var dataPrepare = new DataPrepare<PrepareClass1>();
            await dataPrepare.UploadDataAsync(_fullTestFolder);
            var pathToTable = await dataPrepare.PrepareDataToDataTableAsync(_fullTestFolder);

            var dataAfterCsv = await dataPrepare.UploadDataTableAsync(pathToTable);
            dataAfterCsv.Should().HaveCount(1);
            var dataFirst = dataAfterCsv.First();
            dataFirst.A = "1010";
            dataFirst.B = 1;
            dataFirst.PrepareClass1_Inner = null;
        }

        [TearDown]
        public void TearDown()
        {
            if (Directory.Exists(_fullTestFolder))
                Directory.Delete(_fullTestFolder, true);
        }
    }
}
