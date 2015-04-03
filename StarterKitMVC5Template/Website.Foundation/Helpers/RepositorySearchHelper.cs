using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace $safeprojectname$.Helpers
{
    internal class RepositorySearchHelper : IRepositorySearchHelper
    {
        public bool IsAllPropertyNull<TSearch>(TSearch obj)
        {
            bool isAnyNotNull = obj.GetType().GetProperties().Any(c => c.GetValue(obj) != null);
            return !isAnyNotNull;
        }
    }
}
