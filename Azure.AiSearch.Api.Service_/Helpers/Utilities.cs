using System.Linq.Expressions;
using System.Runtime.Caching;

namespace AzureSearch.Api.Service.Helpers
{
    public class Utilities
    {
        public static string appSettingFilePath = AppContext.BaseDirectory;

        /// <summary>
        /// Return appSetting value by name
        /// </summary>
        /// <param name="appSettingName"></param>
        /// <returns></returns>
        public static string GetAppSetting(string appSettingName)
        {
            return GetAppSettingValue(appSettingName);
        }

        private static string GetAppSettingValue(string appSettingName)
        {
            var configuration = new ConfigurationBuilder()
                    .SetBasePath(appSettingFilePath)
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .Build();
            if (configuration[appSettingName] == null)
            {
                return null;
            }
            else
            {
                return configuration[appSettingName];
            }
        }
    }
}
