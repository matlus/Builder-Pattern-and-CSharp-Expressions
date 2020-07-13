using System.Collections.Generic;
using System.Text;

namespace DomainLayer.Managers.Models
{
    public sealed class DeviceServiceSettings
    {
        public string BaseUrl { get; }
        public string AccessToken { get; }
        public string HttpProxyUrl { get; }
        public IEnumerable<Device> Devices { get; }

        public DeviceServiceSettings(string baseUrl, string accessToken, string httpProxyUrl, IEnumerable<Device> devices)
        {
            BaseUrl = baseUrl;
            AccessToken = accessToken;
            HttpProxyUrl = httpProxyUrl;
            Devices = devices;
        }

        public override string ToString()
        {
            var devicesAsString = new StringBuilder();
            
            foreach (var device in Devices)
            {
                devicesAsString.AppendLine($"\t{device.ToString()}");
            }

            return $"Baserl: {BaseUrl}\r\nAccessToken: {AccessToken}\r\nHttpPRoxyUrl: {HttpProxyUrl}\r\nDevices:\r\n{devicesAsString.ToString()}";
        }
    }
}
