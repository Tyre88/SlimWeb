using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SW.Core.Helpers
{
    public static class AppSettingsHelper
    {
        public static string GetAppSetting(string key)
        {
            string settingValue = null;
            System.Configuration.Configuration rootWebConfig =
                System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~");
            if (rootWebConfig.AppSettings.Settings.Count > 0)
            {
                System.Configuration.KeyValueConfigurationElement customSetting =
                    rootWebConfig.AppSettings.Settings[key];
                if (customSetting != null)
                    settingValue = customSetting.Value;
            }
            return settingValue;
        }
    }
}
