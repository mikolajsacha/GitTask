using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GitTask.Domain.Services.Interface
{
    public interface IStorageService
    {
        event Action<string> StorageOperationStarted;
        event Action<string> StorageOperationFinished;
    }

    public interface IStorageService<TDataObject> : IStorageService
    {
        Task Save(TDataObject objectToBeSaved);
        Task Delete(object objectToBeDeletedKeyValue);
        Task<IEnumerable<TDataObject>> GetAll();
    }
}
