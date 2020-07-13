using DomainLayer.Managers.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace BuilderPattern
{
    internal static class Program
    {
        public static void Main()
        {
            var deviceServiceSettings = new DeviceServiceSettingsBuilder()
                .Set(x => x.BaseUrl, null)
                .Build();

            Console.WriteLine(deviceServiceSettings.ToString());
        }
    }

    internal sealed class DeviceServiceSettingsBuilder
    {
        private readonly Dictionary<string, object> _propertiesToBuild = new Dictionary<string, object>();

        public DeviceServiceSettingsBuilder Set<T>(Expression<Func<DeviceServiceSettings, T>> expression, T value)
        {
            var propertyName = ((MemberExpression)expression.Body).Member.Name;
            _propertiesToBuild[propertyName] = value;
            return this;
        }
        public DeviceServiceSettings Build()
        {
            return new DeviceServiceSettings(
                baseUrl: _propertiesToBuild.TryGetValue(nameof(DeviceServiceSettings.BaseUrl), out var baseUrl) ? (string)baseUrl : RandomDataGenerator.BaseUrl(),
                accessToken: RandomDataGenerator.AccessToken(),
                httpProxyUrl: RandomDataGenerator.Url("proxy"),
                devices: GenerateDevices());
        }

        private IEnumerable<Device> GenerateDevices(int count = 0)
        {
            if (count == 0)
            {
                count = RandomDataGenerator.Integer(3, 5);
            }

            var devices = new List<Device>();

            for (int i = 0; i < count; i++)
            {
                devices.Add(new Device(
                    identifier: RandomDataGenerator.AlphaString(12),
                    type: RandomDataGenerator.AlphaString(8),
                    otaKey: RandomDataGenerator.AccessToken()));
            }

            return devices;
        }

    }
}
























    ////private IEnumerable<Device> GenerateDevices(int count = 0)
    ////{
    ////    if (count == 0)
    ////    {
    ////        count = RandomDataGenerator.Integer(3, 5);
    ////    }

    ////    var devices = new List<Device>();

    ////    for (int i = 0; i < count; i++)
    ////    {
    ////        devices.Add(new Device(
    ////            identifier: RandomDataGenerator.AlphaString(12),
    ////            type: RandomDataGenerator.AlphaString(8),
    ////            otaKey: RandomDataGenerator.AccessToken()));
    ////    }

    ////    return devices;
    ////}
