
using DomainLayer.Managers.ConfigurationProviders.ConfigModels;
using DomainLayer.Managers.Models;
using System.Collections.Generic;

namespace DomainLayer.Managers.ConfigurationProviders.Mappers
{
    internal static class MapperDeviceServiceSettingsConfig
    {
        public static DeviceServiceSettings MapToWebExSettings(DeviceServiceSettingsConfig deviceServiceSettingsConfig)
        {
            var devices = new List<Device>();

            foreach (var deviceConfig in deviceServiceSettingsConfig.DeviceConfigs)
            {
                devices.Add(new Device(deviceConfig.Identifier, deviceConfig.Type, deviceConfig.OtaKey));
            }

            return new DeviceServiceSettings(
                deviceServiceSettingsConfig.BaseUrl,
                deviceServiceSettingsConfig.AccessToken,
                deviceServiceSettingsConfig.HttpProxyUrl,
                devices);
        }

    }
}
