
using Core;

namespace Rosalind
{
    public abstract class ProblemDomain
    {
        public static string? InputPath { get; set; }
        public static string? OutputPath { get; set; }

        public static int ReadInputToInt32()
        {
            if (InputPath != null)
                return Convert.ToInt32(File.ReadAllText(InputPath).Trim());
            else
                throw new InvalidInputException();
        }

        public static List<double[]> ReadInputToDoubleListArray()
        {
            if (InputPath != null)
                return File.ReadAllText(InputPath).Trim().ToDoubleListArray('\n', ' ');
            else
                throw new InvalidInputException();
        }

        public static void WriteOutput(string output)
        {
            WriteOutput(output, true);
        }

        public static void WriteOutput(string output, bool printToConsole)
        {
            if (OutputPath != null)
                File.WriteAllText(OutputPath, output);
            else
                throw new InvalidInputException();

            if (printToConsole)
                Console.WriteLine(output);
        }
    }
}
