using DomainLayer.Managers.Models;

namespace ConfigurationProviderTests
{
    internal class ConfigSettingsRoot
    {
        public AppSettings AppSettings { get; set; }
        public DeviceServiceSettings DeviceService { get; set; }
    }

    public class AppSettings
    {
        public string HttpProxyUrl { get; set; }
    }
}
