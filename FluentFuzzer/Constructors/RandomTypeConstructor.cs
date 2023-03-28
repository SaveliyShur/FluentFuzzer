#pragma warning disable CS8629 // Nullable value type may be null.
#pragma warning disable CS8605 // Unboxing a possibly null value.

using FuzzerRunner.Constructors;
using FuzzerRunner.Utils;

namespace FuzzerRunner
{
    public class RandomTypeConstructor : BaseConstructor
    {
        private static readonly object _lockRandomObject = new ();
        private static readonly Random _random = new ();

        public override T Construct<T>()
        {
            return (T)ConstructByType(typeof(T));
        }

        private object ConstructByType(Type type)
        {
            if (type == typeof(string))
            {
                return ConstructRandomString();
            }
            else if (type == typeof(int))
            {
                return ConstructRandomInt();
            }
            else if (type == typeof(short))
            {
                return ConstructRandomShort();
            }
            else if (type == typeof(long))
            {
                return ConstructRandomLong();
            }
            else if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                return ConstructRandomNullable(type);
            }
            else if (type.IsEnum)
            {
                return ConstructRandomEnum(type);
            }
            else if (type.IsInterface)
            {
                return ConstructInterface(type);
            }
            else if (type.IsClass)
            {
                return ConstructObject(type);
            }
            else
            {
                throw new ArgumentNullException("Type of T is not implemented.");
            }
        }

        private object ConstructObject(Type type)
        {
            var randomForNull = GetRundomInt(0, 100);
            if (randomForNull < 20)
            {
                return null;
            }

            var objConsruct = Activator.CreateInstance(type);

            if (type.IsList())
            {
                var capacity = GetCapacity();
                var innerType = type.GetCollectionElementType();
                for (var i = 0; i < capacity; i++)
                {
                    var value = ConstructByType(innerType);
                    type.GetMethod("Add").Invoke(objConsruct, new object[] { value });
                }
            }
            else if (type.IsDictionary())
            {
                var capacity = GetCapacity();
                var innerType = type.GetDictionaryElementTypes();
                for (var i = 0; i < capacity; i++)
                {
                    var key = ConstructByType(innerType.key);
                    var value = ConstructByType(innerType.value);

                    if ((bool)type.GetMethod("ContainsKey").Invoke(objConsruct, new object[] {key}))
                    {
                        var method = type.GetMethods().Where(m => m.Name == "Remove").First(m => m.GetParameters().Length == 1);
                        method.Invoke(objConsruct, new object[] { key });
                    }

                    type.GetMethod("Add").Invoke(objConsruct, new object[] { key, value });
                }
            }
            else
            {
                var properties = type.GetProperties();
                foreach (var property in properties)
                {
                    if (property.CanWrite)
                    {
                        var value = ConstructByType(property.PropertyType);
                        property.SetValue(objConsruct, value);
                    }
                }
            }

            return objConsruct;
        }

        private string ConstructRandomString()
        {
            return GetRandomString();
        }

        private object ConstructRandomNullable(Type type)
        {
            var randomForNull = GetRundomInt(0, 100);
            if (randomForNull < 20)
            {
                return null;
            }

            var innerType = type.GenericTypeArguments[0];
            return ConstructByType(innerType);
        }

        private object ConstructRandomEnum(Type type)
        {
            var values = Enum.GetValues(type);
            var enumNumber = GetRundomInt(0, values.Length);

            return values.GetValue(enumNumber);
        }

        private object ConstructInterface(Type type)
        {
            if (type.Name == typeof(IDictionary<,>).Name ||
                type.Name == typeof(IReadOnlyDictionary<,>).Name)
            {
                var firstInnerType = type.GetGenericArguments()[0];
                var secondInnerType = type.GetGenericArguments()[1];
                var newType = typeof(Dictionary<,>).MakeGenericType(firstInnerType, secondInnerType);
                return ConstructObject(newType);
            }

            if (type.Name == typeof(IList<>).Name ||
                type.Name == typeof(ICollection<>).Name ||
                type.Name == typeof(IEnumerable<>).Name ||
                type.Name == typeof(IReadOnlyCollection<>).Name ||
                type.Name == typeof(IReadOnlyList<>).Name)
            {
                var innerType = type.GetGenericArguments()[0];
                var newType = typeof(List<>).MakeGenericType(innerType);
                return ConstructObject(newType);
            }

            throw new IndexOutOfRangeException($"Class for interface {type.Name} not found.");
        }

        private int ConstructRandomInt()
        {
            var checker = GetRundomInt(0, 8);
            return checker switch
            {
                0 => 0,
                1 => 1,
                2 => 2,
                3 => -1,
                4 => -2,
                5 => int.MinValue,
                6 => int.MaxValue,
                7 => GetRundomInt(),
                _ => throw new InvalidOperationException("ConstructRandomInt error."),
            };
        }

        private short ConstructRandomShort()
        {
            var checker = GetRundomInt(0, 8);
            return checker switch
            {
                0 => 0,
                1 => 1,
                2 => 2,
                3 => -1,
                4 => -2,
                5 => short.MinValue,
                6 => short.MaxValue,
                7 => (short)GetRundomInt(short.MinValue, short.MaxValue),
                _ => throw new InvalidOperationException("ConstructRandomShort error."),
            };
        }

        private long ConstructRandomLong()
        {
            var checker = GetRundomInt(0, 8);
            return checker switch
            {
                0 => 0L,
                1 => 1L,
                2 => 2L,
                3 => -1L,
                4 => -2L,
                5 => long.MinValue,
                6 => long.MaxValue,
                7 => GetRandomLong(),
                _ => throw new InvalidOperationException("ConstructRandomLong error."),
            };
        }

        private int GetCapacity()
        {
            var checker = GetRundomInt(0, 10000);

            if (checker < 3330)
                return 0;
            else if (checker < 6660)
                return 1;
            else if (checker < 9990)
                return 2;
            else if (checker < 9995)
                return 3;
            else 
                return 10;
        }

        private int GetRundomInt(int? start = null, int? end = null)
        {
            lock(_lockRandomObject)
            {
                if (start is null && end is null)
                    return _random.Next();
                else if (start is null)
                    return _random.Next(end.Value);
                else if (end is null)
                    return _random.Next(start.Value, int.MaxValue);

                return _random.Next(start.Value, end.Value);
            }
        }

        private long GetRandomLong()
        {
            lock (_lockRandomObject)
            {
                return _random.NextInt64();
            }
        }
    }
}
