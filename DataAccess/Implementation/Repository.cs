using DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace DataAccess.Implementation
{
    public class Repository<TEntity> : IRepository<TEntity>
        where TEntity : class
    {
        protected readonly TmDbContext Context;

        public Repository(TmDbContext context)
        {
            Context = context;
        }

        public IQueryable<TEntity> Get()
        {
            return Context.Set<TEntity>();
        }

        public IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> predicate)
        {
            return Context.Set<TEntity>().Where(predicate);
        }

        public void Add(TEntity entity)
        {
            Context.Set<TEntity>().Add(entity);
        }

        public void Add(IEnumerable<TEntity> entities)
        {
            Context.Set<TEntity>().AddRange(entities);
        }

        public void Remove(TEntity entity)
        {
            Context.Set<TEntity>().Remove(entity);
        }

        public void Remove(IEnumerable<TEntity> entities)
        {
            Context.Set<TEntity>().RemoveRange(entities);
        }

        public void Update(TEntity entity)
        {
            Context.Set<TEntity>().Update(entity);
        }
    }
}
