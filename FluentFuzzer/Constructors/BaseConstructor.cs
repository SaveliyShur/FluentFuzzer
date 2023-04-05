using FluentFuzzer.Constructors.StringCorpus;
using FuzzerRunner.Constructors.StringCorpus;

namespace FuzzerRunner.Constructors
{
    public abstract class BaseConstructor : IConstructor
    {
        private bool _isUsedStandartStringCorpus = true;

        private static StaticStringCorpus? _staticStringCorpus = null;
        private UserStringCorpus? _userStringCorpus = null;
        private TestStringCorpus _testStringCorpus = new ();

        public abstract T Construct<T>();

        public void UseStandartStringCorpus(bool isUsed = true)
        {
            _isUsedStandartStringCorpus = isUsed;
        }

        public async Task UseStringCorpusAsync(string folderPath)
        {
            _userStringCorpus = new UserStringCorpus();
            await _userStringCorpus.ReadStringCorpusAsync(folderPath);
        }

        public void UseStringCorpus(string folderPath)
        {
            _userStringCorpus = new UserStringCorpus();
            _userStringCorpus.ReadStringCorpus(folderPath);
        }

        public async Task UseStaticStringCorpusAsync(string folderPath)
        {
            _staticStringCorpus = new StaticStringCorpus();
            await _staticStringCorpus.ReadStringCorpusAsync(folderPath);
        }

        public void UseStaticStringCorpus(string folderPath)
        {
            _staticStringCorpus = new StaticStringCorpus();
            _staticStringCorpus.ReadStringCorpus(folderPath);
        }

        public void AddStringToTestStringCorpus(string testString)
        {
            _testStringCorpus.Add(testString);
        }

        protected string GetRandomString()
        {
            var generator = new GenerateStringFromCorpuse();

            if (_isUsedStandartStringCorpus)
            {
                generator.AddStandartCorpuse(new StandartStringCorpus());
            }
            
            if (_staticStringCorpus is not null && _userStringCorpus is not null)
            {
                generator.AddUserStringsCorpuse(_userStringCorpus);
            }

            if (_staticStringCorpus is null && _userStringCorpus is not null)
            {
                generator.AddUserStringsCorpuse(_userStringCorpus);
            }

            if (_staticStringCorpus is not null && _userStringCorpus is null)
            {
                generator.AddUserStringsCorpuse(_staticStringCorpus);
            }

            generator.AddTestStringCorpuse(_testStringCorpus);

            return generator.GenerateRandomString();
        }
    }
}
