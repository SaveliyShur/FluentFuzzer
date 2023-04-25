using CsvHelper;
using CsvHelper.Configuration;
using FluentFuzzer.DataPreparation.Model;
using Newtonsoft.Json;
using System.Globalization;

namespace FluentFuzzer.DataPreparation
{
    public class DataPrepare<T> : IDataPrepare<T> where T : class
    {
        public const string CSV_DELIMITER = ";;;";
        public const string END_OBJECT_FILE_NAME = "object.log";
        public const string END_CLASS_LABEL_FILE_NAME = "object.log.cls";

        public static readonly string LINE_SEPARATOR = Environment.NewLine;

        private static readonly CsvConfiguration CsvConfiguration = new(CultureInfo.InvariantCulture)
        {
            Delimiter = CSV_DELIMITER,
            NewLine = LINE_SEPARATOR,
        };

        private static string GeneratePathToCsvFile(string folder) => Path.Combine(folder, "DataCompile_" + Guid.NewGuid().ToString("N") + ".csv");

        public List<DataModel<T>> Data { get; private set; }

        public async Task<string> PrepareDataToDataTableAsync(string folder)
        {
            if (!Directory.Exists(folder))
                throw new ApplicationException("PrepareDataToDataTableAsync Folder not found. System error.");

            if (Data is null || Data.Count == 0)
                throw new ApplicationException($"Data not uploaded. Use UploadDataAsync.");

            var pathToCsvFile = GeneratePathToCsvFile(folder);
            using (var writer = new StreamWriter(pathToCsvFile))
            using (var csv = new CsvWriter(writer, CsvConfiguration))
            {
                await csv.WriteRecordsAsync(Data);
            }

            return pathToCsvFile;
        }

        public async Task<int> UploadDataAsync(string folder)
        {
            if (!Directory.Exists(folder))
                throw new ApplicationException("UploadDataAsync Folder not found. System error.");

            var files = Directory.GetFiles(folder, "", SearchOption.TopDirectoryOnly);
            var objectFiles = files.Where(f => f.EndsWith(END_OBJECT_FILE_NAME)).ToList();
            var classesFiles = files.Where(f => f.EndsWith(END_CLASS_LABEL_FILE_NAME)).ToList();

            if (!objectFiles.Any())
                throw new ApplicationException($"Object files not found. Object files should be end on {END_OBJECT_FILE_NAME}");

            if (!classesFiles.Any())
                throw new ApplicationException($"Class label files not found. Class label files should be end on {END_CLASS_LABEL_FILE_NAME}");

            var objects = new List<DataModel<T>>(objectFiles.Count());

            foreach (var file in objectFiles)
            {
                var fileObject = await File.ReadAllTextAsync(file);
                var deserializeObj = JsonConvert.DeserializeObject<T>(fileObject);

                var clsFile = classesFiles.FirstOrDefault(c => c.StartsWith(file));

                if (clsFile is null)
                    throw new ApplicationException($"Class label file not found. Object file is {file}");

                var clsString = await File.ReadAllTextAsync(clsFile);

                var isParceClassLabel = int.TryParse(clsString, out var cls);

                if (!isParceClassLabel)
                    throw new ApplicationException($"Class label should be int. Class label file is {clsFile}");

                objects.Add(new DataModel<T>()
                {
                    DataObject = deserializeObj,
                    ClassLabel = cls,
                });
            }

            Data = objects;

            return objects.Count;
        }

        public async Task<List<T>> UploadDataTableAsync(string pathToTable)
        {
            if (!File.Exists(pathToTable))
                throw new ApplicationException("UploadDataTableAsync File not found. System error.");

            return await Task.Run(() =>
            {
                using var reader = new StreamReader(pathToTable);
                using var csv = new CsvReader(reader, CsvConfiguration);
                var records = csv.GetRecords<DataModel<T>>().ToList();

                return records.Select(record => record.DataObject).ToList();
            });
        }
    }
}
