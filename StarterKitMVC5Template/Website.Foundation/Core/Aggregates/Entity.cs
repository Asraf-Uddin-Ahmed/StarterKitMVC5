using Ratul.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace $safeprojectname$.Core.Aggregates
{
    public abstract class Entity
    {
        [Required]
        public Guid ID { get; set; }

        //[Required]
        //public Guid CreatedByUserID { get; set; }
        //[Required]
        //public Guid UpdatedByUserID { get; set; }
        //[Required]
        //public DateTime CreationTime { get; set; }
        //[Required]
        //public DateTime UpdateTime { get; set; }

        //public Entity()
        //{
        //    ID = GuidUtility.GetNewSequentialGuid();
        //    UpdateTime = DateTime.UtcNow;
        //}
    }
}
