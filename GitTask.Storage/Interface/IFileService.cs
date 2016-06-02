using System.Threading.Tasks;

namespace GitTask.Storage.Interface
{
    public interface IFileService
    {
        string FilesExtension { get; }

        Task Save(object objectToBeSaved, string filePath);
        Task Delete(string filePath);
        TDataObject ParseString<TDataObject>(string content);
        Task<TDataObject> Load<TDataObject>(string filePath);
    }
}
