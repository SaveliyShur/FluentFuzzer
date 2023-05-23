using Newtonsoft.Json;

namespace FluentFuzzer.Utils
{
    internal static class ExtensionMethods
    {
        public static T DeepClone<T>(this T a)
        {
            var serialize = JsonConvert.SerializeObject(a);

            var deserialize = JsonConvert.DeserializeObject<T>(serialize);

            return deserialize;
        }
    }
}
