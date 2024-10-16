using System;
using System.Runtime.Caching;
using Microsoft.Extensions.Configuration;
using Azure.Security.KeyVault.Secrets;
using Azure.Identity;


namespace AzureSearch.Api.Service.Helpers
{
    public class KeyVaultClient
    {
        const string cacheKeyPrefix = "DEMO_KEYVAULT_KEY_";
        const string LOCAL = "LOCAL";
        const string LOCAL_NOKEYVAULT = "LOCAL_NOKEYVAULT";

        public static string appSettingFilePath = AppContext.BaseDirectory;

        private static readonly object cacheLock = new object();

        //get secret from Azure Keyvault
        public static string GetSecret(string secretName)
        {
            return RetrieveAppSettingValue(secretName, true);
        }
        //get value from app setting
        public static string GetAppSetting(string appSettingName)
        {
            return RetrieveAppSettingValue(appSettingName, false);
        }
        private static string RetrieveAppSettingValue(string keyName, bool useKeyVault)
        {
            string appSettingValue;
            var cacheKey = cacheKeyPrefix + keyName;
            var cachedData = MemoryCache.Default.Get(cacheKey, null) as string;

            if (cachedData != null)
            {
                appSettingValue = cachedData;
                //Console.WriteLine(string.Format("\nGet from cache::{0}", cachedData));
            }
            else
            {
                int cacheExpired;
                string keyValue;

                if (useKeyVault == true)
                    keyValue = GetSecretValue(keyName, out cacheExpired);
                else
                    keyValue = GetAppSettingValue(keyName, out cacheExpired);

                lock (cacheLock)
                {
                    cachedData = MemoryCache.Default.Get(cacheKey, null) as string;
                    if (cachedData != null)
                    {
                        appSettingValue = cachedData;
                    }
                    else
                    {
                        if (keyValue != null)
                            MemoryCache.Default.Set(cacheKey, keyValue, new CacheItemPolicy { SlidingExpiration = TimeSpan.FromHours(cacheExpired) });
                        appSettingValue = keyValue;
                    }
                }
            }
            if (string.IsNullOrEmpty(appSettingValue) && useKeyVault)
                throw new Exception("Seiu.Framework.Azure.KeyVault.KeyVaultClient.RetrieveAppSettingValue, appSetting Key Vault value can not be null:" + keyName);

            return appSettingValue;
        }
        private static string GetSecretValue(string secretName, out int cacheExpired)
        {
            var configuration = new ConfigurationBuilder()
                    .SetBasePath(appSettingFilePath)
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .Build();
            var environment = configuration["ENVIRONMENT"].ToUpper();

            if (environment == LOCAL_NOKEYVAULT  // no azure key vault access, all values are using local appsetting values
                || environment == LOCAL)         // local development access to azure dev key vault using tenant information
            {
                // using appsettings.Development.json
                configuration = new ConfigurationBuilder()
                        .SetBasePath(appSettingFilePath)
                        .AddJsonFile("appsettings.Development.json", optional: false, reloadOnChange: true)
                        .Build();
                cacheExpired = Convert.ToInt32(configuration["APPSETTING_VALUE_CACHE_EXPIRED"].ToString());
                if (environment == LOCAL_NOKEYVAULT)
                {
                    if (configuration[secretName] == null)
                        throw new Exception("Seiu.Framework.Azure.KeyVault.KeyVaultClient.GetSecretValue, Can't find appsetting item:" + secretName);
                    else
                        return configuration[secretName];
                }
                else
                {
                    //if (configuration[secretName] == null)
                    //    throw new Exception("Seiu.Framework.Azure.KeyVault.KeyVaultClient.GetSecretValue, Can't find appsetting item:" + secretName);

                    var azureKeyVaultEndpoint = configuration["AZURE_KEYVAULT_ENDPOINT"];
                    var tenantId = configuration["AZURE_TENANT_ID"];
                    var clientId = configuration["AZURE_CLIENT_ID"];
                    var clientSecret = configuration["AZURE_CLIENT_SECRET"];
                    var credential = new ClientSecretCredential(tenantId, clientId, clientSecret);
                    var secretClient = new SecretClient(new Uri(azureKeyVaultEndpoint), credential);
                    var secret = secretClient.GetSecret(secretName).Value.Value;

                    return secret.ToString();
                }
            }
            else
            {
                //if (configuration[secretName] == null)
                //    throw new Exception("Seiu.Framework.Azure.KeyVault.KeyVaultClient.GetSecretValue, Can't find appsetting item:" + secretName);
                // azure app service access key vault using default managed identity
                cacheExpired = Convert.ToInt32(configuration["APPSETTING_VALUE_CACHE_EXPIRED"].ToString());
                var azureKeyVaultEndpoint = configuration["AZURE_KEYVAULT_ENDPOINT"];
                var secretClient = new SecretClient(
                    new Uri(azureKeyVaultEndpoint),
                    new DefaultAzureCredential());
                var secret = secretClient.GetSecret(secretName).Value.Value;

                //Console.WriteLine(string.Format("\nGet sqlConnectionString from KeyVault:\n{0}", secret.ToString()));
                return secret.ToString();
            }
        }
        private static string GetAppSettingValue(string appSettingName, out int cacheExpired)
        {
            var configuration = new ConfigurationBuilder()
                    .SetBasePath(appSettingFilePath)
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .Build();
            var environment = configuration["ENVIRONMENT"].ToUpper();

            if (environment == LOCAL_NOKEYVAULT
                || environment == LOCAL)
            {
                // using appsettings.Development.json for local or development
                configuration = new ConfigurationBuilder()
                        .SetBasePath(appSettingFilePath)
                        .AddJsonFile("appsettings.Development.json", optional: false, reloadOnChange: true)
                        .Build();
            }
            cacheExpired = 0;// Convert.ToInt32(configuration["APPSETTING_VALUE_CACHE_EXPIRED"].ToString());
            if (configuration[appSettingName] == null)
                return null;
            else
                return configuration[appSettingName];
        }
    }
}
