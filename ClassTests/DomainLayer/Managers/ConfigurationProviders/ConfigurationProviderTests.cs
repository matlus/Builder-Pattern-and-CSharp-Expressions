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

        public static IEnumerable<object[]> MemberDataForWhenAnyPropertyIsNull()
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

            yield return new object[]
            {
                new DeviceServiceSettingsBuilder()
                .Set(x => x.BaseUrl, null)
                .Set(x => x.AccessToken, null)
                .Set(x => x.HttpProxyUrl, null)
                .Set(x => x.Devices, null)
                .Build(),
                @"The property: ""DeviceServiceSettings.BaseUrl"" must be a valid DeviceServiceSettings.BaseUrl and can not be Empty
The property: ""DeviceServiceSettings.AccessToken"" must be a valid DeviceServiceSettings.AccessToken and can not be Empty
The property: ""DeviceServiceSettings.Devices"" collection is Empty. The collection must contain at least 1 item.
",
            };
        }

        [Fact]
        [Trait("Class Test", "")]
        public void GetDeviceServiceSettings_WhenAllSeetingsArePresentAndCorrect_ShouldReturnExpectedSettings()
        {
            // Arrange
            var generatedWebExSettings = new DeviceServiceSettingsBuilder().Build();
            var appSettings = new AppSettings { HttpProxyUrl = ProxyUrl };
            var configurationProvider = InitializeConfigurationProvider(generatedWebExSettings, appSettings);

            var expectedWebExSettings = new DeviceServiceSettingsBuilder()
                .Set(x => x.HttpProxyUrl, appSettings.HttpProxyUrl)
                .With(generatedWebExSettings);

            // Act
            var actualDeviceServiceSettings = configurationProvider.GetDeviceServiceSettings();

            // Assert
            ObjectComparer.AssertAreEqual(expectedWebExSettings, actualDeviceServiceSettings);
        }

        [Fact]
        [Trait("Class Test", "")]
        public void GetDeviceServiceSettings_WhenBaseUrlDoesNotEndWithForwardSlash_ShouldReturnBaseUrlEndingWithForwardSlash()
        {
            // Arrange
            var baseUrl = "http://api.matlus.com";
            var expectedBaseUrl = baseUrl + "/";
            var generatedDeviceServiceSettings = new DeviceServiceSettingsBuilder()
                .Set(x => x.BaseUrl, baseUrl)
                .Build();

            var appSettings = new AppSettings { HttpProxyUrl = ProxyUrl };
            var configurationProvider = InitializeConfigurationProvider(generatedDeviceServiceSettings, appSettings);

            var expectedDeviceServiceSettings = new DeviceServiceSettingsBuilder()
                .Set(x => x.BaseUrl, expectedBaseUrl)
                .Set(x => x.HttpProxyUrl, appSettings.HttpProxyUrl)
                .With(generatedDeviceServiceSettings);

            // Act
            var actualDeviceServiceSettings = configurationProvider.GetDeviceServiceSettings();

            // Assert
            ObjectComparer.AssertAreEqual(expectedDeviceServiceSettings, actualDeviceServiceSettings);
        }

        [Fact]
        [Trait("Class Test", "")]
        public void GetDeviceServiceSettings_WhenHttpProxyUrlDoesNotEndWithForwardSlash_ShouldReturnHttpProxyUrlAsIs()
        {
            // Arrange
            var expectedHttpProxyUrl = "http://proxy.outbound.matlus.com";
            var generatedDeviceServiceSettings = new DeviceServiceSettingsBuilder()
                .Build();

            var appSettings = new AppSettings { HttpProxyUrl = expectedHttpProxyUrl };
            var configurationProvider = InitializeConfigurationProvider(generatedDeviceServiceSettings, appSettings);

            var expectedDeviceServiceSettings = new DeviceServiceSettingsBuilder()
                .Set(x => x.HttpProxyUrl, expectedHttpProxyUrl)
                .With(generatedDeviceServiceSettings);

            // Act
            var actualDeviceServiceSettings = configurationProvider.GetDeviceServiceSettings();

            // Assert
            ObjectComparer.AssertAreEqual(expectedDeviceServiceSettings, actualDeviceServiceSettings);
        }

        [Theory]
        [Trait("Class Test", "")]
        [MemberData(nameof(ConfigurationProviderTests.MemberDataForWhenAnyPropertyIsNull))]
        internal void GetDeviceServiceSettings_WhenAnyPropertyIsNull_ShouldThrow(DeviceServiceSettings deviceServiceSettings, string expectedExceptionMessage)
        {
            var appSettings = new AppSettings { HttpProxyUrl = ProxyUrl };
            var configurationProvider = InitializeConfigurationProvider(deviceServiceSettings, appSettings);

            try
            {
                // Act
                _ = configurationProvider.GetDeviceServiceSettings();
                throw new XunitException("We were expecting an Excption of type: ConfigurationSettingMissingException, but no exception was thrown");
            }
            catch (ConfigurationSettingMissingException e)
            {
                // Assert
                Assert.Equal(expectedExceptionMessage, e.Message);
            }
        }
    }
}
