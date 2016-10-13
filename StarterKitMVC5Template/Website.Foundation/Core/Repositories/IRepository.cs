using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using $safeprojectname$.Core.Aggregates;
using $safeprojectname$.Core.SearchData;

namespace $safeprojectname$.Core.Repositories
{
    public interface IRepository<TEntity> where TEntity : Entity
    {
        void Add(TEntity entity);
        void AddRange(ICollection<TEntity> entities);

        void Update(TEntity entity);

        void Remove(TEntity currentItem);
        void Remove(Guid ID);
        void RemoveRange(ICollection<TEntity> entities);

        TEntity Get(Guid ID);
        ICollection<TEntity> GetAll();
        ICollection<TEntity> GetBy(Pagination pagination, OrderBy<TEntity> orderBy);

        int GetTotal();
    }
}
