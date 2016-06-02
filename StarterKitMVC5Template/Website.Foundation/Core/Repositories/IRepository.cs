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
        void Add(TEntity entity, bool isPersist = false);
        void AddRange(IEnumerable<TEntity> entities, bool isPersist = false);

        void Update(TEntity entity, bool isPersist = false);

        void Remove(TEntity currentItem, bool isPersist = false);
        void Remove(Guid ID, bool isPersist = false);
        void RemoveRange(IEnumerable<TEntity> entities, bool isPersist = false);

        TEntity Get(Guid ID);
        IEnumerable<TEntity> GetAll();
        IEnumerable<TEntity> GetBy(int index, int size, SortBy<TEntity> sortBy);

        int GetTotal();

        void Commit();
    }
}
