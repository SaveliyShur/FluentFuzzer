namespace FluentFuzzer.DataPreparation.Model
{
    public class DataModel<T> where T : class
    {
        public T DataObject { get; set; }

        public int? ClassLabel { get; set; }
    }
}
