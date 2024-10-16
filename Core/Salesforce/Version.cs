
namespace Core.Salesforce
{
    public class Version(string label, string url, decimal version)
    {
        public string Label { get; set; } = label;
        public string Url { get; set; } = url;
        public decimal Value { get; set; } = version;
    }
}
