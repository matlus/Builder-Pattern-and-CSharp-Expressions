using DomainLayer.Managers.ConfigurationProviders.ConfigModels;
using DomainLayer.Managers.ConfigurationProviders.Mappers;
using DomainLayer.Managers.ConfigurationProviders.Validators;
using DomainLayer.Managers.Models;
using DomainLayer.Managers.Validators;
using Microsoft.Extensions.Configuration;
using System;
using System.Diagnostics.CodeAnalysis;

namespace DomainLayer.Managers.ConfigurationProviders
{
    public class ConfigurationProvider
    {
        private readonly IConfigurationRoot _configurationRoot;

        public ConfigurationProvider()
        {
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddJsonFile("appsettings.json");
            LoadEnvironmentSpecificAppSettings(configurationBuilder);
            _configurationRoot = configurationBuilder.Build();
        }

        private static void LoadEnvironmentSpecificAppSettings(ConfigurationBuilder configurationBuilder)
        {
            var aspNetCoreEnvironment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            configurationBuilder.AddJsonFile($"appsettings.{aspNetCoreEnvironment}.json", optional: true);
        }

        [ExcludeFromCodeCoverage]
        internal ConfigurationProvider(IConfigurationRoot configurationRoot)
        {
            _configurationRoot = configurationRoot;
        }

        private string RetrieveConfigurationSettingValue(string key)
        {
            return _configurationRoot[key];
        }

        private string RetrieveConfigurationSettingValueOrNull(string key)
        {
            var value = RetrieveConfigurationSettingValue(key);
            switch (ValidatorString.DetermineNullEmptyOrWhiteSpaces(value))
            {
                case StringState.Null:
                case StringState.Empty:
                case StringState.WhiteSpaces:
                    return null;
                case StringState.Valid:
                default:
                    return value;
            }
        }

        private string GetHttpProxyUrl()
        {
            return RetrieveConfigurationSettingValueOrNull("AppSettings:HttpProxyUrl");
        }

        private DeviceServiceSettingsConfig GetDeviceServiceSettingsPreValidated()
        {
            const string deviceServiceKey = "DeviceService";
            var deviceServiceSettingsConfig = _configurationRoot.GetSection(deviceServiceKey).Get<DeviceServiceSettingsConfig>();

            if (deviceServiceSettingsConfig != null)
            {
                deviceServiceSettingsConfig.HttpProxyUrl = GetHttpProxyUrl();
                var deviceConfigs = _configurationRoot.GetSection($"{deviceServiceKey}:Devices").Get<DeviceConfig[]>();
                deviceServiceSettingsConfig.DeviceConfigs = deviceConfigs ?? Array.Empty<DeviceConfig>();
            }
            else
            {
                deviceServiceSettingsConfig = new DeviceServiceSettingsConfig
                {
                    DeviceConfigs = Array.Empty<DeviceConfig>()
                };
            }

            return deviceServiceSettingsConfig;
        }

        public DeviceServiceSettings GetDeviceServiceSettings()
        {
            var deviceServiceSettingsConfig = GetDeviceServiceSettingsPreValidated();
            ValidatorDeviceServiceSettingsConfig.Validate(deviceServiceSettingsConfig);
            return MapperDeviceServiceSettingsConfig.MapToWebExSettings(deviceServiceSettingsConfig);
        }
    }
}
