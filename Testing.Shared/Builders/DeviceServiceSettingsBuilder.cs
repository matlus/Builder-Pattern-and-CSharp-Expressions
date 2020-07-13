using DomainLayer.Managers.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Testing.Shared
{
    internal sealed class DeviceServiceSettingsBuilder
    {
        private readonly Dictionary<string, object> _propertiesToBuild = new Dictionary<string, object>();

        public DeviceServiceSettingsBuilder Set<T>(Expression<Func<DeviceServiceSettings, T>> propertyNameExpression, T value)
        {
            var memberExpression = (MemberExpression)propertyNameExpression.Body;
            var propertyName = memberExpression.Member.Name;
            _propertiesToBuild[propertyName] = value;
            return this;
        }

        public DeviceServiceSettings Build()
        {
            return new DeviceServiceSettings(
                baseUrl: GetPropertyValue(nameof(DeviceServiceSettings.BaseUrl), RandomDataGenerator.BaseUrl()),
                accessToken: GetPropertyValue(nameof(DeviceServiceSettings.AccessToken), RandomDataGenerator.AccessToken()),
                httpProxyUrl: GetPropertyValue(nameof(DeviceServiceSettings.HttpProxyUrl), RandomDataGenerator.Url(subdomain: "proxy")),
                devices: GetPropertyValue(nameof(DeviceServiceSettings.Devices), GenerateDevices()));
        }

        public DeviceServiceSettings With(DeviceServiceSettings deviceServiceSettings)
        {
            return new DeviceServiceSettings(
                baseUrl: GetPropertyValue(nameof(DeviceServiceSettings.BaseUrl), deviceServiceSettings.BaseUrl),
                accessToken: GetPropertyValue(nameof(DeviceServiceSettings.AccessToken), deviceServiceSettings.AccessToken),
                httpProxyUrl: GetPropertyValue(nameof(DeviceServiceSettings.HttpProxyUrl), deviceServiceSettings.HttpProxyUrl),
                devices: GetPropertyValue(nameof(DeviceServiceSettings.Devices), deviceServiceSettings.Devices));
        }

        public DeviceServiceSettings BuildWithDefaults()
        {
            return new DeviceServiceSettings(
                baseUrl: GetPropertyValue<string>(nameof(DeviceServiceSettings.BaseUrl), default),
                accessToken: GetPropertyValue<string>(nameof(DeviceServiceSettings.AccessToken), default),
                httpProxyUrl: GetPropertyValue<string>(nameof(DeviceServiceSettings.HttpProxyUrl), default),
                devices: GetPropertyValue<IEnumerable<Device>>(nameof(DeviceServiceSettings.Devices), default));
        }

        private T GetPropertyValue<T>(string propertyName, T defaultValue)
        {
            return _propertiesToBuild.TryGetValue(propertyName, out var value) ? (T)value : defaultValue;
        }

        private static IEnumerable<Device> GenerateDevices(int count = 0)
        {
            if (count == 0)
            {
                count = RandomDataGenerator.Integer(3, 5);
            }

            var deviceBuilder = new DeviceBuilder();

            var devices = new List<Device>();

            for (int i = 0; i < count; i++)
            {
                devices.Add(deviceBuilder.Build());
            }

            return devices;
        }
    }
}
