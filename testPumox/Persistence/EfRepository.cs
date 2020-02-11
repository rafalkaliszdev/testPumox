using Microsoft.EntityFrameworkCore;
using System;
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

        public virtual TEntity GetById(long id)
        {
            return _dbContext.Set<TEntity>().Find(id);
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

        public void DeleteCompany(long id)
        {
            // No need to retrieve record from db.
            var company = Activator.CreateInstance<Company>();
            company.Id = id;
            _dbContext.Company.Attach(company);
            _dbContext.Company.Remove(company);
            _dbContext.SaveChanges();

            // Previous approach.
            //_dbContext.Set<TEntity>().Remove(entity);
            //_dbContext.SaveChanges();
        }
    }
}