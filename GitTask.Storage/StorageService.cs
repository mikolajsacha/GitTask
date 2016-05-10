using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using GitTask.Repository.Services.Interface;
using GitTask.Domain.Attributes;
using GitTask.Domain.Services.Interface;
using GitTask.Storage.Interface;

namespace GitTask.Storage
{
    public class StorageService<TDataObject> : IStorageService<TDataObject>
    {
        private const string FilesExtension = ".json";
        private readonly PropertyInfo _dataObjectKeyProperty;

        private readonly IFileService _fileService;
        private readonly IProjectPathsReadonlyService _projectPathsService;

        public event Action<string> StorageOperationStarted;
        public event Action<string> StorageOperationFinished;

        public StorageService(IFileService fileService, IProjectPathsReadonlyService projectPathsService)
        {
            _fileService = fileService;
            _projectPathsService = projectPathsService;
            _dataObjectKeyProperty = KeyAttribute.GetKeyProperty(typeof(TDataObject));
        }

        public async Task Save(TDataObject objectToBeSaved)
        {
            Directory.CreateDirectory(GetBasePath());
            var keyValue = _dataObjectKeyProperty.GetValue(objectToBeSaved);
            var filePath = GetFullPath(keyValue.ToString());
            StorageOperationStarted?.Invoke(filePath);
            await _fileService.Save(objectToBeSaved, filePath);
            StorageOperationFinished?.Invoke(filePath);
        }

        public async Task Delete(object objectToBeDeletedKeyValue)
        {
            var filePath = GetFullPath(objectToBeDeletedKeyValue.ToString());
            StorageOperationStarted?.Invoke(filePath);
            await _fileService.Delete(filePath);
            StorageOperationFinished?.Invoke(filePath);
        }

        public async Task<IEnumerable<TDataObject>> GetAll()
        {
            var result = new LinkedList<TDataObject>();
            if (!Directory.Exists(GetBasePath())) return result;

            foreach (var fileName in Directory.GetFiles(GetBasePath()).Where(fileName => fileName.EndsWith(FilesExtension)))
            {
                result.AddLast(await _fileService.Load<TDataObject>(fileName));
            }
            return result;
        }

        private string GetFullPath(string fileName)
        {
            return GetBasePath() + '\\' + fileName + FilesExtension;
        }

        private string GetBasePath()
        {
            return _projectPathsService.GetPathForModel(typeof(TDataObject));
        }
    }
}