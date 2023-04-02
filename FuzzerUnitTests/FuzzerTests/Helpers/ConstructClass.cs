using System.Collections.Generic;
using System.Linq;

namespace FuzzerUnitTests.FuzzerTests.Helpers
{
    public class ConstructClass
    {
        public string Name { get; set; }

        public List<string> NameChildren { get; set; }

        public int Age { get; set; }

        public override bool Equals(object? obj)
        {
            var item = obj as ConstructClass;

            if (item == null)
            {
                return false;
            }

            return (Name == item.Name) 
                && (Age == item.Age)
                && ((NameChildren is null && item.NameChildren is null) || (NameChildren.All(n => item.NameChildren.Contains(n)) && NameChildren.Count == item.NameChildren.Count));
        }
    }
}
