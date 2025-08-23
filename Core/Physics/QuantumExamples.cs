using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Physics
{
    public class QuantumExamples
    {
        public static void Run()
        {
            Console.WriteLine("=== Quantum Mechanics Simulation ===\n");

            // Single qubit operations
            Console.WriteLine("1. Single Qubit Operations:");
            var singleQubit = new QuantumSystem(1);
            Console.WriteLine($"Initial state: {singleQubit}");

            QuantumGates.Hadamard(singleQubit.State, 0);
            Console.WriteLine($"After Hadamard: {singleQubit}");
            Console.WriteLine($"Probability |0⟩: {singleQubit.State.GetProbability(0):F3}");
            Console.WriteLine($"Probability |1⟩: {singleQubit.State.GetProbability(1):F3}\n");

            // Bell state creation
            Console.WriteLine("2. Bell State (Entanglement):");
            var bellSystem = new QuantumSystem(2);
            bellSystem.CreateBellState();
            Console.WriteLine($"Bell state: {bellSystem}");
            Console.WriteLine($"Von Neumann entropy: {bellSystem.VonNeumannEntropy():F3}\n");

            // GHZ state with 3 qubits
            Console.WriteLine("3. GHZ State (3-qubit entanglement):");
            var ghzSystem = new QuantumSystem(3);
            ghzSystem.CreateGHZState();
            Console.WriteLine($"GHZ state: {ghzSystem}");
            Console.WriteLine($"Von Neumann entropy: {ghzSystem.VonNeumannEntropy():F3}\n");

            // Quantum interference
            Console.WriteLine("4. Quantum Interference:");
            var interference = new QuantumSystem(1);
            QuantumGates.Hadamard(interference.State, 0);
            QuantumGates.Phase(interference.State, 0, Math.PI); // π phase
            QuantumGates.Hadamard(interference.State, 0);
            Console.WriteLine($"H-Phase(π)-H: {interference}");

            // Multiple measurements
            Console.WriteLine("\n5. Measurement Statistics:");
            var measureSystem = new QuantumSystem(2);
            measureSystem.CreateBellState();

            var counts = new Dictionary<string, int>();
            var random = new Random(42); // Fixed seed for reproducibility

            for (int i = 0; i < 1000; i++)
            {
                var copySystem = new QuantumSystem(measureSystem.State);
                string result = copySystem.MeasureAll(random);
                counts[result] = counts.GetValueOrDefault(result, 0) + 1;
            }

            Console.WriteLine("Bell state measurement results (1000 trials):");
            foreach (var kvp in counts.OrderBy(x => x.Key))
            {
                Console.WriteLine($"|{kvp.Key}⟩: {kvp.Value} times ({kvp.Value / 10.0:F1}%)");
            }
        }
    }
}
