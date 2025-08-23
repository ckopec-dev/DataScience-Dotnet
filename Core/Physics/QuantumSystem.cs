using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Physics
{
    /// <summary>
    /// Main quantum system class
    /// </summary>
    public class QuantumSystem
    {
        public QuantumState State { get; private set; }
        public int NumQubits => State.NumQubits;

        public QuantumSystem(int numQubits)
        {
            State = new QuantumState(numQubits);
        }

        public QuantumSystem(QuantumState initialState)
        {
            State = initialState;
        }

        /// <summary>
        /// Creates a Bell state (maximally entangled state)
        /// </summary>
        public void CreateBellState()
        {
            if (NumQubits < 2)
                throw new InvalidOperationException("Need at least 2 qubits for Bell state");

            // Reset to |00⟩
            State = new QuantumState(NumQubits);

            // Apply H to first qubit, then CNOT
            QuantumGates.Hadamard(State, 0);
            QuantumGates.CNOT(State, 0, 1);
        }

        /// <summary>
        /// Creates a GHZ state (generalized Bell state for n qubits)
        /// </summary>
        public void CreateGHZState()
        {
            // Reset to |00...0⟩
            State = new QuantumState(NumQubits);

            // Apply H to first qubit
            QuantumGates.Hadamard(State, 0);

            // Apply CNOT from first qubit to all others
            for (int i = 1; i < NumQubits; i++)
            {
                QuantumGates.CNOT(State, 0, i);
            }
        }

        /// <summary>
        /// Calculates quantum fidelity between this state and another
        /// </summary>
        public double Fidelity(QuantumSystem other)
        {
            if (NumQubits != other.NumQubits)
                throw new ArgumentException("States must have same number of qubits");

            QuantumComplex overlap = new(0);
            for (int i = 0; i < State.Dimension; i++)
            {
                overlap += (State[i].Conjugate * other.State[i]);
            }
            return overlap.Magnitude * overlap.Magnitude;
        }

        /// <summary>
        /// Calculates von Neumann entropy (measure of entanglement)
        /// </summary>
        public double VonNeumannEntropy()
        {
            double entropy = 0;
            for (int i = 0; i < State.Dimension; i++)
            {
                double prob = State.GetProbability(i);
                if (prob > 1e-15)
                {
                    entropy -= prob * Math.Log2(prob);
                }
            }
            return entropy;
        }

        /// <summary>
        /// Measures all qubits and returns the result
        /// </summary>
        public string MeasureAll(Random? random = null)
        {
            int result = State.Measure(random);
            return Convert.ToString(result, 2).PadLeft(NumQubits, '0');
        }

        public override string ToString() => State.ToString();
    }
}
