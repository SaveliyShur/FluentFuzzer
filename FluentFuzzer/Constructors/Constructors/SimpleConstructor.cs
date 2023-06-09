﻿using FluentFuzzer.Constructors.ConstructorExceptions;
using FluentFuzzer.Constructors.ConstructorInterfaces;
using FluentFuzzer.Utils;
using FuzzerRunner.Constructors;

namespace FluentFuzzer.Constructors.Constructors
{
    public class SimpleConstructor<Model> : BaseConstructor, IUploadObjects<Model> where Model : class
    {
        private List<Model> _objects = new ();
        private int _counter = -1;

        private static readonly object _lock = new ();

        public override ConstructorEnum GetConstructorEnum() => ConstructorEnum.Simple;

        public override T Construct<T>()
        {
            if (typeof(T) != typeof(Model))
                throw new InvalidOperationException("Type of T not equals ModelType");

            lock(_lock)
            {
                if (_counter >= _objects.Count - 1)
                    throw new ConstructorBreakException("All objects were constructed.");

                _counter++;

                return (T)(object)_objects[_counter];
            }
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
    }
}
