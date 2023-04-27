namespace FluentFuzzer.Constructors.StringCorpus
{
    internal interface IStringCorpuse
    {
        public const string StandartTitleStart = "#	";

        IReadOnlyList<string> GetCorpuse();

        IReadOnlyList<string> GetTitles();

        IReadOnlyList<string> GetStringFromBlocksByTitle(string title);

        string? GetTitleByStringOrNull(string str);
    }
}
