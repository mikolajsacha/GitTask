using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GitTask.Repository.Services.Interface;
using GitTask.Domain.Attributes;
using GitTask.Domain.Services.Interface;
using GitTask.Storage.Exception;

namespace GitTask.Storage
{
    public class QueryService<TModel> : IQueryService<TModel>
    {
        private readonly IStorageService<TModel> _storageService;

        private readonly HashSet<object> _recentlyChanged;
        private readonly HashSet<object> _recentlyDeleted;

        private Dictionary<object, TModel> _data;
        private KeyGeneratorService _keyGeneratorService;

        public event Action<TModel> ElementAdded;
        public event Action<TModel> ElementUpdated;
        public event Action<TModel> ElementDeleted;
        public event Action ElementsCleared;

        public QueryService(IStorageService<TModel> storageService, IProjectPathsReadonlyService projectPathsService)
        {
            _storageService = storageService;
            _recentlyChanged = new HashSet<object>();
            _recentlyDeleted = new HashSet<object>();

            projectPathsService.ProjectPathChanged += InitializeDataFromStorage;
            InitializeDataFromStorage();
        }

        private async void InitializeDataFromStorage()
        {
            _recentlyChanged.Clear();
            _recentlyDeleted.Clear();

            _data =  GetKeySortedData(await _storageService.GetAll());

            if (GetKeyType().IsAssignableFrom(typeof(int)))
            {
                _keyGeneratorService = new KeyGeneratorService(_data.Keys.Select(keyValue => (int)keyValue));
            }

            ElementsCleared?.Invoke();
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

        public void Delete(TModel modelObject)
        {
            var modelObjectKeyValue = KeyAttribute.GetKeyValue(modelObject);
            AssertKeyExists(modelObjectKeyValue);
            RemoveFromCollection(modelObjectKeyValue);
            ElementDeleted?.Invoke(modelObject);
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
            UpdateCollection(modelObjectKeyValue, modelObject);
            ElementAdded?.Invoke(modelObject);
        }

        public void Update(TModel modelObject)
        {
            var modelObjectKeyValue = KeyAttribute.GetKeyValue(modelObject);
            AssertKeyExists(modelObjectKeyValue);
            UpdateCollection(modelObjectKeyValue, modelObject);
            ElementUpdated?.Invoke(modelObject);
        }

        public async Task SaveChanges()
        {
            foreach (var modelObjectKeyValue in _recentlyChanged)
            {
                await _storageService.Save(_data[modelObjectKeyValue]);
            }
            foreach (var modelObjectKeyValue in _recentlyDeleted)
            {
                await _storageService.Delete(modelObjectKeyValue);
            }
            _recentlyChanged.Clear();
            _recentlyDeleted.Clear();
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

        private void UpdateCollection(object keyValue, TModel modelObject)
        {
            _data.Add(keyValue, modelObject);
            _recentlyChanged.Add(keyValue);
        }

        private void RemoveFromCollection(object keyValue)
        {
            _data.Remove(keyValue);
            _recentlyDeleted.Add(keyValue);
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