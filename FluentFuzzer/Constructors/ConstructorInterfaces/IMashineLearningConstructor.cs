namespace FluentFuzzer.Constructors.ConstructorInterfaces
{
    public interface IMashineLearningConstructor
    {
        void Structure<T>();

        Task DownloadFitDataAsync(string pathToFolder);

        Task FitAsync();

        Task UploadModelAsync(string pathToModel);

        Task DownloadModelAsync(string pathToFolder, string modelName = "model.ml");
    }
}
