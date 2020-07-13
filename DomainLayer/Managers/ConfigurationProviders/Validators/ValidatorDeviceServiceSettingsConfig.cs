using DomainLayer.Managers.ConfigurationProviders.ConfigModels;
using DomainLayer.Managers.Exceptions;
using DomainLayer.Managers.Validators;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DomainLayer.Managers.ConfigurationProviders.Validators
{
    internal static class ValidatorDeviceServiceSettingsConfig
    {
        public static void Validate(DeviceServiceSettingsConfig deviceServiceSettingsConfig)
        {
            var errorMessages = new StringBuilder();

            ValidateDeviceServiceSettingsConfig(errorMessages, deviceServiceSettingsConfig);
            ValidateNocMeetingConfigs(errorMessages, deviceServiceSettingsConfig.DeviceConfigs);

            if (errorMessages.Length != 0)
            {
                throw new ConfigurationSettingMissingException(errorMessages.ToString());
            }
        }

        private static void ValidateDeviceServiceSettingsConfig(StringBuilder errorMessages, DeviceServiceSettingsConfig deviceServiceSettingsConfig)
        {
            var (baseUrl, baseUrlErrorMessage) = ValidatorUrl.ValidateAndFix($"DeviceServiceSettings.{nameof(deviceServiceSettingsConfig.BaseUrl)}", deviceServiceSettingsConfig.BaseUrl);
            deviceServiceSettingsConfig.BaseUrl = baseUrl;
            errorMessages.AppendLineIfNotNull(baseUrlErrorMessage);
            errorMessages.AppendLineIfNotNull(ValidatorString.Validate($"DeviceServiceSettings.{nameof(deviceServiceSettingsConfig.AccessToken)}", deviceServiceSettingsConfig.AccessToken));

            if (!deviceServiceSettingsConfig.DeviceConfigs.Any())
            {
                errorMessages.AppendLine($"The property: \"DeviceServiceSettings.Devices\" collection is Empty. The collection must contain at least 1 item.");
            }
        }

        private static void ValidateNocMeetingConfigs(StringBuilder errorMessages, IEnumerable<DeviceConfig> deviceConfigs)
        {
            foreach (var deviceConfig in deviceConfigs)
            {
                errorMessages.AppendLineIfNotNull(ValidatorString.Validate($"Device.{nameof(deviceConfig.Identifier)}", deviceConfig.Identifier));
                errorMessages.AppendLineIfNotNull(ValidatorString.Validate($"Device.{nameof(deviceConfig.Type)}", deviceConfig.Type));
                errorMessages.AppendLineIfNotNull(ValidatorString.Validate($"Device.{nameof(deviceConfig.OtaKey)}", deviceConfig.OtaKey));
            }
        }
    }
}
