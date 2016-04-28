using System.Collections.Generic;

using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using GitTask.Domain.Attributes;
using GitTask.Storage.Interface;

namespace GitTask.Storage
{
    public class StorageService<TDataObject> : IStorageService<TDataObject>
    {
        private const string FilesExtension = ".json";
        private readonly string _path;
        private readonly PropertyInfo _dataObjectKeyProperty;
        private readonly IFileService _fileService;

        public StorageService(IFileService fileService, StoragePath baseStoragePath)
        {
            _fileService = fileService;
            _dataObjectKeyProperty = KeyAttribute.GetKeyProperty(typeof(TDataObject));
            _path = $"{baseStoragePath.Path.TrimEnd('/', '\\')}/{typeof(TDataObject).Name}";
            Directory.CreateDirectory(_path);
        }

        public Task Save(TDataObject objectToBeSaved)
        {
            var keyValue = _dataObjectKeyProperty.GetValue(objectToBeSaved);
            var filePath = GetFullPath(keyValue.ToString());
            return _fileService.Save(objectToBeSaved, filePath);
        }

        public async Task<IEnumerable<TDataObject>> GetAll()
        {
            var result = new LinkedList<TDataObject>();
            foreach (var fileName in Directory.GetFiles(_path).Where(fileName => fileName.EndsWith(FilesExtension)))
            {
                result.AddLast(await _fileService.Load<TDataObject>(fileName));
            }
            return result;
        }

        private string GetFullPath(string fileName)
        {
            return _path + '\\' + fileName + FilesExtension;
        }
    }
}