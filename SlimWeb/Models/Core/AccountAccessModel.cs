using SW.Core.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SlimWeb.Models
{
    public class AccountAccessModel
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public int AccessId { get; set; }

        public static AccountAccessModel MapAccountAccess(AccountAccess accountAccess)
        {
            return new AccountAccessModel()
            {
                AccessId = accountAccess.AccessID,
                AccountId = accountAccess.AccountID,
                Id = accountAccess.ID
            };
        }

        public static List<AccountAccessModel> MapAccountAccesses(List<AccountAccess> accountAccesses)
        {
            List<AccountAccessModel> models = new List<AccountAccessModel>();
            accountAccesses.ForEach(a => models.Add(MapAccountAccess(a)));
            return models;
        }
    }
}