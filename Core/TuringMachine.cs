using System.Text;

namespace Core
{
    public class TuringMachine
    {
        public List<bool> Memory = [ false, false, false ];
        public object? Assembly = null;
        public int TapePosition = 1;

        public TuringMachine()
        {
            Reset();
        }

        public void Reset()
        {
            Memory = [false, false, false];
            TapePosition = 1;
        }

        public void Load(object assemblyCode)
        {
            // transitions + states
            throw new NotImplementedException();
        }

        public void Collect()
        {
            // shrink memory. all leading and trailing false values are implied
            throw new NotImplementedException();
        }

        public void Run(int steps)
        {
            throw new NotImplementedException();
        }

        public object Read()
        {
            // reads value at current position 
            throw new NotImplementedException();
        }

        public object Write()
        {
            // writes value at current position 
            throw new NotImplementedException();
        }

        public void Move(object direction)
        {
            // moves head left or right (direction)
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            // Returns a string representing the full state of the TM (all memory, etc.)

            StringBuilder sb = new();

            sb.AppendLine("### Current State ###");
            sb.AppendLine(String.Format("Memory Size: {0}", Memory.Count));
            sb.Append("Memory: ");
            for(int i = 0; i < Memory.Count; i++)
            {
                sb.Append(Memory[i] == true ? 1 : 0);
                if (i == Memory.Count - 1)
                    sb.Append(Environment.NewLine);
            }
            sb.AppendLine(String.Format("Tape position: {0}", TapePosition));

            return sb.ToString();
        }
    }
}
