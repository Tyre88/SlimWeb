using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SW.Entities.Mediabank
{
    public class MediabankEntity
    {
        public int Id { get; set; }
        public int ClubId { get; set; }
        public int CreatedById { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string FileUrl { get; set; }
        public string Thumbnail { get; set; }
        public string FileType { get; set; }
        public string FileExtension { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool LimitFileAccess { get; set; }
    }
}
