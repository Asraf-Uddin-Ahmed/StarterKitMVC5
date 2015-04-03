using Ratul.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace $safeprojectname$.Aggregates
{
    public class Entity : IEntity
    {
        [Required]
        public Guid ID { get; set; }

        public Entity()
        {
            ID = GuidUtility.GetNewSequentialGuid();
        }
    }
}
