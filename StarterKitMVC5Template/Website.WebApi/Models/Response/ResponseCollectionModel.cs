using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Website.Foundation.Core.Aggregates;
using Website.Foundation.Core.SearchData;

namespace $safeprojectname$.Models.Response
{
    public class ResponseCollectionModel<TModel> 
        where TModel : ResponseModel
    {
        public IEnumerable<TModel> Items { get; set; }
        public Pagination Pagination { get; set; }
        public SortBy SortBy { get; set; }
        public int TotalItem { get; set; }

    }
}