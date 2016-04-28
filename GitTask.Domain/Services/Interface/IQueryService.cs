using System.Collections.Generic;

namespace GitTask.Domain.Services.Interface
{
    public interface IQueryService<TModel>
    {
        void AddNew(TModel modelObject);
        void Update(TModel modelObject);
        TModel GetByKey(object keyValue);
        IEnumerable<TModel> GetByProperty(string propertyName, object propertyValue);
        IEnumerable<TModel> GetAll();
        void SaveChanges();
    }
}
