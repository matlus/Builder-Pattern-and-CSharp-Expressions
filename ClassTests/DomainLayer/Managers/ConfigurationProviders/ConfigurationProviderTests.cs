using DomainLayer.Managers.Exceptions;
using DomainLayer.Managers.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using Testing.Shared;
using Xunit;
using Xunit.Sdk;
using ConfigurationProvider = DomainLayer.Managers.ConfigurationProviders.ConfigurationProvider;

namespace ClassTests
{
    public class ConfigurationProviderTests
    {
        private const string ProxyUrl = "http://proxy.abcdeefghij.com:80";

        private static ConfigurationProvider InitializeConfigurationProvider(ConfigSettingsRoot configSettingsRoot)
        {
            var configurationRoot = new ConfigurationBuilder()
                .AddAsJsonRoot(configSettingsRoot)
                .Build();
            return new ConfigurationProvider(configurationRoot);
        }

        private static ConfigurationProvider InitializeConfigurationProvider(DeviceServiceSettings deviceServiceSettings, AppSettings appSettings)
        {
            var configSettingsRoot = new ConfigSettingsRoot
            {
                AppSettings = appSettings,
                DeviceService = deviceServiceSettings
            };

            return InitializeConfigurationProvider(configSettingsRoot);
        }

        public static IEnumerable<object[]> TestData()
        {

            yield return new object[]
            {
                new DeviceServiceSettingsBuilder().Set(x => x.BaseUrl, null).Build(),
                @"The property: ""DeviceServiceSettings.BaseUrl"" must be a valid DeviceServiceSettings.BaseUrl and can not be Empty
",
            };

            yield return new object[]
            {
                new DeviceServiceSettingsBuilder().Set(x => x.AccessToken, null).Build(),
                @"The property: ""DeviceServiceSettings.AccessToken"" must be a valid DeviceServiceSettings.AccessToken and can not be Empty
",
            };

            yield return new object[]
            {
                new DeviceServiceSettingsBuilder().Set(x => x.Devices, null).Build(),
                @"The property: ""DeviceServiceSettings.Devices"" collection is Empty. The collection must contain at least 1 item.
",
            };
        }

        [Theory]
        [MemberData(nameof(ConfigurationProviderTests.TestData))]
        internal void Test1(DeviceServiceSettings deviceServiceSettings, string expectedExceptionMessage)
        {
            var appSettings = new AppSettings { HttpProxyUrl = ProxyUrl };
            var configurationProvider = InitializeConfigurationProvider(deviceServiceSettings, appSettings);

            try
            {
                // Act
                _ = configurationProvider.GetDeviceServiceSettings();
                throw new XunitException("We were expectiing an Excption of type: ConfigurationSettingMissingException, but no exception was thrown");
            }
            catch (ConfigurationSettingMissingException e)
            {
                // Assert
                Assert.Equal(expectedExceptionMessage, e.Message);
            }
        }
    }
}
