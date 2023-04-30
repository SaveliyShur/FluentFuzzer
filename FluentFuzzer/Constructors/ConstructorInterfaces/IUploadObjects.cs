using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentFuzzer.Constructors.ConstructorInterfaces
{
    public interface IUploadObjects<Model> where Model : class
    {
        void Upload(List<Model> list);
        void UploadWithStringsChangedOnSectionTitles(List<Model> list);
        int Count();
    }
}
