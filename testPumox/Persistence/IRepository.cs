using System.Collections.Generic;
using testPumox.Domain;

namespace testPumox.Persistence
{
    public interface IRepository<TEntity> where TEntity : EntityBase
    {
        bool ExistsById(long id);
        TEntity GetById(long id);
        IReadOnlyCollection<TEntity> ListAll();
        TEntity Add(TEntity model);
        void Update(TEntity entity);
        void Delete(TEntity model);
    }
}