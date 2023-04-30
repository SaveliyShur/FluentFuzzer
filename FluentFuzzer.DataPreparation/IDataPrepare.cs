﻿namespace FluentFuzzer.DataPreparation
{
    public interface IDataPrepare<T> where T : class
    {
        Task<int> UploadDataAsync(string folder, bool isAddClassLabels = true);
        Task<List<T>> UploadDataTableAsync(string pathToTable);
        Task<string> PrepareDataToDataTableAsync(string folder);
    }
}