using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuzzerRunner.Utils
{
    internal static class ReflectionUtility
    {

        public static bool IsList(this Type type)
        {
            if (null == type)
                throw new ArgumentNullException("type");

            if (typeof(System.Collections.IList).IsAssignableFrom(type))
                return true;

            foreach (var it in type.GetInterfaces())
                if (it.IsGenericType && typeof(IList<>) == it.GetGenericTypeDefinition())
                    return true;

            return false;
        }

        public static bool IsDictionary(this Type type)
        {
            if (null == type)
                throw new ArgumentNullException("type");

            if (typeof(System.Collections.IList).IsAssignableFrom(type))
                return true;

            foreach (var it in type.GetInterfaces())
                if (it.IsGenericType && typeof(IDictionary<,>) == it.GetGenericTypeDefinition())
                    return true;

            return false;
        }

        public static (Type key, Type value) GetDictionaryElementTypes(this Type type)
        {
            if (null == type)
                throw new ArgumentNullException("type");

            var innerTypes = type.GetGenericArguments();

            if (innerTypes.Length != 2)
                throw new Exception("Dictionary has specific type");

            return (innerTypes[0], innerTypes[1]);
        }

        public static Type GetCollectionElementType(this Type type)
        {
            if (null == type)
                throw new ArgumentNullException("type");

            var etype = typeof(IEnumerable<>);
            foreach (var bt in type.GetInterfaces())
                if (bt.IsGenericType && bt.GetGenericTypeDefinition() == etype)
                    return bt.GetGenericArguments()[0];

            if (typeof(System.Collections.IDictionary).IsAssignableFrom(type))
                return typeof(System.Collections.DictionaryEntry);

            if (typeof(System.Collections.IList).IsAssignableFrom(type))
            {
                foreach (var prop in type.GetProperties())
                {
                    if ("Item" == prop.Name && typeof(object) != prop.PropertyType)
                    {
                        var ipa = prop.GetIndexParameters();
                        if (1 == ipa.Length && typeof(int) == ipa[0].ParameterType)
                        {
                            return prop.PropertyType;
                        }
                    }
                }
            }

            if (typeof(System.Collections.ICollection).IsAssignableFrom(type))
            {
                foreach (var meth in type.GetMethods())
                {
                    if ("Add" == meth.Name)
                    {
                        var pa = meth.GetParameters();
                        if (1 == pa.Length && typeof(object) != pa[0].ParameterType)
                            return pa[0].ParameterType;
                    }
                }
            }
            if (typeof(System.Collections.IEnumerable).IsAssignableFrom(type))
                return typeof(object);
            return null;
        }
    }
}
