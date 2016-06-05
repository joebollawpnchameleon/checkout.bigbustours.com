

using bigbus.checkout.data.Model;
using System.Configuration;

namespace bigbus.checkout.Helpers
{
    public class SettingsHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="site"></param>
        /// <param name="settingName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static string SubSiteSetting(MicroSite site, string settingName, string defaultValue)
        {
            var key = string.Format("{0}-{1}", site.Id, settingName);
            var val = ConfigurationManager.AppSettings[key];
            return val ?? defaultValue;
        }

        public static int SubSiteSettingInt(MicroSite site, string settingName, int defaultValue)
        {
            var key = string.Format("{0}-{1}", site.Id, settingName);
            var val = ConfigurationManager.AppSettings[key];
            if (val == null) return defaultValue;

            int intValue;
            if (int.TryParse(val, out intValue))
                return intValue;

            return defaultValue;
        }

        public static string GlobalSetting(string settingName, string defaultValue)
        {
            var val = ConfigurationManager.AppSettings[settingName];
            return val ?? defaultValue;
        }
    }
}