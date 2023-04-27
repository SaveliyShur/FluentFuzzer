using FluentFuzzer.Constructors.StringCorpus;

namespace FuzzerRunner.Constructors.StringCorpus
{
    internal class StandartStringCorpus : IStringCorpuse
    {
        private static readonly string _pathToFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Misc");
        private static readonly List<string> _corpuse = new();
        private static readonly Dictionary<string, List<string>> _corpuseByTitle = new();

        static StandartStringCorpus()
        {
            if (!Directory.Exists(_pathToFolder))
                throw new ApplicationException("Standart corpus not found. System error.");
            if (Directory.GetFiles(_pathToFolder, "", SearchOption.AllDirectories).Length == 0)
                throw new ApplicationException("Standart corpus not found. System error.");

            var files = Directory.GetFiles(_pathToFolder, "", SearchOption.AllDirectories);

            var startTitle = "Start";
            _corpuseByTitle.Add(startTitle, new List<string>());

            foreach (var file in files)
            {
                var lines = File.ReadLines(file).ToArray();
                foreach (var line in lines)
                {
                    if (line.StartsWith(IStringCorpuse.StandartTitleStart))
                    {
                        var title = line.Replace(IStringCorpuse.StandartTitleStart, "").Trim();
                        if (!_corpuseByTitle.ContainsKey(title))
                        {
                            _corpuseByTitle.Add(title, new List<string>());
                            startTitle = title;
                        }
                    }
                    else
                    {
                        _corpuseByTitle[startTitle].Add(line);
                    }
                }

                _corpuse.AddRange(lines.Where(l => !l.StartsWith(IStringCorpuse.StandartTitleStart)));
            }
        }

        public IReadOnlyList<string> GetCorpuse()
        {
            return _corpuse;
        }

        public IReadOnlyList<string> GetStringFromBlocksByTitle(string title)
        {
            if (!_corpuseByTitle.ContainsKey(title))
                throw new ApplicationException("Title not found in corpus.");

            return _corpuseByTitle[title];
        }

        public string? GetTitleByStringOrNull(string str)
        {
            foreach(var title in _corpuseByTitle.Keys)
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
