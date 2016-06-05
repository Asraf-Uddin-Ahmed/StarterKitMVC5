using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using $safeprojectname$.Core.Container;

namespace $safeprojectname$.Core.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        void Add(TEntity entity);
        void AddRange(IEnumerable<TEntity> entities);

        void Update(TEntity entity);

        void Remove(TEntity currentItem);
        void Remove(Guid ID);
        void RemoveRange(IEnumerable<TEntity> entities);

        TEntity Get(Guid ID);
        IEnumerable<TEntity> GetAll();
        IEnumerable<TEntity> GetBy(int index, int size, SortBy<TEntity> sortBy);

        int GetTotal();
    }
}
