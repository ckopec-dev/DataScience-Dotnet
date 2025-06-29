
namespace Core.Bioinformatics
{
    public class FastaItem(string label, string data)
    {
        public string Label { get; set; } = label;
        public string Data { get; set; } = data;

        public override string ToString()
        {
            return Label + Environment.NewLine + Data;
        }
    }
}
