namespace FluentFuzzer.Constructors.StringCorpus
{
    internal interface IStringCorpuse
    {
        const string CommentStart = "# ";

        IReadOnlyList<string> GetCorpuse();
    }
}
