namespace Rosalind
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string switchErr = "Switch missing or invalid.";
            
            // /Problem InputPath OutputPath
            if (args != null && args.Length == 1)
            {
                switch (args[0].ToLower())
                {
                    // Algorithmic Heights
                    case "/fibo": AlgorithmicHeights.ProblemFIBO(); break;
                    case "/bins": AlgorithmicHeights.ProblemBINS(); break;
                    case "/deg": AlgorithmicHeights.ProblemDEG(); break;
                    case "/ins": AlgorithmicHeights.ProblemINS(); break;
                    case "/ddeg": AlgorithmicHeights.ProblemDDEG(); break;

                    // Bioinformatics Armory
                    case "/ini": BioinformaticsArmory.ProblemINI(); break;

                    // Bioinformatics Stronghold
                    case "/dna": BioinformaticsStronghold.ProblemDNA(); break;

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