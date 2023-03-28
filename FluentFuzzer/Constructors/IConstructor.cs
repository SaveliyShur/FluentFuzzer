using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuzzerRunner
{
    public interface IConstructor
    {
        T Construct<T>();

        Task UseStringCorpusAsync(string folderPath);

        void UseStringCorpus(string folderPath);

        void UseStandartStringCorpus(bool isUsed = true);
    }
}
