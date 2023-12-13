namespace Rosalind
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string switchErr = "Switch missing or invalid.";
            
            // /Problem InputPath OutputPath
            if (args != null && args.Length == 3)
            {
                ProblemDomain.InputPath = args[1];
                ProblemDomain.OutputPath = args[2];

                switch (args[0].ToLower())
                {
                    // Algorithmic Heights
                    case "/fibo": AlgorithmicHeights.ProblemFIBO(); break;
                    case "/bins": AlgorithmicHeights.ProblemBINS(); break;
                    
                    default: Console.WriteLine(switchErr); break;
                }
            }
            else
            {
                Console.WriteLine(switchErr);
            }
        }
    }
}