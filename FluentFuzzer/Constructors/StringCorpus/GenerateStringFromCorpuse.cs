namespace FluentFuzzer.Constructors.StringCorpus
{
    internal class GenerateStringFromCorpuse
    {
        private static readonly object _locker = new();
        private static readonly Random _random = new();

        private IReadOnlyList<string> _standartStringCorpus = new List<string>();
        private IReadOnlyList<string> _userStringCorpus = new List<string>();
        private IReadOnlyList<string> _testStringCorpus = new List<string>();

        public void AddStandartCorpuse(IStringCorpuse corpuse)
        {
            _standartStringCorpus = corpuse.GetCorpuse();
        }

        public void AddUserStringsCorpuse(IStringCorpuse corpuse)
        {
            _userStringCorpus = corpuse.GetCorpuse();
        }

        public void AddTestStringCorpuse(IStringCorpuse corpuse)
        {
            _testStringCorpus = corpuse.GetCorpuse();
        }

        public string GenerateRandomString()
        {
            var countAllStrings = _standartStringCorpus.Count + _userStringCorpus.Count + _testStringCorpus.Count;
            var randomCount = GetRandomInt(countAllStrings);

            if (randomCount < _standartStringCorpus.Count)
                return _standartStringCorpus[randomCount];

            randomCount = randomCount - _standartStringCorpus.Count;

            if (randomCount < _userStringCorpus.Count)
                return _userStringCorpus[randomCount];

            randomCount = randomCount - _userStringCorpus.Count;

            return _testStringCorpus[randomCount];
        }

        private static int GetRandomInt(int end)
        {
            lock (_locker)
            {
                return _random.Next(end);
            }
        }
    }
}
