using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using GitTask.Json.Exception;
using GitTask.Storage.Interface;
using Newtonsoft.Json;

namespace GitTask.Json
{
    public class JsonFileService : IFileService
    {
        public async Task Save(object objectToBeSaved, string filePath)
        {
            await Task.Run(() => { Thread.Sleep(5000); });
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
                return JsonConvert.DeserializeObject<TDataObject>(modelJson);
            }
            catch (FileNotFoundException)
            {
                throw new FileNotFoundException($"File \"{filePath}\" does not exist");
            }
        }
    }
}
