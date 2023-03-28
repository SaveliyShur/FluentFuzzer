namespace FuzzerRunner.Constructors
{
    internal class StandartStringCorpus
    {
        private const string _commentStart = "# ";
        private static readonly string _pathToFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Misc");
        private static readonly List<string> _corpuse = new();

        private static readonly object _locker = new ();
        private static readonly Random _random = new ();

        static StandartStringCorpus()
        {
            if (!Directory.Exists(_pathToFolder))
                throw new ApplicationException("Standart corpus not found. System error.");

            var files = Directory.GetFiles(_pathToFolder, "", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                var lines = File.ReadLines(file)
                    .Where(l => !l.StartsWith(_commentStart))
                    .Where(l => !string.IsNullOrEmpty(l))
                    .ToArray();
                _corpuse.AddRange(lines);
                _corpuse.Add(""); // Add empty line, becouse delete all empty on a privious step
            }
        }

        public static string GetRandomString()
        {
            return _corpuse[GetRandomInt(_corpuse.Count)];
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
