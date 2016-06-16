using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace $safeprojectname$.Models.Common
{
    public class SortBy
    {
        public string FieldName { get; set; }
        public bool IsAscending { get; set; }


        public SortBy(string fieldName, bool isAscending)
        {
            this.FieldName = fieldName;
            this.IsAscending = isAscending;
        }

    }
}