using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentFuzzer.Constructors.ConstructorInterfaces
{
    public interface IUploadObjects<Model>
    {
        void Add(Model model);
        void Upload(List<Model> list);
        void UploadWithSectionTitles(List<Model> list);
        void UploadSectionTitlesToManyString(List<Model> list);
        int Count();
    }
}
