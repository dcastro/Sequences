using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Sequences
{
    /// <summary>
    /// Provides a set of static methods for creating instances of <see cref="Sequence{T}"/>.
    /// </summary>
    public static class Sequence
    {
        /// <summary>
        /// Creates an infinite sequence starting at <paramref name="start"/> and incrementing by <paramref name="step"/> in each step.
        /// </summary>
        /// <param name="start">The start value of the sequence.</param>
        /// <param name="step">The value to increment in each step.</param>
        /// <returns>A sequence starting at value <paramref name="start"/>.</returns>
        public static Sequence<int> From(int start, int step)
        {
            return new Sequence<int>(start, () => From(start + step, step));
        }

        /// <summary>
        /// Creates an infinite sequence starting at <paramref name="start"/> and incrementing by 1 in each step.
        /// </summary>
        /// <param name="start">The start value of the sequence.</param>
        /// <returns>A sequence starting at value <paramref name="start"/>.</returns>
        public static Sequence<int> From(int start)
        {
            return From(start, 1);
        }

        /// <summary>
        /// Creates an infinite sequence starting at <paramref name="start"/> and incrementing by <paramref name="step"/> in each step.
        /// </summary>
        /// <param name="start">The start value of the sequence.</param>
        /// <param name="step">The value to increment in each step.</param>
        /// <returns>A sequence starting at value <paramref name="start"/>.</returns>
        public static Sequence<long> From(long start, int step)
        {
            return new Sequence<long>(start, () => From(start + step, step));
        }

        /// <summary>
        /// Creates an infinite sequence starting at <paramref name="start"/> and incrementing by 1 in each step.
        /// </summary>
        /// <param name="start">The start value of the sequence.</param>
        /// <returns>A sequence starting at value <paramref name="start"/>.</returns>
        public static Sequence<long> From(long start)
        {
            return From(start, 1);
        }

        /// <summary>
        /// Creates an infinite sequence starting at <paramref name="start"/> and incrementing by <paramref name="step"/> in each step.
        /// </summary>
        /// <param name="start">The start value of the sequence.</param>
        /// <param name="step">The value to increment in each step.</param>
        /// <returns>A sequence starting at value <paramref name="start"/>.</returns>
        public static Sequence<BigInteger> From(BigInteger start, int step)
        {
            return new Sequence<BigInteger>(start, () => From(start + step, step));
        }

        /// <summary>
        /// Creates an infinite sequence starting at <paramref name="start"/> and incrementing by 1 in each step.
        /// </summary>
        /// <param name="start">The start value of the sequence.</param>
        /// <returns>A sequence starting at value <paramref name="start"/>.</returns>
        public static Sequence<BigInteger> From(BigInteger start)
        {
            return From(start, 1);
        }
    }
}
