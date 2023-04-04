using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentFuzzer.Constructors
{
    public interface ITuningConstructor
    {
        void SetMaxStringLenght(int lenght);

        void SetNotNullMainObject();
    }
}
