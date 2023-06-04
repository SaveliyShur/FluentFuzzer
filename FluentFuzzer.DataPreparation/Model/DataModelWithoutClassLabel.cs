using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentFuzzer.DataPreparation.Model
{
    public class DataModelWithoutClassLabel<T> where T : class
    {
        public T DataObject { get; set; }
    }
}
