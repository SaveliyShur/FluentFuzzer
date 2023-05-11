using CsvHelper;
using CsvHelper.Configuration;
using FluentFuzzer.DataPreparation.Model;
using Newtonsoft.Json;
using System.Globalization;

namespace FluentFuzzer.DataPreparation
{
    public class DataPrepare<T> : IDataPrepare<T> where T : class
    {
        public const string CSV_DELIMITER = "column_separator";
        public const string END_OBJECT_FILE_NAME = "object.log";
        public const string END_CLASS_LABEL_FILE_NAME = "object.log.cls";

        public static readonly string LINE_SEPARATOR = Environment.NewLine;

        private static string GeneratePathToCsvFile(string folder) => Path.Combine(folder, "DataCompile_" + Guid.NewGuid().ToString("N") + ".csv");

        public List<DataModel<T>> Data { get; private set; }

        public async Task<string> PrepareDataToDataTableAsync(string folder)
        {
            if (!Directory.Exists(folder))
                throw new ApplicationException("PrepareDataToDataTableAsync Folder not found. System error.");

            if (Data is null || Data.Count == 0)
                throw new ApplicationException($"Data not uploaded. Use UploadDataAsync.");

            var pathToCsvFile = GeneratePathToCsvFile(folder);
            var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = CSV_DELIMITER,
            };

            using (var writer = new StreamWriter(pathToCsvFile))
            using (var csv = new CsvWriter(writer, csvConfig))
            {
                await csv.WriteRecordsAsync(Data);
            }

            return pathToCsvFile;
        }

        public async Task<int> UploadDataAsync(string folder, bool isAddClassLabels = true)
        {
            if (!Directory.Exists(folder))
                throw new ApplicationException("UploadDataAsync Folder not found. System error.");

            var files = Directory.GetFiles(folder, "", SearchOption.TopDirectoryOnly);
            var objectFiles = files.Where(f => f.EndsWith(END_OBJECT_FILE_NAME)).ToList();
            var classesFiles = files.Where(f => f.EndsWith(END_CLASS_LABEL_FILE_NAME)).ToList();

            if (!objectFiles.Any())
                throw new ApplicationException($"Object files not found. Object files should be end on {END_OBJECT_FILE_NAME}");

            if (isAddClassLabels && !classesFiles.Any())
                throw new ApplicationException($"Class label files not found. Class label files should be end on {END_CLASS_LABEL_FILE_NAME}");

            var objects = new List<DataModel<T>>(objectFiles.Count());

            foreach (var file in objectFiles)
            {
                var fileObject = await File.ReadAllTextAsync(file);
                var deserializeObj = JsonConvert.DeserializeObject<T>(fileObject);
                int? cls = null;

                if (isAddClassLabels)
                {
                    var clsFile = classesFiles.FirstOrDefault(c => c.StartsWith(file));

                    if (clsFile is null)
                        throw new ApplicationException($"Class label file not found. Object file is {file}");

                    var clsString = await File.ReadAllTextAsync(clsFile);

                    var isParceClassLabel = int.TryParse(clsString, out var clsIs);

                    if (!isParceClassLabel)
                        throw new ApplicationException($"Class label should be int. Class label file is {clsFile}");

                    cls = clsIs;
                }

                objects.Add(new DataModel<T>()
                {
                    DataObject = deserializeObj,
                    ClassLabel = cls,
                });
            }

            Data = objects;

            return objects.Count;
        }

        public List<T> UploadDataTable(string pathToTable, string? separator = null)
        {
            if (!File.Exists(pathToTable))
                throw new ApplicationException("UploadDataTableAsync File not found. System error.");

            var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = separator ?? CSV_DELIMITER,
            };

            using var reader = new StreamReader(pathToTable);
            using var csv = new CsvReader(reader, csvConfig);
            var records = csv.GetRecords<DataModel<T>>().ToList();

            return records.Select(record => record.DataObject).ToList();
        }
    }
}
