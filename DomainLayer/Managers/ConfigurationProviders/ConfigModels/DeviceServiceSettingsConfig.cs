using System.Collections.Generic;

namespace DomainLayer.Managers.ConfigurationProviders.ConfigModels
{
    internal sealed class DeviceServiceSettingsConfig
    {
        public string BaseUrl { get; set; }
        public string AccessToken { get; set; }
        public string HttpProxyUrl { get; set; }
        public IEnumerable<DeviceConfig> DeviceConfigs { get; set; }
    }
}
