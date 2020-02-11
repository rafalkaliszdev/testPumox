using System.Collections.Generic;
using testPumox.Domain;

namespace testPumox.Persistence
{
    public interface IRepository<TEntity> where TEntity : EntityBase
    {
        TEntity GetById(long id);
        TEntity Add(TEntity model);
        void Update(TEntity entity);
        void DeleteCompany(long id);
    }
}