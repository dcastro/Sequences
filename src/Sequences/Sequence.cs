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
        /// Creates a sequence from a given array.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <param name="items">The elements for which a sequence will be created.</param>
        /// <returns>A sequence created from the elements in <paramref name="items"/>.</returns>
        public static Sequence<T> For<T>(params T[] items)
        {
            return For(items.AsEnumerable());
        }

        /// <summary>
        /// Creates a sequence from a given enumerable.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <param name="items">The enumerable to be evaluated.</param>
        /// <returns>A sequence created by lazily-evaluating <paramref name="items"/>.</returns>
        public static Sequence<T> For<T>(IEnumerable<T> items)
        {
            return For(items.GetEnumerator());
        }

        private static Sequence<T> For<T>(IEnumerator<T> iterator)
        {
            return iterator.MoveNext()
                       ? new Sequence<T>(iterator.Current, () => For(iterator))
                       : Sequence<T>.Empty;
        }

        /// <summary>
        /// Creates a finite sequence containing the given element a number of times.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <param name="elem">The delegate to be repeatedly evaluated.</param>
        /// <param name="count">The number of times to repeat <paramref name="elem"/>.</param>
        /// <returns>A sequence containg <paramref name="count"/> number of <paramref name="elem"/>.</returns>
        public static Sequence<T> Fill<T>(Func<T> elem, int count)
        {
            return (count <= 0)
                       ? Sequence<T>.Empty
                       : new Sequence<T>(elem(), () => Fill(elem, count - 1));
        }

        /// <summary>
        /// Creates a finite sequence containing the given element a number of times.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <param name="elem">The element to be repeated.</param>
        /// <param name="count">The number of times to repeat <paramref name="elem"/>.</param>
        /// <returns>A sequence containg <paramref name="count"/> number of <paramref name="elem"/>.</returns>
        public static Sequence<T> Fill<T>(T elem, int count)
        {
            return Fill(() => elem, count);
        }

        /// <summary>
        /// Create an infinite sequence containing the given element expression (which is computed for each occurrence).
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <param name="elem">A delegate that will be continuously evaluated.</param>
        /// <returns>A sequence containing an infinite number of elements returned by the <paramref name="elem"/> delegate.</returns>
        public static Sequence<T> Continually<T>(Func<T> elem)
        {
            return new Sequence<T>(elem(), () => Continually(elem));
        }

        /// <summary>
        /// Create an infinite sequence containing the given element.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <param name="elem">The element to be continuously repeated.</param>
        /// <returns>A sequence containing an infinite number of <paramref name="elem"/></returns>
        public static Sequence<T> Continually<T>(T elem)
        {
            return Continually(() => elem);
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
