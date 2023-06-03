namespace FluentFuzzer.Constructors.StringCorpus
{
    internal class StaticStringCorpus : IStringCorpuse
    {
        private static IReadOnlyList<string>? _words = null;
        private static readonly Dictionary<string, List<string>> _corpuseByTitle = new();

        public IReadOnlyList<string> GetCorpuse()
        {
            if (_words is null)
                throw new NullReferenceException("User string corpus is not define");

            return _words;
        }

        public async Task ReadStringCorpusAsync(string folder)
        {
            ValidateFolder(folder);

            var files = Directory.GetFiles(folder, "", SearchOption.AllDirectories);
            var list = new List<string>();
            foreach (var file in files)
            {
                var lines = (await File.ReadAllLinesAsync(file)).ToArray();

                AddLinesToDictionary(lines);

                list.AddRange(lines.Where(l => !l.StartsWith(IStringCorpuse.StandartTitleStart)));
            }

            _words = list.AsReadOnly();
        }

        public void ReadStringCorpus(string folder)
        {
            ValidateFolder(folder);

            var files = Directory.GetFiles(folder, "", SearchOption.AllDirectories);
            var list = new List<string>();
            foreach (var file in files)
            {
                var lines = File.ReadLines(file).ToArray();

                AddLinesToDictionary(lines);

                list.AddRange(lines.Where(l => !l.StartsWith(IStringCorpuse.StandartTitleStart)));
            }

            _words = list.AsReadOnly();
        }

        private void AddLinesToDictionary(string[] lines)
        {
            var corpusInDictionary = new List<string>();

            foreach (var line in lines)
            {
                if (line.StartsWith(IStringCorpuse.StandartTitleStart))
                {
                    var title = line.Replace(IStringCorpuse.StandartTitleStart, "").Trim();

                    if (_corpuseByTitle.ContainsKey(title))
                    {
                        corpusInDictionary = _corpuseByTitle[title];
                    }
                    else
                    {
                        corpusInDictionary = new List<string>();
                        _corpuseByTitle.Add(title, corpusInDictionary);
                    }
                }
                else
                {
                    corpusInDictionary.Add(line);
                }
            }
        }

        private void ValidateFolder(string folder)
        {
            if (folder == null)
                throw new ArgumentNullException(nameof(folder));

            if (!Directory.Exists(folder))
                throw new ApplicationException("UserString corpus not found. System error.");
            if (Directory.GetFiles(folder, "", SearchOption.AllDirectories).Length == 0)
                throw new ApplicationException("UserString corpus not found. System error.");
            if (Directory.GetFiles(folder, "", SearchOption.AllDirectories).Where(f => f.EndsWith(".txt")).Count() == 0)
                throw new ApplicationException("Files string corpus should end on '.txt' . System error.");
        }

        public IReadOnlyList<string> GetStringFromBlocksByTitle(string title)
        {
            if (!_corpuseByTitle.ContainsKey(title))
                throw new ApplicationException("Title not found in corpus.");

            return _corpuseByTitle[title];
        }

        public string? GetTitleByStringOrNull(string str)
        {
            foreach (var title in _corpuseByTitle.Keys)
            {
                if (_corpuseByTitle[title].Contains(str))
                {
                    return title;
                }
            }

            return null;
        }

        public IReadOnlyList<string> GetTitles()
        {
            return _corpuseByTitle.Keys.ToList();
        }
    }
}
