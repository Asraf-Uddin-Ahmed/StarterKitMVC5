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


        protected abstract Func<TEntity, bool> GetAndSearchCondition(TSearch searchItem);
        protected abstract Func<TEntity, bool> GetOrSearchCondition(TSearch searchItem);



        public int GetTotalAnd(TSearch searchItem)
        {
            Func<TEntity, bool> predicateCount = this.GetAndSearchCondition(searchItem);
            int total = this.GetTotalBy(predicateCount);
            return total;
        }
        public int GetTotalOr(TSearch searchItem)
        {
            Func<TEntity, bool> predicateCount = this.GetOrSearchCondition(searchItem);
            int total = this.GetTotalBy(predicateCount);
            return total;
        }

        public ICollection<TEntity> GetByAnd(TSearch searchItem, Pagination pagination, OrderBy<TEntity> orderBy)
        {
            Func<TEntity, bool> predicateWhere = this.GetAndSearchCondition(searchItem);
            ICollection<TEntity> listEntity = this.GetBy(pagination, orderBy, predicateWhere);
            return listEntity;
        }
        public ICollection<TEntity> GetByOr(TSearch searchItem, Pagination pagination, OrderBy<TEntity> orderBy)
        {
            Func<TEntity, bool> predicateWhere = this.GetOrSearchCondition(searchItem);
            ICollection<TEntity> listEntity = this.GetBy(pagination, orderBy, predicateWhere);
            return listEntity;
        }


        protected int GetTotalBy(Func<TEntity, bool> predicateCount)
        {
            return _context.Set<TEntity>().Count(predicateCount);
        }
        protected ICollection<TEntity> GetBy(Pagination pagination, OrderBy<TEntity> orderBy, Func<TEntity, bool> predicateWhere)
        {
            ICollection<TEntity> listEntity = _context.Set<TEntity>()
                .Where(predicateWhere)
                .OrderByDirection(orderBy.PredicateOrderBy, orderBy.IsAscending)
                .Skip(pagination.DisplayStart).Take(pagination.DisplaySize).ToList();
            return listEntity;
        }
        protected bool IsAllPropertyNull(TSearch obj)
        {
            if (obj == null)
            {
                return true;
            }
            bool isAnyNotNull = obj.GetType().GetProperties().Any(c => c.GetValue(obj) != null);
            return !isAnyNotNull;
        }

    }
}
