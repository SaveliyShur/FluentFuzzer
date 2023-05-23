using FluentFuzzer.Constructors.ConstructorExceptions;
using FluentFuzzer.Constructors.ConstructorInterfaces;
using FluentFuzzer.Utils;
using FuzzerRunner;
using FuzzerRunner.Constructors;
using FuzzerRunner.Utils;
using System.Collections;

namespace FluentFuzzer.Constructors.Constructors
{
    public class MutationConstructor<Model> : BaseConstructor, IMutation, ITuningConstructor, IUploadObjects<Model>
    {
        private const int MAX_CHANGE_RETRY = 1000;

        private List<Model> _objects = new();
        private int _countMutation = 0;
        private List<MutationEnum> _allowedMutation = new() { MutationEnum.Delete, MutationEnum.Modify, MutationEnum.Add };
        private bool _notNullMainObject = false;

        private static readonly RandomTypeConstructor _randomTypeConstructor = new();

        public IReadOnlyList<MutationEnum> AllowedMutation => _allowedMutation.AsReadOnly();

        public IReadOnlyList<Model> Objects => _objects.AsReadOnly();

        public int CountMutation => _countMutation;

        public override ConstructorEnum GetConstructorEnum() => ConstructorEnum.Mutation;

        public override T Construct<T>()
        {
            if (typeof(T) != typeof(Model))
                throw new InvalidOperationException("Type of T not equals ModelType");

            var obj = (T)(object)_objects[new Random().Next(0, _objects.Count - 1)];

            var copyObject = obj.DeepClone();

            var mutations = new List<MutationEnum>(_countMutation);
            for (var i = 0; i < _countMutation; i++)
            {
                mutations.Add(_allowedMutation[new Random().Next(_allowedMutation.Count)]);
            }

            foreach (var mutate in mutations)
            {
                copyObject = (T)Mutate(copyObject, mutate);
            }

            return copyObject;
        }

        public void Upload(List<Model> list)
        {
            _objects = list.OrderBy(a => Guid.NewGuid()).ToList();
        }

        public void UploadWithSectionTitles(List<Model> list)
        {
            _objects = list
                .Select(l => (Model)ChangeAllSectionTitleToRandomString(l))
                .OrderBy(a => Guid.NewGuid())
                .ToList();
        }

        public void UploadSectionTitlesToManyString(List<Model> list)
        {
            _objects = list
                .Select(l => (Model)ChangeAllSectionTitleToRandomString(l))
                .OrderBy(a => Guid.NewGuid())
                .ToList();
        }

        public int Count()
        {
            return _objects.Count;
        }

        public void Add(Model model)
        {
            _objects.Add(model);
        }

        public void SetCountMutation(int count)
        {
            _countMutation = count;
        }

        public void SetAllowedMutation(List<MutationEnum> allowedMutation)
        {
            _allowedMutation = allowedMutation.Distinct().ToList();
        }

        public void SetMaxStringLenght(int lenght)
        {
            throw new NotImplementedException();
        }

        public void SetNotNullMainObject()
        {
            _notNullMainObject = true;
        }

        private object Mutate(object obj, MutationEnum mutation)
        {
            switch (mutation)
            {
                case MutationEnum.Delete:
                    return DeleteOneInnerObject(obj);
                case MutationEnum.Modify:
                    return ModifyOneInnerObject(obj);
                case MutationEnum.Add:
                    return AddOneInnerObject(obj);
                default:
                    throw new ArgumentException($"Mutation {mutation} not implemented");
            }
        }

        #region Delete
        private object DeleteOneInnerObject(object obj)
        {
            for (var i = 0; i < MAX_CHANGE_RETRY; i++)
            {
                var deleteInnerObject = TryDeleteInnerObject(obj);

                if (_notNullMainObject && deleteInnerObject.Obj is null)
                {
                    continue;
                }

                if (deleteInnerObject.IsDelete)
                {
                    return deleteInnerObject.Obj;
                }
            }

            return obj;
        }

        private (object? Obj, bool IsDelete) TryDeleteInnerObject(object innerObject)
        {
            if (innerObject == null)
                return (null, false);

            var type = innerObject.GetType();
            var defaultValue = type.IsValueType ? Activator.CreateInstance(type) : null;

            if (innerObject == defaultValue)
                return (innerObject, false);

            // Удалить полностью
            if (TryChange)
                return (defaultValue, true);

            // Удалить из листа
            if (type.IsList())
            {
                return TryDeleteFromList(innerObject);
            }

            if (type.IsDictionary())
            {
                return TryDeleteFromDictionary(innerObject);
            }

            var properties = innerObject.GetType().GetProperties();
            foreach (var property in properties)
            {
                if (!property.CanWrite)
                    continue;

                var value = property.GetValue(innerObject);
                var deleteInnerValue = TryDeleteInnerObject(value);

                if (deleteInnerValue.IsDelete)
                {
                    property.SetValue(innerObject, deleteInnerValue.Obj);

                    return (innerObject, true);
                }
            }

            return (innerObject, false);
        }

        private (object? Obj, bool IsDelete) TryDeleteFromList(object listObject)
        {
            var type = listObject.GetType();
            var innerType = type.GetCollectionElementType();

            var newValue = Activator.CreateInstance(type);
            var list = (IList)listObject;
            var isDeletedListValue = false;

            foreach (var item in list)
            {
                if (isDeletedListValue)
                {
                    type.GetMethod("Add").Invoke(newValue, new object[] { item });
                    continue;
                }

                if (TryChange)
                {
                    isDeletedListValue = true;
                }
                else
                {
                    if (innerType.IsClass && innerType != typeof(string))
                    {
                        var tryDeleteInnerObject = TryDeleteInnerObject(item);
                        if (tryDeleteInnerObject.IsDelete)
                        {
                            isDeletedListValue = true;
                        }

                        type.GetMethod("Add").Invoke(newValue, new object[] { tryDeleteInnerObject.Obj });
                    }
                    else
                    {
                        type.GetMethod("Add").Invoke(newValue, new object[] { item });
                    }
                }
            }

            if (isDeletedListValue)
            {
                return (newValue, true);
            }
            else
            {
                return (newValue, false);
            }
        }

        private (object? Obj, bool IsDelete) TryDeleteFromDictionary(object dictObject)
        {
            var type = dictObject.GetType();
            var innerType = type.GetDictionaryElementTypes();

            if (innerType.key != typeof(string))
            {
                throw new ConstructException("Key should be string in all dictionaries");
            }

            var newValue = Activator.CreateInstance(type);
            var list = (IDictionary)dictObject;
            var isDeletedListValue = false;

            foreach (var item in list)
            {
                var itemType = item.GetType();

                var key = itemType.GetProperty("Key").GetValue(item);
                var value = itemType.GetProperty("Value").GetValue(item);

                if (isDeletedListValue)
                {
                    type.GetMethod("Add").Invoke(newValue, new object[] { key, value });
                    continue;
                }

                if (TryChange)
                {
                    isDeletedListValue = true;
                    continue;
                }

                var tryDeleteInnerValue = TryDeleteInnerObject(value);
                if (tryDeleteInnerValue.IsDelete)
                {
                    type.GetMethod("Add").Invoke(newValue, new object[] { key, tryDeleteInnerValue.Obj });
                    isDeletedListValue = true;
                    continue;
                }

                type.GetMethod("Add").Invoke(newValue, new object[] { key, value });
            }

            if (isDeletedListValue)
            {
                return (newValue, true);
            }
            else
            {
                return (newValue, false);
            }
        }

        #endregion

        #region Modify
        private object ModifyOneInnerObject(object obj)
        {
            for (var i = 0; i < MAX_CHANGE_RETRY; i++)
            {
                var modifyObject = TryModifyInnerObject(obj);

                if (_notNullMainObject && modifyObject.Obj is null)
                {
                    continue;
                }

                if (modifyObject.IsModify)
                {
                    return modifyObject.Obj;
                }
            }

            return obj;
        }

        private (object? Obj, bool IsModify) TryModifyInnerObject(object innerObject)
        {
            if (innerObject == null)
                return (null, false);

            var type = innerObject.GetType();

            if (TryChange)
                return (_randomTypeConstructor.ConstructByType(type), true);

            if (type.IsList())
            {
                return TryModifyFromList(innerObject);
            }

            if (type.IsDictionary())
            {
                return TryModifyFromDictionary(innerObject);
            }

            var properties = innerObject.GetType().GetProperties();
            foreach (var property in properties)
            {
                if (!property.CanWrite)
                    continue;

                var value = property.GetValue(innerObject);
                var modifyInnerValue = TryModifyInnerObject(value);

                if (modifyInnerValue.IsModify)
                {
                    property.SetValue(innerObject, modifyInnerValue.Obj);

                    return (innerObject, true);
                }
            }

            return (innerObject, false);
        }

        private (object? Obj, bool IsModify) TryModifyFromList(object listObject)
        {
            var type = listObject.GetType();
            var innerType = type.GetCollectionElementType();

            var newValue = Activator.CreateInstance(type);
            var list = (IList)listObject;
            var isModifyedListValue = false;

            foreach (var item in list)
            {
                if (isModifyedListValue)
                {
                    type.GetMethod("Add").Invoke(newValue, new object[] { item });
                    continue;
                }

                if (innerType.IsClass)
                {
                    var tryModifyInnerObject = TryModifyInnerObject(item);
                    if (tryModifyInnerObject.IsModify)
                    {
                        isModifyedListValue = true;
                    }

                    type.GetMethod("Add").Invoke(newValue, new object[] { tryModifyInnerObject.Obj });
                }
                else
                {
                    type.GetMethod("Add").Invoke(newValue, new object[] { item });
                }
            }

            if (isModifyedListValue)
            {
                return (newValue, true);
            }
            else
            {
                return (newValue, false);
            }
        }

        private (object? Obj, bool IsModify) TryModifyFromDictionary(object dictObject)
        {
            var type = dictObject.GetType();
            var innerType = type.GetDictionaryElementTypes();

            var newValue = Activator.CreateInstance(type);
            var list = (IDictionary)dictObject;
            var isModifyedListValue = false;

            foreach (var item in list)
            {
                var itemType = item.GetType();

                var key = itemType.GetProperty("Key").GetValue(item);
                var value = itemType.GetProperty("Value").GetValue(item);

                if (isModifyedListValue)
                {
                    type.GetMethod("Add").Invoke(newValue, new object[] { key, value });
                    continue;
                }

                var tryModifyKey = TryModifyInnerObject(key);
                if (tryModifyKey.IsModify)
                {
                    if ((bool)type.GetMethod("ContainsKey").Invoke(newValue, new object[] { tryModifyKey.Obj }))
                    {
                        var method = type.GetMethods().Where(m => m.Name == "Remove").First(m => m.GetParameters().Length == 1);
                        method.Invoke(newValue, new object[] { tryModifyKey.Obj });
                    }

                    type.GetMethod("Add").Invoke(newValue, new object[] { tryModifyKey.Obj, value });
                    isModifyedListValue = true;
                    continue;
                }
                var tryModifyInnerValue = TryModifyInnerObject(value);
                if (tryModifyInnerValue.IsModify)
                {
                    if ((bool)type.GetMethod("ContainsKey").Invoke(newValue, new object[] { key }))
                    {
                        var method = type.GetMethods().Where(m => m.Name == "Remove").First(m => m.GetParameters().Length == 1);
                        method.Invoke(newValue, new object[] { key });
                    }

                    type.GetMethod("Add").Invoke(newValue, new object[] { key, tryModifyInnerValue.Obj });
                    isModifyedListValue = true;
                    continue;
                }

                type.GetMethod("Add").Invoke(newValue, new object[] { key, value });
            }

            if (isModifyedListValue)
            {
                return (newValue, true);
            }
            else
            {
                return (newValue, false);
            }
        }
        #endregion

        #region Add

        private object AddOneInnerObject(object obj)
        {
            for (var i = 0; i < MAX_CHANGE_RETRY; i++)
            {
                var changeInnerObject = TryAddInnerObject(obj, obj.GetType());

                if (_notNullMainObject && changeInnerObject.Obj is null)
                {
                    continue;
                }

                if (changeInnerObject.IsModify)
                {
                    return changeInnerObject.Obj;
                }
            }

            return obj;
        }

        private (object? Obj, bool IsModify) TryAddInnerObject(object innerObject, Type typeInnerObject)
        {
            if (innerObject == null)
            {
                if (TryChange)
                    return (_randomTypeConstructor.ConstructByType(typeInnerObject), true);
                else
                    return (innerObject, false);
            }

            if (typeInnerObject == typeof(string))
            {
                if (innerObject == default(string))
                {
                    if (TryChange)
                        return (_randomTypeConstructor.ConstructByType(typeInnerObject), true);
                    else
                        return (innerObject, false);
                }
                else
                {
                    return (innerObject, false);
                }
                
            }

            var defaultValue = Activator.CreateInstance(typeInnerObject);
            if (Equals(defaultValue, innerObject))
            {
                if (TryChange)
                    return (_randomTypeConstructor.ConstructByType(typeInnerObject), true);
                else
                    return (innerObject, false);
            }

            if (typeInnerObject.IsList())
            {
                return TryAddToList(innerObject);
            }

            if (typeInnerObject.IsDictionary())
            {
                return TryAddToDictionary(innerObject);
            }

            var properties = innerObject.GetType().GetProperties();
            foreach (var property in properties)
            {
                if (!property.CanWrite)
                    continue;

                var value = property.GetValue(innerObject);
                var addObjectIfNotSetted = TryAddInnerObject(value, property.PropertyType);

                if (addObjectIfNotSetted.IsModify)
                {
                    property.SetValue(innerObject, addObjectIfNotSetted.Obj);

                    return (innerObject, true);
                }
            }

            return (innerObject, false);
        }

        private (object? Obj, bool IsModify) TryAddToList(object listObject)
        {
            var type = listObject.GetType();
            var innerType = type.GetCollectionElementType();

            var newValue = Activator.CreateInstance(type);
            var list = (IList)listObject;
            var isModifyedListValue = false;

            // Try Add
            if (TryChange)
            {
                var addNewObject = _randomTypeConstructor.ConstructByType(innerType);
                type.GetMethod("Add").Invoke(newValue, new object[] { addNewObject });
                isModifyedListValue = true;
            }

            // Try add in inner value
            foreach (var item in list)
            {
                if (isModifyedListValue)
                {
                    type.GetMethod("Add").Invoke(newValue, new object[] { item });
                    continue;
                }

                if (innerType.IsClass)
                {
                    var tryModifyInnerObject = TryAddInnerObject(item, innerType);
                    if (tryModifyInnerObject.IsModify)
                    {
                        isModifyedListValue = true;
                    }

                    type.GetMethod("Add").Invoke(newValue, new object[] { tryModifyInnerObject.Obj });
                }
                else
                {
                    type.GetMethod("Add").Invoke(newValue, new object[] { item });
                }
            }

            // Try Add
            if (TryChange && !isModifyedListValue)
            {
                var addNewObject = _randomTypeConstructor.ConstructByType(innerType);
                type.GetMethod("Add").Invoke(newValue, new object[] { addNewObject });
                isModifyedListValue = true;
            }

            if (isModifyedListValue)
            {
                return (newValue, true);
            }
            else
            {
                return (newValue, false);
            }
        }

        private (object? Obj, bool IsModify) TryAddToDictionary(object dictObject)
        {
            var type = dictObject.GetType();
            var innerType = type.GetDictionaryElementTypes();

            var newValue = Activator.CreateInstance(type);
            var list = (IDictionary)dictObject;
            var isModifyedListValue = false;

            foreach (var item in list)
            {
                var itemType = item.GetType();

                var key = itemType.GetProperty("Key").GetValue(item);
                var value = itemType.GetProperty("Value").GetValue(item);

                if (isModifyedListValue)
                {
                    type.GetMethod("Add").Invoke(newValue, new object[] { key, value });
                    continue;
                }

                var tryModifyKey = TryAddInnerObject(key, innerType.key);
                if (tryModifyKey.IsModify)
                {
                    if (!(bool)type.GetMethod("ContainsKey").Invoke(newValue, new object[] { tryModifyKey.Obj }))
                    {
                        type.GetMethod("Add").Invoke(newValue, new object[] { tryModifyKey.Obj, value });
                        isModifyedListValue = true;
                        continue;
                    }
                }

                var tryModifyInnerValue = TryAddInnerObject(value, innerType.value);
                if (tryModifyInnerValue.IsModify)
                {
                    if ((bool)type.GetMethod("ContainsKey").Invoke(newValue, new object[] { key }))
                    {
                        var method = type.GetMethods().Where(m => m.Name == "Remove").First(m => m.GetParameters().Length == 1);
                        method.Invoke(newValue, new object[] { key });
                    }

                    type.GetMethod("Add").Invoke(newValue, new object[] { key, tryModifyInnerValue.Obj });
                    isModifyedListValue = true;
                    continue;
                }

                type.GetMethod("Add").Invoke(newValue, new object[] { key, value });
            }

            if (TryChange)
            {
                var newAddedKey = _randomTypeConstructor.ConstructByType(innerType.key);
                var newAddedValue = _randomTypeConstructor.ConstructByType(innerType.value);

                if (!(bool)type.GetMethod("ContainsKey").Invoke(newValue, new object[] { newAddedKey }))
                {
                    type.GetMethod("Add").Invoke(newValue, new object[] { newAddedKey, newAddedValue });
                    isModifyedListValue = true;
                }
            }

            if (isModifyedListValue)
            {
                return (newValue, true);
            }
            else
            {
                return (newValue, false);
            }
        }

        #endregion

        private bool TryChange => new Random().Next(100) == 1;
    }
}
