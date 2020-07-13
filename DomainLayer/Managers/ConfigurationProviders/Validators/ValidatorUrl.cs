using DomainLayer.Managers.Validators;
using System;

namespace DomainLayer.Managers.ConfigurationProviders.Validators
{
    internal static class ValidatorUrl
    {
        public static (string url, string errorMessage) ValidateAndFix(string propertyName, string url)
        {
            var errorMessage = ValidatorString.Validate(propertyName, url);
            string fixedUrl = null;

            if (errorMessage == null)
            {
                fixedUrl = url.EndsWith("/", StringComparison.OrdinalIgnoreCase) ? url : url + "/";
            }

            return (fixedUrl, errorMessage);
        }
    }
}
