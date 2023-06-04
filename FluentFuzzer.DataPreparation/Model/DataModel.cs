namespace FluentFuzzer.DataPreparation.Model
{
    public class DataModel<T>  : DataModelWithoutClassLabel<T> where T : class
    {
        public int? ClassLabel { get; set; }
    }
}
