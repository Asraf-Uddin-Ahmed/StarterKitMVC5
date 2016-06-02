using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using $safeprojectname$.Core.Aggregates;

namespace $safeprojectname$.Core.Container
{
    public class SortBy<TEntity> where TEntity : class
    {
        public Expression<Func<TEntity, dynamic>> PredicateOrderBy { get; set; }
        public bool IsAscending { get; set; }
    }
}
