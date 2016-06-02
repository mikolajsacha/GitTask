using System;
using System.IO;
using System.Threading.Tasks;
using GitTask.Json.Exception;
using GitTask.Storage.Interface;
using Newtonsoft.Json;

namespace GitTask.Json
{
    public class JsonFileService : IFileService
    {
        public string FilesExtension => ".json";

        public async Task Save(object objectToBeSaved, string filePath)
        {
            var modelJson = JsonConvert.SerializeObject(objectToBeSaved);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            var bufferToWrite = BufferWorker.ToBuffer(modelJson);
            using (var saveFile = new FileStream(filePath, FileMode.CreateNew))
            {
                await saveFile.WriteAsync(bufferToWrite, 0, bufferToWrite.Length);
            }
        }

        public Task Delete(string filePath)
        {
            return Task.Factory.StartNew(() =>
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            });
        }

        public TDataObject ParseString<TDataObject>(string content)
        {
            return JsonConvert.DeserializeObject<TDataObject>(content);
        }

        public async Task<TDataObject> Load<TDataObject>(string filePath)
        {
            try
            {
                byte[] bufferToRead;
                using (var readFile = new FileStream(filePath, FileMode.Open))
                {
                    try
                    {
                        var fileLength = Convert.ToInt32(readFile.Length);
                        bufferToRead = new byte[fileLength];
                        await readFile.ReadAsync(bufferToRead, 0, fileLength);
                    }
                    catch (OverflowException)
                    {
                        throw new FileTooBigException(filePath, int.MaxValue);
                    }
                }
                var modelJson = BufferWorker.ToString(bufferToRead);
                return ParseString<TDataObject>(modelJson);
            }
            catch (FileNotFoundException)
            {
                throw new FileNotFoundException($"File \"{filePath}\" does not exist");
            }
        }
    }
}
