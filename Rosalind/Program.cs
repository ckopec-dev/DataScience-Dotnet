using Core;
using QuikGraph.Algorithms.Search;

namespace Rosalind
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string switchErr = "Switch missing or invalid.";
            
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
                    case "/maj": AlgorithmicHeights.ProblemMAJ(); break;
                    case "/mer": AlgorithmicHeights.ProblemMER(); break;
                    case "/2sum": AlgorithmicHeights.Problem2SUM(); break;
                    case "/bfs": AlgorithmicHeights.ProblemBFS(); break;
                    case "/cc": AlgorithmicHeights.ProblemCC(); break;

                    // Bioinformatics Stronghold
                    case "/dna": BioinformaticsStronghold.ProblemDNA(); break;
                    case "/rna": BioinformaticsStronghold.ProblemRNA(); break;
                    case "/revc": BioinformaticsStronghold.ProblemREVC(); break;
                    case "/fib": BioinformaticsStronghold.ProblemFIB(); break;
                    case "/gc": BioinformaticsStronghold.ProblemGC(); break;
                    case "/hamm": BioinformaticsStronghold.ProblemHAMM(); break;
                    case "/iprb": BioinformaticsStronghold.ProblemIPRB(); break;
                    case "/prot": BioinformaticsStronghold.ProblemPROT(); break;
                    case "/subs": BioinformaticsStronghold.ProblemSUBS(); break;
                    case "/cons": BioinformaticsStronghold.ProblemCONS(); break;

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