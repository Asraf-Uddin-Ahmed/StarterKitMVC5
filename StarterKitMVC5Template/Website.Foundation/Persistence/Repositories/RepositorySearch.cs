using Ratul.Utility;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using $safeprojectname$.Core.Aggregates;
using $safeprojectname$.Core.Repositories;
using $safeprojectname$.Core.SearchData;

namespace $safeprojectname$.Persistence.Repositories
{
    public abstract class RepositorySearch<TEntity, TSearch> : Repository<TEntity>, IRepositorySearch<TEntity, TSearch>
        where TEntity : Entity
        where TSearch : EntitySearch
    {

        private DbContext _context;
        public RepositorySearch(DbContext context)
            : base(context)
        {
            _context = context;
        }


        protected abstract Expression<Func<TEntity, bool>> GetAndSearchCondition(TSearch searchItem);
        protected abstract Expression<Func<TEntity, bool>> GetOrSearchCondition(TSearch searchItem);



        public int GetTotalAnd(TSearch searchItem)
        {
            Expression<Func<TEntity, bool>> predicateCount = this.GetAndSearchCondition(searchItem);
            int total = this.GetTotalBy(predicateCount);
            return total;
        }
        public int GetTotalOr(TSearch searchItem)
        {
            Expression<Func<TEntity, bool>> predicateCount = this.GetOrSearchCondition(searchItem);
            int total = this.GetTotalBy(predicateCount);
            return total;
        }

        public IEnumerable<TEntity> GetByAnd(TSearch searchItem, Pagination pagination, OrderBy<TEntity> sortBy)
        {
            Expression<Func<TEntity, bool>> predicateWhere = this.GetAndSearchCondition(searchItem);
            IEnumerable<TEntity> listUser = this.GetBy(pagination, sortBy, predicateWhere);
            return listUser;
        }
        public IEnumerable<TEntity> GetByOr(TSearch searchItem, Pagination pagination, OrderBy<TEntity> sortBy)
        {
            Expression<Func<TEntity, bool>> predicateWhere = this.GetOrSearchCondition(searchItem);
            IEnumerable<TEntity> listUser = this.GetBy(pagination, sortBy, predicateWhere);
            return listUser;
        }


        protected int GetTotalBy(Expression<Func<TEntity, bool>> predicateCount)
        {
            return _context.Set<TEntity>().Count(predicateCount);
        }
        protected IEnumerable<TEntity> GetBy(Pagination pagination, OrderBy<TEntity> sortBy, Expression<Func<TEntity, bool>> predicateWhere)
        {
            IEnumerable<TEntity> listEntity = _context.Set<TEntity>()
                .Where(predicateWhere)
                .OrderByDirection(sortBy.PredicateOrderBy, sortBy.IsAscending)
                .Skip(pagination.DisplayStart).Take(pagination.DisplaySize);
            return listEntity;
        }
        protected bool IsAllPropertyNull(TSearch obj)
        {
            bool isAnyNotNull = obj.GetType().GetProperties().Any(c => c.GetValue(obj) != null);
            return !isAnyNotNull;
        }

    }
}
