using DomainLayer.Managers.Models;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("ConfigurationProviderTests")]
[assembly: InternalsVisibleTo("ClassTests")]
namespace Testing.Shared
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
