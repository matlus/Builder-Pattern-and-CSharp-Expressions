using DomainLayer.Managers.Models;
using KellermanSoftware.CompareNetObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;

namespace Testing.Shared
{
    internal static class ObjectComparer
    {
        private static readonly CompareLogic s_compareLogic = new CompareLogic(new ComparisonConfig { MaxDifferences = 100, CompareChildren = true, AutoClearCache = false });

        public static void AssertAreEqual<T>(T expected, T actual)
        {
            var typeName = typeof(DeviceServiceSettings).Name;

            var comparisonResult = s_compareLogic.Compare(expected, actual);
            if (comparisonResult.AreEqual)
            {
                return;
            }

            var errorMessages = new StringBuilder();
            var firstDifference = comparisonResult.Differences[0];
            errorMessages.AppendLine($"The Expected {firstDifference.ParentObject1.GetType().Name} and Actual {firstDifference.ParentObject2.GetType().Name}, are not the same. There are {comparisonResult.Differences.Count} Differences.");

            foreach (var difference in comparisonResult.Differences)
            {
                difference.ExpectedName = typeName;
                difference.ActualName = typeName;
                errorMessages.AppendLine($"The Expected {difference.ExpectedName}.{difference.PropertyName}: {difference.Object1Value} != Actual {difference.ActualName}.{difference.PropertyName}: {difference.Object1Value}");
            }

            throw new AssertFailedException(errorMessages.ToString());
        }
    }
}
