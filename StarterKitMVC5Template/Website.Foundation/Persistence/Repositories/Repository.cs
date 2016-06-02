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


        public void Add(TEntity entity, bool isPersist = false)
        {
            dbSet.Add(entity);
            this.Commit(isPersist);
        }
        public void AddRange(IEnumerable<TEntity> entities, bool isPersist = false)
        {
            dbSet.AddRange(entities);
            this.Commit(isPersist);
        }


        public void Update(TEntity entity, bool isPersist = false)
        {
            _context.Entry(entity).State = EntityState.Modified;
            this.Commit(isPersist);
        }


        public void Remove(TEntity entity, bool isPersist = false)
        {
            dbSet.Remove(entity);
            this.Commit(isPersist);
        }
        public void Remove(Guid ID, bool isPersist = false)
        {
            TEntity currentItem = this.Get(ID);
            this.Remove(currentItem, isPersist);
        }
        public void RemoveRange(IEnumerable<TEntity> entities, bool isPersist = false)
        {
            dbSet.RemoveRange(entities);
            this.Commit(isPersist);
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


        public void Commit()
        {
            this.Commit(true);
        }



        private void Commit(bool isPersist)
        {
            if(isPersist)
            {
                _context.SaveChanges();
            }
        }
    }
}
