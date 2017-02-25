using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SW.Core.Enums
{
    public class GenericItemPermissionEntity
    {
        public int Id { get; set; }
        public int ClubId { get; set; }
        public GenericItemPermissionObjectTypes Type { get; set; }
        public int ObjectId { get; set; }
        public int? AccessrightId { get; set; }
        public int? UserId { get; set; }
    }
}
