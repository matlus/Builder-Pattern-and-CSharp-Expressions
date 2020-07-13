namespace DomainLayer.Managers.Models
{
    public sealed class Device
    {
        public string Identifier { get; }
        public string Type { get; }
        public string OtaKey { get; }

        public Device(string identifier, string type, string otaKey)
        {
            Identifier = identifier;
            Type = type;
            OtaKey = otaKey;
        }

        public override string ToString()
        {
            return $"Itendifier: {Identifier}, Type: {Type}, OtaKey: {OtaKey}";
        }
    }
}
