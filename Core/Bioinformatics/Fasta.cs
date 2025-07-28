
using System.Text;

namespace Core.Bioinformatics
{
    /// <summary>
    /// DNA strings must be labeled when they are consolidated into a database. 
    /// A commonly used method of string labeling is called FASTA format. In this format, the string is introduced by a line that 
    /// begins with '>', followed by some labeling information. Subsequent lines contain the string itself; 
    /// the first line to begin with '>' indicates the label of the next string.
    /// </summary>
    public class Fasta(string rawInput)
    {
        #region Fields

        private readonly string _RawInput = rawInput;
        private readonly List<FastaEntry> _Entries = [];

        #endregion

        #region Properties

        public string? RawInput
        {
            get { return _RawInput; }
        }

        public List<FastaEntry> Entries
        {
            get
            {
                if (_Entries.Count == 0)
                {
                    if (!_RawInput.Contains('>'))
                    {
                        return _Entries;
                    }
                        
                    ProcessEntry(_RawInput);
                }

                return _Entries;
            }
        }

        #endregion

        #region Methods

        public override string? ToString()
        {
            return _RawInput;
        }

        private void ProcessEntry(string input)
        {
            // Look for first line starting with a ">". Everything on this line (except the ">") is the label. 
            // Look for the next line starting with a ">". 
            // If it doesn't exist, everything remaining is the data.
            // If it does exist, everything up to that line is the data. Everything after (and including) that line is a new entry to process.

            //Console.WriteLine("Processing entry: \n{0}", input); // for debugging

            StringBuilder data = new();
            bool firstTokenFound = false;
            StringBuilder nextBuffer = new();
            string label = "INVALID";

            using (StringReader sr = new(input))
            {
                string? buffer;

                while ((buffer = sr.ReadLine()) != null)
                {
                    if (buffer.StartsWith('>'))
                    {
                        if (firstTokenFound == false)
                        {
                            label = buffer[1..];
                            firstTokenFound = true;
                        }
                        else
                        {
                            nextBuffer.AppendLine(buffer);
                            nextBuffer.AppendLine(sr.ReadToEnd());
                        }
                    }
                    else
                    {
                        data.Append(buffer);
                    }
                }

                sr.Close();
            }

            _Entries.Add(new(label, data.ToString()));

            if (!String.IsNullOrWhiteSpace(nextBuffer.ToString()))
            {
                ProcessEntry(nextBuffer.ToString());
            }
        }

        #endregion
    }
}
