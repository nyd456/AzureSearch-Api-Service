namespace AzureSearch.Api.Service.Helpers
{
    public class KeyVaultHelper
    {
        public static int GetInt(string key, int defaultValue = 0)
        {
            var envValue = KeyVaultClient.GetAppSetting(key);

            if (!string.IsNullOrEmpty(envValue)
                && int.TryParse(envValue, out int result))
            {
                return result;
            }

            return defaultValue;
        }


        public static bool GetBoolean(string key, bool defaultValue = false)
        {
            var envValue = KeyVaultClient.GetAppSetting(key);

            if (!string.IsNullOrEmpty(envValue)
                && bool.TryParse(envValue, out bool result))
            {
                return result;
            }

            return defaultValue;
        }
    }
}
