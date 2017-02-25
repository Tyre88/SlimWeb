using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SW.Core.Entities
{
    public class AccountSessionEntity
    {
        public int AccountId { get; set; }
        public int ClubId { get; set; }
        public string Token { get; set; }
        public DateTime LoginDate { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}
