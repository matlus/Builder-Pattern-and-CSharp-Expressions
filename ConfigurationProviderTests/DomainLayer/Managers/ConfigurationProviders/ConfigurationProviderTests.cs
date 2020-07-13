using DomainLayer.Managers.Exceptions;
using DomainLayer.Managers.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Testing.Shared;
using ConfigurationProvider = DomainLayer.Managers.ConfigurationProviders.ConfigurationProvider;

namespace ConfigurationProviderTests
{
    [TestCategory("Class Test")]
    [TestClass]
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

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
        public void GetDeviceServiceSettings_WhenAllPropertiesAreNull_ShouldThrowWithMessageMentioningAllProperties()
        {
            // Arrange  
            var expectedExceptionMessage =
@"The property: ""DeviceServiceSettings.BaseUrl"" must be a valid DeviceServiceSettings.BaseUrl and can not be Empty
The property: ""DeviceServiceSettings.AccessToken"" must be a valid DeviceServiceSettings.AccessToken and can not be Empty
The property: ""DeviceServiceSettings.Devices"" collection is Empty. The collection must contain at least 1 item.
";
            var deviceServiceSettings = new DeviceServiceSettingsBuilder()
                .BuildWithDefaults();

            var appSettings = new AppSettings { HttpProxyUrl = ProxyUrl };
            var configurationProvider = InitializeConfigurationProvider(deviceServiceSettings, appSettings);

            try
            {
                // Act
                _ = configurationProvider.GetDeviceServiceSettings();
                Assert.Fail("We were expectiing an Excption of type: ConfigurationSettingMissingException, but no exception was thrown");
            }
            catch (ConfigurationSettingMissingException e)
            {
                // Assert
                Assert.AreEqual(expectedExceptionMessage, e.Message);
            }
        }

        [DataTestMethod]
        [DataRow()]
        [TestMethod]
        public void GetDeviceServiceSettings_WhenBaseUrlIsNull_ShouldThrow()
        {
            // Arrange  
            var expectedExceptionMessage =
@"The property: ""DeviceServiceSettings.BaseUrl"" must be a valid DeviceServiceSettings.BaseUrl and can not be Empty
";
            var deviceServiceSettings = new DeviceServiceSettingsBuilder()
                .Set(x => x.BaseUrl, null)
                .Build();

            var appSettings = new AppSettings { HttpProxyUrl = ProxyUrl };
            var configurationProvider = InitializeConfigurationProvider(deviceServiceSettings, appSettings);

            try
            {
                // Act
                _ = configurationProvider.GetDeviceServiceSettings();
                Assert.Fail("We were expectiing an Excption of type: ConfigurationSettingMissingException, but no exception was thrown");
            }
            catch (ConfigurationSettingMissingException e)
            {
                // Assert
                Assert.AreEqual(expectedExceptionMessage, e.Message);
            }
        }

        [TestMethod]
        public void GetDeviceServiceSettings_WhenAccessTokenIsNull_ShouldThrow()
        {
            // Arrange  
            var expectedExceptionMessage =
@"The property: ""DeviceServiceSettings.AccessToken"" must be a valid DeviceServiceSettings.AccessToken and can not be Empty
";
            var deviceServiceSettings = new DeviceServiceSettingsBuilder()
                .Set(x => x.AccessToken, null)
                .Build();

            var appSettings = new AppSettings { HttpProxyUrl = ProxyUrl };
            var configurationProvider = InitializeConfigurationProvider(deviceServiceSettings, appSettings);

            try
            {
                // Act
                _ = configurationProvider.GetDeviceServiceSettings();
                Assert.Fail("We were expectiing an Excption of type: ConfigurationSettingMissingException, but no exception was thrown");
            }
            catch (ConfigurationSettingMissingException e)
            {
                // Assert
                Assert.AreEqual(expectedExceptionMessage, e.Message);
            }
        }

        [TestMethod]
        public void GetDeviceServiceSettings_WhenDevicesNull_ShouldThrowWithMessageSayingDevicesIsEmpty()
        {
            // Arrange  
            var expectedExceptionMessage =
@"The property: ""DeviceServiceSettings.Devices"" collection is Empty. The collection must contain at least 1 item.
";
            var deviceServiceSettings = new DeviceServiceSettingsBuilder()
                .Set(x => x.Devices, null)
                .Build();

            var appSettings = new AppSettings { HttpProxyUrl = ProxyUrl };
            var configurationProvider = InitializeConfigurationProvider(deviceServiceSettings, appSettings);

            try
            {
                // Act
                _ = configurationProvider.GetDeviceServiceSettings();
                Assert.Fail("We were expectiing an Excption of type: ConfigurationSettingMissingException, but no exception was thrown");
            }
            catch (ConfigurationSettingMissingException e)
            {
                // Assert
                Assert.AreEqual(expectedExceptionMessage, e.Message);
            }
        }
    }
}
