namespace FluentFuzzer.Constructors.StringCorpus
{
    internal class TestStringCorpus : IStringCorpuse
    {
        private readonly List<string> _words = new List<string>();

        public IReadOnlyList<string> GetCorpuse()
        {
            return _words.AsReadOnly();
        }

        public void Add(string testString)
        {
            _words.Add(testString);
        }
    }
}
