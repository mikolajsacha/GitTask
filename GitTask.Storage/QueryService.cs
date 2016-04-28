using System;
using System.Collections.Generic;
using System.Linq;
using GitTask.Domain.Attributes;
using GitTask.Domain.Services.Interface;
using GitTask.Storage.Exception;
using GitTask.Storage.Interface;

namespace GitTask.Storage
{
    public class QueryService<TModel> : IQueryService<TModel>
    {
        private readonly IStorageService<TModel> _storageService;
        private readonly Dictionary<object, TModel> _data;
        private readonly HashSet<object> _recentlyChanged;
        private readonly KeyGeneratorService _keyGeneratorService;

        public QueryService(IStorageService<TModel> storageService)
        {
            _storageService = storageService;
            _recentlyChanged = new HashSet<object>();
            _data = GetKeySortedData(storageService.GetAll().Result);
            if (GetKeyType().IsAssignableFrom(typeof(int)))
            {
                _keyGeneratorService = new KeyGeneratorService(_data.Keys.Select(keyValue => (int)keyValue));
            }
        }

        public IEnumerable<TModel> GetByProperty(string propertyName, object propertyValue)
        {
            var property = typeof(TModel).GetProperty(propertyName);
            if (property == null)
            {
                throw new PropertyNotFoundException(propertyName, typeof(TModel));
            }
            return _data.Values.Where(modelObject => property.GetValue(modelObject) == propertyValue);
        }

        public IEnumerable<TModel> GetAll()
        {
            return _data.Values;
        }

        public TModel GetByKey(object keyValue)
        {
            AssertKeyExists(keyValue);
            return _data[keyValue];
        }

        public void AddNew(TModel modelObject)
        {
            if (_keyGeneratorService != null)
            {
                KeyAttribute.GetKeyProperty(typeof(TModel)).SetValue(modelObject, _keyGeneratorService.GenerateKey());
            }
            var modelObjectKeyValue = KeyAttribute.GetKeyValue(modelObject);
            AssertKeyNotExists(modelObjectKeyValue);
            UpdateDictionary(modelObjectKeyValue, modelObject);
        }

        public void Update(TModel modelObject)
        {
            var modelObjectKeyValue = KeyAttribute.GetKeyValue(modelObject);
            AssertKeyExists(modelObjectKeyValue);
            UpdateDictionary(modelObjectKeyValue, modelObject);
        }

        public async void SaveChanges()
        {
            foreach (var modelObjectKeyValue in _recentlyChanged)
            {
                await _storageService.Save(_data[modelObjectKeyValue]);
            }
            _recentlyChanged.Clear();
        }

        private void AssertKeyExists(object keyValue)
        {
            if (!_data.ContainsKey(keyValue))
            {
                throw new KeyNotExistsException(keyValue);
            }
        }

        private void AssertKeyNotExists(object keyValue)
        {
            if (_data.ContainsKey(keyValue))
            {
                throw new KeyAlreadyExistsException(keyValue);
            }
        }

        private void UpdateDictionary(object keyValue, TModel modelObject)
        {
            _data.Add(keyValue, modelObject);
            _recentlyChanged.Add(keyValue);
        }

        private static Type GetKeyType()
        {
            return KeyAttribute.GetKeyProperty(typeof(TModel)).GetType();
        }

        private static Dictionary<object, TModel> GetKeySortedData(IEnumerable<TModel> data)
        {
            var modelKeyProperty = KeyAttribute.GetKeyProperty(typeof(TModel));
            return data.ToDictionary(modelObject => modelKeyProperty.GetValue(modelObject));
        }
    }
}