using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

namespace Sequences
{
    /// <summary>
    /// Provides a set of static methods for creating instances of <see cref="Sequence{T}"/>.
    /// </summary>
    public static class Sequence
    {
        /// <summary>
        /// Create an infinite sequence containing the given element expression.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <param name="elem">The element to be continuously repeated.</param>
        /// <returns>A sequence containing an infinite number of <paramref name="elem"/></returns>
        public static Sequence<T> Continually<T>(T elem)
        {
            return new Sequence<T>(elem, () => Continually(elem));
        }

        /// <summary>
        /// Create an infinite sequence containing the given element expression (which is computed for each occurrence).
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <param name="elemFunc">A delegate that will be continuously evaluated.</param>
        /// <returns>A sequence containing an infinite number of elements returned by the <paramref name="elemFunc"/> delegate.</returns>
        public static Sequence<T> Continually<T>(Func<T> elemFunc)
        {
            return new Sequence<T>(elemFunc(), () => Continually(elemFunc));
        }

        /// <summary>
        /// Creates an infinite sequence starting at <paramref name="start"/> and incrementing by <paramref name="step"/> in each step.
        /// </summary>
        /// <param name="start">The start value of the sequence.</param>
        /// <param name="step">The value to increment in each step (positive or negative).</param>
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
        /// <param name="step">The value to increment in each step (positive or negative).</param>
        /// <returns>A sequence starting at value <paramref name="start"/>.</returns>
        public static Sequence<long> From(long start, long step)
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
        /// <param name="step">The value to increment in each step (positive or negative).</param>
        /// <returns>A sequence starting at value <paramref name="start"/>.</returns>
        public static Sequence<BigInteger> From(BigInteger start, BigInteger step)
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

        /// <summary>
        /// Creates a sequence containing equally spaced values in some integer interval.
        /// </summary>
        /// <param name="start">The start value of the collection.</param>
        /// <param name="end">The exclusive upper bound.</param>
        /// <param name="step">The value to increment in each step (positive or negative).</param>
        /// <returns>A collection with values <paramref name="start"/>, <paramref name="start"/> + <paramref name="step"/>, ...
        /// up to, but excluding <paramref name="end"/>.</returns>
        public static Sequence<int> Range(int start, int end, int step)
        {
            if ((step > 0 && start >= end) ||
                (step <= 0 && start <= end))
                return Sequence<int>.Empty;

            return new Sequence<int>(start, () => Range(start + step, end, step));
        }

        /// <summary>
        /// Creates a sequence containing equally spaced values in some integer interval.
        /// </summary>
        /// <param name="start">The start value of the collection.</param>
        /// <param name="end">The exclusive upper bound.</param>
        /// <param name="step">The value to increment in each step (positive or negative).</param>
        /// <returns>A collection with values <paramref name="start"/>, <paramref name="start"/> + <paramref name="step"/>, ...
        /// up to, but excluding <paramref name="end"/>.</returns>
        public static Sequence<long> Range(long start, long end, long step)
        {
            if ((step > 0 && start >= end) ||
                (step <= 0 && start <= end))
                return Sequence<long>.Empty;

            return new Sequence<long>(start, () => Range(start + step, end, step));
        }

        /// <summary>
        /// Creates a sequence containing equally spaced values in some integer interval.
        /// </summary>
        /// <param name="start">The start value of the collection.</param>
        /// <param name="end">The exclusive upper bound.</param>
        /// <param name="step">The value to increment in each step (positive or negative).</param>
        /// <returns>A collection with values <paramref name="start"/>, <paramref name="start"/> + <paramref name="step"/>, ...
        /// up to, but excluding <paramref name="end"/>.</returns>
        public static Sequence<BigInteger> Range(BigInteger start, BigInteger end, BigInteger step)
        {
            if ((step > 0 && start >= end) ||
                (step <= 0 && start <= end))
                return Sequence<BigInteger>.Empty;

            return new Sequence<BigInteger>(start, () => Range(start + step, end, step));
        }
    }
}
