using System.Collections.Generic;
using System.Threading.Tasks;
using GitTask.Storage.Interface;

namespace GitTask.UI.MVVM.Design
{
    public class DesignStorageService<TDataObject> : IStorageService<TDataObject>
    {
        private readonly IEnumerable<TDataObject> _mockData;

        public DesignStorageService(IEnumerable<TDataObject> mockData)
        {
            _mockData = mockData;
        }

        public Task Save(TDataObject objectToBeSaved)
        {
            return Task.Run(() => { }); // DO NOTHING
        }

        public async Task<IEnumerable<TDataObject>> GetAll()
        {
            await Task.Run(() => { }).ConfigureAwait(false); // DO NOTHING
            return _mockData;
        }
    }
}