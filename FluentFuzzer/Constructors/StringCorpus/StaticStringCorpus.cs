namespace FluentFuzzer.Constructors.StringCorpus
{
    internal class StaticStringCorpus : IStringCorpuse
    {
        private static IReadOnlyList<string>? _words = null;

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
                var lines = (await File.ReadAllLinesAsync(file))
                    .Where(l => !l.StartsWith(IStringCorpuse.CommentStart))
                    .Where(l => !string.IsNullOrEmpty(l))
                    .ToArray();

                if (lines.Any(l => string.IsNullOrEmpty(l)))
                {
                    lines.Where(l => !string.IsNullOrEmpty(l)).ToArray();
                    list.Add("");
                }

                list.AddRange(lines);
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
                var lines = File.ReadLines(file)
                    .Where(l => !l.StartsWith(IStringCorpuse.CommentStart))
                    .Where(l => !string.IsNullOrEmpty(l))
                    .ToArray();

                if (lines.Any(l => string.IsNullOrEmpty(l)))
                {
                    lines.Where(l => !string.IsNullOrEmpty(l)).ToArray();
                    list.Add("");
                }

                list.AddRange(lines);
            }

            _words = list.AsReadOnly();
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
    }
}
