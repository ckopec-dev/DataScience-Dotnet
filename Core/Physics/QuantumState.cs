using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Physics
{
    // <summary>
    /// Represents a quantum state vector
    /// </summary>
    public class QuantumState
    {
        private readonly QuantumComplex[] _amplitudes;
        public int NumQubits { get; }
        public int Dimension => 1 << NumQubits; // 2^n

        public QuantumState(int numQubits)
        {
            NumQubits = numQubits;
            _amplitudes = new QuantumComplex[Dimension];
            // Initialize to |0...0⟩ state
            _amplitudes[0] = new QuantumComplex(1.0);
        }

        public QuantumState(QuantumComplex[] amplitudes)
        {
            if (!IsPowerOfTwo(amplitudes.Length))
                throw new ArgumentException("Amplitude array length must be a power of 2");

            NumQubits = (int)Math.Log2(amplitudes.Length);
            _amplitudes = new QuantumComplex[amplitudes.Length];
            Array.Copy(amplitudes, _amplitudes, amplitudes.Length);
            Normalize();
        }

        public QuantumComplex this[int index]
        {
            get => _amplitudes[index];
            set => _amplitudes[index] = value;
        }

        public QuantumComplex[] GetAmplitudes() => (QuantumComplex[])_amplitudes.Clone();

        /// <summary>
        /// Normalizes the quantum state so that the sum of squared magnitudes equals 1
        /// </summary>
        public void Normalize()
        {
            double norm = Math.Sqrt(_amplitudes.Sum(amp => amp.Magnitude * amp.Magnitude));
            if (norm > 0)
            {
                for (int i = 0; i < _amplitudes.Length; i++)
                {
                    _amplitudes[i] = (1.0 / norm) * _amplitudes[i];
                }
            }
        }

        /// <summary>
        /// Measures the quantum state, collapsing it to a classical state
        /// </summary>
        public int Measure(Random? random = null)
        {
            random ??= new Random();
            double r = random.NextDouble();
            double cumulative = 0;

            for (int i = 0; i < _amplitudes.Length; i++)
            {
                cumulative += _amplitudes[i].Magnitude * _amplitudes[i].Magnitude;
                if (r <= cumulative)
                {
                    // Collapse to measured state
                    for (int j = 0; j < _amplitudes.Length; j++)
                        _amplitudes[j] = new QuantumComplex(j == i ? 1.0 : 0.0);
                    return i;
                }
            }
            return _amplitudes.Length - 1;
        }

        /// <summary>
        /// Gets the probability of measuring a specific state
        /// </summary>
        public double GetProbability(int state)
        {
            if (state < 0 || state >= _amplitudes.Length)
                return 0;
            return _amplitudes[state].Magnitude * _amplitudes[state].Magnitude;
        }

        private static bool IsPowerOfTwo(int n) => n > 0 && (n & (n - 1)) == 0;

        public override string ToString()
        {
            var terms = new List<string>();
            for (int i = 0; i < _amplitudes.Length; i++)
            {
                if (_amplitudes[i].Magnitude > 1e-10)
                {
                    string binary = Convert.ToString(i, 2).PadLeft(NumQubits, '0');
                    terms.Add($"{_amplitudes[i]}|{binary}⟩");
                }
            }
            return string.Join(" + ", terms);
        }
    }
}
