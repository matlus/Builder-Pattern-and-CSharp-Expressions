using DomainLayer.Managers.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Testing.Shared
{
    internal sealed class DeviceBuilder
    {
        private readonly Dictionary<string, object> _propertiesToBuild = new Dictionary<string, object>();

        public DeviceBuilder Set<T>(Expression<Func<Device, T>> propertyNameExpression, T value)
        {
            var memberExpression = (MemberExpression)propertyNameExpression.Body;
            var propertyName = memberExpression.Member.Name;
            _propertiesToBuild[propertyName] = value;
            return this;
        }

        public Device Build()
        {
            return new Device(
              identifier: GetPropertyValue(nameof(Device.Identifier), RandomDataGenerator.AlphaString(12)),
              type: GetPropertyValue(nameof(Device.Type), RandomDataGenerator.AlphaString(8)),
              otaKey: GetPropertyValue(nameof(Device.OtaKey), RandomDataGenerator.AccessToken()));
        }

        public Device BuildWith(Device device)
        {
            return new Device(
              identifier: GetPropertyValue(nameof(Device.Identifier), device.Identifier),
              type: GetPropertyValue(nameof(Device.Type), device.Type),
              otaKey: GetPropertyValue(nameof(Device.OtaKey), device.OtaKey));
        }

        private T GetPropertyValue<T>(string propertyName, T defaultValue)
        {
            return _propertiesToBuild.TryGetValue(propertyName, out var value) ? (T)value : defaultValue;
        }
    }
}
