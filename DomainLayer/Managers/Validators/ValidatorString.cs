
namespace DomainLayer.Managers.Validators
{
    internal enum StringState { Null, Empty, WhiteSpaces, Valid }

    internal static class ValidatorString
    {
        public static string Validate(string propertyName, string propertyValue)
        {
            switch (DetermineNullEmptyOrWhiteSpaces(propertyValue))
            {
                case StringState.Null:
                    return $"The property: \"{propertyName}\" must be a valid {propertyName} and can not be null";
                case StringState.Empty:
                    return $"The property: \"{propertyName}\" must be a valid {propertyName} and can not be Empty";
                case StringState.WhiteSpaces:
                    return $"The property: \"{propertyName}\" must be a valid {propertyName} and can not be Whitespaces";
                case StringState.Valid:
                default:
                    return null;
            }
        }

        public static StringState DetermineNullEmptyOrWhiteSpaces(string data)
        {
            if (data == null)
            {
                return StringState.Null;
            }
            else if (data.Length == 0)
            {
                return StringState.Empty;
            }

            foreach (var chr in data)
            {
                if (!char.IsWhiteSpace(chr))
                {
                    return StringState.Valid;
                }
            }

            return StringState.WhiteSpaces;
        }
    }
}
