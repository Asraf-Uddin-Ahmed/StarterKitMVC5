using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using Website.Foundation.Core.Aggregates;
using Website.Foundation.Core.SearchData;

namespace $safeprojectname$.Models.Request
{
    public class RequestSearchModel<TEntity, TSearch>
        where TEntity : Entity
        where TSearch : EntitySearch
    {
        private OrderBy<TEntity> _orderBy;


        public TSearch SearchItem { get; set; }
        public Pagination Pagination { get; set; }
        public SortBy SortBy { get; set; }
        public OrderBy<TEntity> OrderBy
        {
            get
            {
                if(_orderBy==null)
                {
                    _orderBy = new OrderBy<TEntity>();
                }
                if (this.SortBy != null && !string.IsNullOrWhiteSpace(this.SortBy.FieldName))
                {
                    PropertyInfo propertyInfo = typeof(TEntity).GetProperty(this.SortBy.FieldName);
                    if (propertyInfo==null)
                    {
                        throw new NullReferenceException("SortBy.FieldName value does not match with Entity");
                    }
                    _orderBy.PredicateOrderBy = x => propertyInfo.GetValue(x, null);
                    _orderBy.IsAscending = this.SortBy.IsAscending;
                }
                return _orderBy;
            }
        }


        public RequestSearchModel()
        {
            this.Pagination = new Pagination();
            this.SortBy = new SortBy("ID", true);
        }
    }
}