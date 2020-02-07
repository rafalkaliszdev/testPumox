using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using testPumox.Domain;

namespace testPumox.Persistence
{
    public class EfRepository<TEntity> : IRepository<TEntity> where TEntity : EntityBase
    {
        protected readonly EfDbContext _dbContext;

        public EfRepository(EfDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public bool ExistsById(long id)
        {
            return _dbContext.Set<TEntity>().Any(p => p.Id == id);
        }

        public virtual TEntity GetById(long id)
        {
            return _dbContext.Set<TEntity>().Find(id);
        }

        public IReadOnlyCollection<TEntity> ListAll()
        {
            return _dbContext.Set<TEntity>().ToList();
        }

        public TEntity Add(TEntity entity)
        {
            _dbContext.Set<TEntity>().Add(entity);
            _dbContext.SaveChanges();
            return entity;
        }

        public void Update(TEntity entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            _dbContext.SaveChanges();
        }

        public void Delete(TEntity entity)
        {
            _dbContext.Set<TEntity>().Remove(entity);
            _dbContext.SaveChanges();
        }
    }
}