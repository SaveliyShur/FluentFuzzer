using FluentAssertions;
using FluentFuzzer.DataPreparation;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestApp;

namespace FuzzerUnitTests.PrepareTests
{
    [TestFixture]
    [Parallelizable(ParallelScope.Fixtures)]
    public class IntegrationPrepareTests
    {
        private readonly static string PathToMiscFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ConstructorTests", "Misc");

        [Test]
        public void UploadDataFromCsvFile()
        {
            var pathToTable = Path.Combine(PathToMiscFolder, "Data", "out.csv");
            var dataPrepare = new DataPrepare<ConstructedClass>();
            var data = dataPrepare.UploadDataTable(pathToTable, "|");
            data.Count.Should().Be(28);
        }
    }
}
