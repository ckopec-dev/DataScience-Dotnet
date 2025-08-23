
namespace Core.Physics
{
    /// <summary>
    /// Represents quantum gates and operations
    /// </summary>
    public static class QuantumGates
    {
        /// <summary>
        /// Pauli-X gate (NOT gate) - flips qubit state
        /// </summary>
        public static void PauliX(QuantumState state, int qubit)
        {
            ApplySingleQubitGate(state, qubit, new QuantumComplex[,]
            {
                { new QuantumComplex(0), new QuantumComplex(1) },
                { new QuantumComplex(1), new QuantumComplex(0) }
            });
        }

        /// <summary>
        /// Pauli-Y gate
        /// </summary>
        public static void PauliY(QuantumState state, int qubit)
        {
            ApplySingleQubitGate(state, qubit, new QuantumComplex[,]
            {
                { new QuantumComplex(0), new QuantumComplex(0, -1) },
                { new QuantumComplex(0, 1), new QuantumComplex(0) }
            });
        }

        /// <summary>
        /// Pauli-Z gate - applies phase flip
        /// </summary>
        public static void PauliZ(QuantumState state, int qubit)
        {
            ApplySingleQubitGate(state, qubit, new QuantumComplex[,]
            {
                { new QuantumComplex(1), new QuantumComplex(0) },
                { new QuantumComplex(0), new QuantumComplex(-1) }
            });
        }

        /// <summary>
        /// Hadamard gate - creates superposition
        /// </summary>
        public static void Hadamard(QuantumState state, int qubit)
        {
            double sqrt2 = 1.0 / Math.Sqrt(2);
            ApplySingleQubitGate(state, qubit, new QuantumComplex[,]
            {
                { new QuantumComplex(sqrt2), new QuantumComplex(sqrt2) },
                { new QuantumComplex(sqrt2), new QuantumComplex(-sqrt2) }
            });
        }

        /// <summary>
        /// Phase gate - applies phase rotation
        /// </summary>
        public static void Phase(QuantumState state, int qubit, double angle)
        {
            ApplySingleQubitGate(state, qubit, new QuantumComplex[,]
            {
                { new QuantumComplex(1), new QuantumComplex(0) },
                { new QuantumComplex(0), new QuantumComplex(Math.Cos(angle), Math.Sin(angle)) }
            });
        }

        /// <summary>
        /// CNOT gate - controlled NOT operation
        /// </summary>
        public static void CNOT(QuantumState state, int control, int target)
        {
            var newAmplitudes = new QuantumComplex[state.Dimension];

            for (int i = 0; i < state.Dimension; i++)
            {
                bool controlBit = ((i >> control) & 1) == 1;
                if (controlBit)
                {
                    // Flip target bit
                    int newIndex = i ^ (1 << target);
                    newAmplitudes[newIndex] = state[i];
                }
                else
                {
                    newAmplitudes[i] = state[i];
                }
            }

            for (int i = 0; i < state.Dimension; i++)
            {
                state[i] = newAmplitudes[i];
            }
        }

        private static void ApplySingleQubitGate(QuantumState state, int qubit, QuantumComplex[,] gate)
        {
            var newAmplitudes = new QuantumComplex[state.Dimension];

            for (int i = 0; i < state.Dimension; i++)
            {
                bool qubitState = ((i >> qubit) & 1) == 1;
                int baseIndex = i & ~(1 << qubit); // Clear the qubit bit

                if (qubitState) // |1⟩
                {
                    newAmplitudes[i] = gate[1, 0] * state[baseIndex] + gate[1, 1] * state[baseIndex | (1 << qubit)];
                }
                else // |0⟩
                {
                    newAmplitudes[i] = gate[0, 0] * state[baseIndex] + gate[0, 1] * state[baseIndex | (1 << qubit)];
                }
            }

            for (int i = 0; i < state.Dimension; i++)
            {
                state[i] = newAmplitudes[i];
            }
        }
    }
}
