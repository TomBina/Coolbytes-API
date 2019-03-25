using System;
using System.IO;
using CoolBytes.WebAPI.Services.Environment;
using Newtonsoft.Json;

namespace CoolBytes.WebAPI.Services.KeyVault
{
    public class KeyVaultSettingsFactory
    {
        public KeyVaultSettings Create()
        {
            if (File.Exists("keyvaultsettings.json"))
            {
                var settingsJson = File.ReadAllText("keyvaultsettings.json");
                var settings = JsonConvert.DeserializeObject<KeyVaultSettings>(settingsJson);
                return settings;
            }

            var environmentService = new EnvironmentService();
            var vault = environmentService.GetVariable("vault");
            var clientId = environmentService.GetVariable("clientid");
            var secret = environmentService.GetVariable("secret");


            if (vault == null || clientId == null || secret == null)
            {
                throw new InvalidOperationException("No keyvault information found. Add a keyvaultsettings.json file or environment variables");
            }

            return new KeyVaultSettings()
            {
                Vault = vault,
                ClientId = clientId,
                Secret = secret
            };
        }
    }
}
