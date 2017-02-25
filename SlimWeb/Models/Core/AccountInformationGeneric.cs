using SW.Core.DAL;
using SW.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SlimWeb.Models.Core
{
    public class AccountInformationGeneric
    {
        public int Id { get; set; }
        public int AccountInformationGenericId { get; set; }
        public int AccountId { get; set; }
        public string Value { get; set; }
        public string Name { get; set; }

        internal static List<Account_Information_Generic_Value> MapGenericValues(List<AccountInformationGeneric> genericValues,
            int accountId)
        {
            List<Account_Information_Generic_Value> values = new List<Account_Information_Generic_Value>();
            genericValues.ForEach(g => values.Add(MapGenericValue(g, accountId)));
            return values;
        }

        internal static Account_Information_Generic_Value MapGenericValue(AccountInformationGeneric genericValue,
            int accountId)
        {
            return new Account_Information_Generic_Value()
            {
                AccountId = accountId,
                AccountInformationGenericId = genericValue.AccountInformationGenericId,
                Id = genericValue.Id,
                Value = genericValue.Value
            };
        }

        internal static List<AccountInformationGeneric> MapValues(List<Account_Information_Generic> values)
        {
            List<AccountInformationGeneric> models = new List<AccountInformationGeneric>();
            values.ForEach(v => models.Add(MapValue(v)));
            return models;
        }

        internal static AccountInformationGeneric MapValue(Account_Information_Generic value)
        {
            AccountInformationGeneric info = null;

            try
            {
                info = new AccountInformationGeneric()
                {
                    AccountId = value.Account_Information_Generic_Value.FirstOrDefault() != null ? value.Account_Information_Generic_Value.FirstOrDefault().AccountId : 0,
                    AccountInformationGenericId = value.Id,
                    Id = value.Account_Information_Generic_Value.FirstOrDefault() != null ? value.Account_Information_Generic_Value.FirstOrDefault().Id : 0,
                    Value = value.Account_Information_Generic_Value.FirstOrDefault() != null ? value.Account_Information_Generic_Value.FirstOrDefault().Value : "",
                    Name = value.Name
                };
            }
            catch (Exception ex)
            {
                LogHelper.LogError(string.Format("Couldn't map AccountInformationGeneric: MapValue(Account_Information_Generic value)"), ex, 0);
            }

            return info;
        }
    }
}