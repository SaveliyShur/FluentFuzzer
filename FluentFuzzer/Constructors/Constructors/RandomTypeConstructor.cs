#pragma warning disable CS8629 // Nullable value type may be null.
#pragma warning disable CS8605 // Unboxing a possibly null value.

using FluentFuzzer.Constructors;
using FluentFuzzer.Utils;
using FuzzerRunner.Constructors;
using FuzzerRunner.Utils;

namespace FuzzerRunner
{
    public class RandomTypeConstructor : BaseConstructor, ITuningConstructor
    {
        private static readonly object _lockRandomObject = new ();
        private static readonly Random _random = new ();
        private int? _maxStringLenght = null;
        private bool _notNullMainObject = false;

        public void SetMaxStringLenght(int lenght)
        {
            _maxStringLenght = lenght;
        }

        public void SetNotNullMainObject()
        {
            _notNullMainObject = true;
        }

        public override ConstructorEnum GetConstructorEnum() => ConstructorEnum.Random;

        public override T Construct<T>()
        {
            var obj = (T)ConstructByType(typeof(T));

            if (_notNullMainObject)
            {
                while (obj is null)
                {
                    obj = (T)ConstructByType(typeof(T));
                }
            }

            return obj;
        }

        internal object ConstructByType(Type type)
        {
            if (type == typeof(string))
            {
                return ConstructRandomString();
            }
            else if (type == typeof(DateTimeOffset))
            {
                return ConstructRandomDatetimeOffset();
            }
            else if (type == typeof(DateTime))
            {
                return ConstructRandomDatetimeOffset().DateTime;
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

        internal object ConstructObject(Type type)
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

        internal string ConstructRandomString()
        {
            var str = GetRandomString();

            for (var i = 0; i < 100; i++)
            {
                if (_maxStringLenght is not null && str?.Length > _maxStringLenght)
                    str = GetRandomString();
                else
                    return str;
            }

            return "";
        }

        internal object ConstructRandomNullable(Type type)
        {
            var randomForNull = GetRundomInt(0, 100);
            if (randomForNull < 20)
            {
                return null;
            }

            var innerType = type.GenericTypeArguments[0];
            return ConstructByType(innerType);
        }

        internal object ConstructRandomEnum(Type type)
        {
            var values = Enum.GetValues(type);
            var enumNumber = GetRundomInt(0, values.Length);

            return values.GetValue(enumNumber);
        }

        internal object ConstructInterface(Type type)
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

        internal int ConstructRandomInt()
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

        internal short ConstructRandomShort()
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

        internal long ConstructRandomLong()
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

        internal DateTimeOffset ConstructRandomDatetimeOffset()
        {
            var year = GetRundomInt(1, 9998);
            var mounth = GetRundomInt(1, 12);
            var day = GetRundomInt(1, 28);
            var hour = GetRundomInt(0, 23);
            var minute = GetRundomInt(0, 59);
            var second = GetRundomInt(0, 59);
            var dateTime = new DateTime(year, mounth, day, hour, minute, second);
            return new DateTimeOffset(dateTime);
        }

        internal int GetCapacity()
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

        internal int GetRundomInt(int? start = null, int? end = null)
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

        internal long GetRandomLong()
        {
            lock (_lockRandomObject)
            {
                return _random.NextInt64();
            }
        }
    }
}
