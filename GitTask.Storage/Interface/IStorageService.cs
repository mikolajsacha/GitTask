using System.Collections.Generic;
using System.Threading.Tasks;

namespace GitTask.Storage.Interface
{
    public interface IStorageService<TDataObject>
    {
        Task Save(TDataObject objectToBeSaved);
        Task<IEnumerable<TDataObject>> GetAll();
    }
}
