namespace FluentFuzzer.Constructors.StringCorpus
{
    internal class TestStringCorpus : IStringCorpuse
    {
        private const string TITLE = "TestStringCorpuse";
        private readonly List<string> _words = new();

        public IReadOnlyList<string> GetCorpuse()
        {
            return _words.AsReadOnly();
        }

        public void Add(string testString)
        {
            _words.Add(testString);
        }

        public IReadOnlyList<string> GetTitles()
        {
            return new List<string>() { TITLE };
        }

        public IReadOnlyList<string> GetStringFromBlocksByTitle(string title)
        {
            if (title == TITLE)
            {
                return _words;
            }

            throw new ArgumentException($"Title is not {TITLE}");
        }

        public string? GetTitleByStringOrNull(string str)
        {
            if (_words.Contains(str))
            {
                return TITLE;
            }

            return null;
        }
    }
}
