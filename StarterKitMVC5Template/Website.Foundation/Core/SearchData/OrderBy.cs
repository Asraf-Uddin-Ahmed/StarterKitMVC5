using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using $safeprojectname$.Core.Aggregates;

namespace $safeprojectname$.Core.SearchData
{
    public class OrderBy<TEntity> where TEntity : Entity
    {
        private static readonly Func<TEntity, dynamic> DEFAULT_PREDICATE_ORDERBY = (col) => col.ID;


        public Func<TEntity, dynamic> PredicateOrderBy { get; set; }
        public bool IsAscending { get; set; }


        public OrderBy(Func<TEntity, dynamic> predicateOrderBy, bool isAscending)
        {
            this.PredicateOrderBy = predicateOrderBy;
            this.IsAscending = isAscending;
        }
        public OrderBy() : this(DEFAULT_PREDICATE_ORDERBY, true)
        {
        }
    }
}
