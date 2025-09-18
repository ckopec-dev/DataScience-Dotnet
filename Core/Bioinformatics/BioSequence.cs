namespace Core.Bioinformatics
{
    public class BioSequence
    {
        public string Id { get; }
        public string Sequence { get; }

        public BioSequence(string? sequence, string? id = null)
        {
            ArgumentNullException.ThrowIfNull(sequence);
            Sequence = sequence.Trim();
            if (Sequence.Length == 0) throw new ArgumentException("Sequence must not be empty.", nameof(sequence));
            Id = id ?? string.Empty;
        }

        public override string ToString() => $">{Id}\n{Sequence}";
    }
}
