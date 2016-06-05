using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using $safeprojectname$.Core;

namespace $safeprojectname$.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private TableContext _context;
        [Inject]
        public UnitOfWork(TableContext context)
        {
            _context = context;
        }



        public void Commit()
        {
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
