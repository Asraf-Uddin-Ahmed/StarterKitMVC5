using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace $safeprojectname$.Core.SearchData
{
    public class Pagination
    {
        private const int DEFAULT_DISPLAY_SIZE = 10;
        private int _displayStart;
        private int _displaySize;
        
        
        public int DisplayStart 
        {
            get
            {
                return _displayStart;
            }
            set
            {
                _displayStart = Math.Max(0, value);
            }
        }
        public int DisplaySize
         {
            get
            {
                return _displaySize;
            }
            set
            {
                _displaySize = Math.Max(0, value);
            }
        }


        public Pagination(int displayStart, int displaySize)
        {
            this.DisplayStart = displayStart;
            this.DisplaySize = displaySize;
        }
        public Pagination() : this(0, DEFAULT_DISPLAY_SIZE) { }
    }
}
