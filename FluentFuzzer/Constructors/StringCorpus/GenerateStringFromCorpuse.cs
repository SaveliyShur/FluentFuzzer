using FluentFuzzer.Constructors.ConstructorExceptions;

namespace FluentFuzzer.Constructors.StringCorpus
{
    internal class GenerateStringFromCorpuse
    {
        private static readonly object _locker = new();
        private static readonly Random _random = new();

        private IStringCorpuse? _standartStringCorpus = null;
        private IStringCorpuse? _userStringCorpus = null;
        private IStringCorpuse? _testStringCorpus = null;

        public void AddStandartCorpuse(IStringCorpuse corpuse)
        {
            _standartStringCorpus = corpuse;
        }

        public void AddUserStringsCorpuse(IStringCorpuse corpuse)
        {
            _userStringCorpus = corpuse;
        }

        public void AddTestStringCorpuse(IStringCorpuse corpuse)
        {
            _testStringCorpus = corpuse;
        }

        public string GenerateRandomString()
        {
            var standartStringTitles = _standartStringCorpus?.GetTitles() ?? new List<string>();
            var userStringTitles = _userStringCorpus?.GetTitles() ?? new List<string>();
            var testStringTitles = _testStringCorpus?.GetTitles() ?? new List<string>();

            var countAllStrings = standartStringTitles.Count + userStringTitles.Count + testStringTitles.Count;

            if (countAllStrings == 0)
                throw new ConstructException("Any corpus not found.");

            var randomCount = GetRandomInt(countAllStrings);

            if (randomCount < standartStringTitles.Count)
                return this.GetStringBySectionTitle(standartStringTitles[randomCount]);

            randomCount = randomCount - standartStringTitles.Count;

            if (randomCount < userStringTitles.Count)
                return this.GetStringBySectionTitle(userStringTitles[randomCount]);

            randomCount = randomCount - userStringTitles.Count;

            var title = testStringTitles[randomCount];

            return this.GetStringBySectionTitle(title);
        }

        public string? GenerateRandomGuidStringFromStringCorpuse()
        {
            var standartStringCorpusList = _standartStringCorpus?.GetCorpuse() ?? new List<string>();
            var userStringCorpusList = _userStringCorpus?.GetCorpuse() ?? new List<string>();
            var testStringCorpusList = _testStringCorpus?.GetCorpuse() ?? new List<string>();

            var listGuid = new List<string>();

            foreach (var str in standartStringCorpusList)
            {
                if (Guid.TryParse(str, out var _))
                {
                    listGuid.Add(str);
                }
            }

            foreach (var str in userStringCorpusList)
            {
                if (Guid.TryParse(str, out var _))
                {
                    listGuid.Add(str);
                }
            }

            foreach (var str in testStringCorpusList)
            {
                if (Guid.TryParse(str, out var _))
                {
                    listGuid.Add(str);
                }
            }

            if (listGuid.Count == 0)
                return null;

            var randomInt = GetRandomInt(listGuid.Count);
            return listGuid[randomInt];
        }

        public string GetSectionTitleByString(string str)
        {
            var title = _standartStringCorpus?.GetTitleByStringOrNull(str)
                ?? _userStringCorpus?.GetTitleByStringOrNull(str) 
                ?? _testStringCorpus?.GetTitleByStringOrNull(str);

            return title ?? throw new Exception("Section not found.");
        }

        public string GetStringBySectionTitle(string title)
        {
            if (_standartStringCorpus is not null && _standartStringCorpus.GetTitles().Contains(title))
            {
                var miniCorpus = _standartStringCorpus.GetStringFromBlocksByTitle(title);
                var random = GetRandomInt(miniCorpus.Count);
                return miniCorpus[random];
            }

            if (_userStringCorpus is not null && _userStringCorpus.GetTitles().Contains(title))
            {
                var miniCorpus = _userStringCorpus.GetStringFromBlocksByTitle(title);
                var random = GetRandomInt(miniCorpus.Count);
                return miniCorpus[random];
            }

            if (_testStringCorpus is not null && _testStringCorpus.GetTitles().Contains(title))
            {
                var miniCorpus = _testStringCorpus.GetStringFromBlocksByTitle(title);
                var random = GetRandomInt(miniCorpus.Count);
                return miniCorpus[random];
            }

            throw new Exception($"Title {title} not found in string corpuses");
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
