using System.Threading.Tasks;

namespace GitTask.Storage.Interface
{
    public interface IFileService
    {
        Task Save(object objectToBeSaved, string filePath);
        Task<TDataObject> Load<TDataObject>(string filePath);
    }
}
