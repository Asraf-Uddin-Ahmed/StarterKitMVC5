using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Reflection;
using Ninject;
using Ratul.Utility;
using $safeprojectname$.Core.Repositories;
using $safeprojectname$.Core.Aggregates;
using System.Linq.Expressions;
using $safeprojectname$.Core.Container;

namespace $safeprojectname$.Persistence.Repositories
{
    public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity
    {
        protected readonly DbSet<TEntity> dbSet;
        private readonly DbContext _context;
        public Repository(DbContext context)
        {
            _context = context;
            dbSet = _context.Set<TEntity>();
        }


        public void Add(TEntity entity)
        {
            dbSet.Add(entity);
        }
        public void AddRange(IEnumerable<TEntity> entities)
        {
            dbSet.AddRange(entities);
        }


        public void Update(TEntity entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }


        public void Remove(TEntity entity)
        {
            dbSet.Remove(entity);
        }
        public void Remove(Guid ID)
        {
            TEntity currentItem = this.Get(ID);
        }
        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            dbSet.RemoveRange(entities);
        }


        public TEntity Get(Guid ID)
        {
            return dbSet.Find(ID);
        }
        public IEnumerable<TEntity> GetAll()
        {
            return dbSet.ToList();
        }
        public IEnumerable<TEntity> GetBy(int index, int size, SortBy<TEntity> sortBy)
        {
            ICollection<TEntity> listEntity = _context.Set<TEntity>()
                .OrderByDirection(sortBy.PredicateOrderBy, sortBy.IsAscending)
                .Skip(index).Take(size).ToList<TEntity>();
            return listEntity;
        }
        

        public int GetTotal()
        {
            return dbSet.Count();
        }
        
    }
}
