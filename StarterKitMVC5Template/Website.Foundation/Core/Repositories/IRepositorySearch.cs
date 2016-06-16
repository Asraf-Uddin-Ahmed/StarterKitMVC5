using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using $safeprojectname$.Core.Aggregates;
using $safeprojectname$.Core.SearchData;

namespace $safeprojectname$.Core.Repositories
{
    public interface IRepositorySearch<TEntity, TSearch> : IRepository<TEntity>
        where TEntity : Entity
        where TSearch : EntitySearch
    {
        IEnumerable<TEntity> GetByAnd(TSearch searchItem, Pagination pagination, OrderBy<TEntity> sortBy);
        IEnumerable<TEntity> GetByOr(TSearch searchItem, Pagination pagination, OrderBy<TEntity> sortBy);
        int GetTotalAnd(TSearch searchItem);
        int GetTotalOr(TSearch searchItem);
    }
}
