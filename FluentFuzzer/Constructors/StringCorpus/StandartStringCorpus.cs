using FluentFuzzer.Constructors.StringCorpus;

namespace FuzzerRunner.Constructors.StringCorpus
{
    internal class StandartStringCorpus : IStringCorpuse
    {
        private static readonly string _pathToFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Misc");
        private static readonly List<string> _corpuse = new();

        static StandartStringCorpus()
        {
            if (!Directory.Exists(_pathToFolder))
                throw new ApplicationException("Standart corpus not found. System error.");
            if (Directory.GetFiles(_pathToFolder, "", SearchOption.AllDirectories).Count() == 0)
                throw new ApplicationException("Standart corpus not found. System error.");

            var files = Directory.GetFiles(_pathToFolder, "", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                var lines = File.ReadLines(file)
                    .Where(l => !l.StartsWith(IStringCorpuse.CommentStart))
                    .Where(l => !string.IsNullOrEmpty(l))
                    .ToArray();
                _corpuse.AddRange(lines);
                _corpuse.Add(""); // Add empty line, becouse delete all empty on a privious step
            }
        }

        public IReadOnlyList<string> GetCorpuse()
        {
            return _corpuse;
        }
    }
}
