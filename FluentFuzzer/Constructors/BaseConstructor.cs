using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuzzerRunner.Constructors
{
    public abstract class BaseConstructor : IConstructor
    {
        private bool _isUsedStandartStringCorpus = true;
        private bool _isUsedUserStringCorpus = false;
        private static bool _isUsedUserStaticStringCorpus = false;

        public abstract T Construct<T>();

        public void UseStandartStringCorpus(bool isUsed = true)
        {
            _isUsedStandartStringCorpus = isUsed;
        }

        public void UseStringCorpus(string folderPath)
        {
            throw new NotImplementedException();
        }

        public Task UseStringCorpusAsync(string folderPath)
        {
            throw new NotImplementedException();
        }

        protected string GetRandomString()
        {
            if (_isUsedStandartStringCorpus)
            {
                return StandartStringCorpus.GetRandomString();
            }
            else
            {
                throw new ApplicationException("String corpus isnt defined");
            }
        }
    }
}
