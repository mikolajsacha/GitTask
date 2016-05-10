using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GitTask.Domain.Services.Interface
{
    public interface IQueryService<TModel>
    {
        event Action<TModel> ElementAdded;
        event Action<TModel> ElementUpdated;
        event Action<TModel> ElementDeleted;
        event Action ElementsCleared;

        void AddNew(TModel modelObject);
        void Update(TModel modelObject);
        void Delete(TModel modelObject);

        TModel GetByKey(object keyValue);
        IEnumerable<TModel> GetByProperty(string propertyName, object propertyValue);
        IEnumerable<TModel> GetAll();
        Task SaveChanges();
    }
}
