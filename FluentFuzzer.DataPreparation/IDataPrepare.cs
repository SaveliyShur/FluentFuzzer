﻿namespace FluentFuzzer.DataPreparation
{
    public interface IDataPrepare<T> where T : class
    {
        Task<int> UploadDataAsync(string folder, bool isAddClassLabels = true);
        List<T> UploadDataTable(string pathToTable, string? separator = null, bool isAddClassLabels = true);
        Task<string> PrepareDataToDataTableAsync(string folder, bool isAddClassLabels = true);
    }
}
