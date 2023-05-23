using FluentFuzzer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentFuzzer.Constructors.ConstructorInterfaces
{
    public interface IMutation
    {
        void SetCountMutation(int count);
        void SetAllowedMutation(List<MutationEnum> allowedMutation);
    }
}
