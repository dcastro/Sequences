using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Numerics;
using System.Text;

namespace Sequences
{
    /// <summary>
    /// Provides a set of static methods for creating instances of <see cref="Sequence{T}"/>.
    /// </summary>
    [Pure]
    public static partial class Sequence
    {
        /// <summary>
        /// Returns an empty sequence.
        /// </summary>
        /// <typeparam name="T">The type parameter of the returned sequence.</typeparam>
        /// <returns>An empty sequence.</returns>
        [Pure]
        public static ISequence<T> Empty<T>()
        {
            return EmptySequence<T>.Instance;
        }

        /// <summary>
        /// Creates a new sequence builder.
        /// </summary>
        /// <typeparam name="T">The type of the element in the sequence.</typeparam>
        /// <returns>A new sequence builder.</returns>
        [Pure]
        public static SequenceBuilder<T> NewBuilder<T>()
        {
            return new SequenceBuilder<T>();
        }

        /// <summary>
        /// Creates a sequence from a given array.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <param name="items">The elements for which a sequence will be created.</param>
        /// <returns>A sequence created from the elements in <paramref name="items"/>.</returns>
        [Pure]
        public static ISequence<T> With<T>(params T[] items)
        {
            return With(items.AsEnumerable());
        }

        /// <summary>
        /// Creates a sequence from a given enumerable.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <param name="items">The enumerable to be evaluated.</param>
        /// <returns>A sequence created by lazily-evaluating <paramref name="items"/>.</returns>
        [Pure]
        public static ISequence<T> With<T>(IEnumerable<T> items)
        {
            if (items == null) throw new ArgumentNullException("items");
            return items as ISequence<T> ?? With(items.GetEnumerator());
        }

        private static ISequence<T> With<T>(IEnumerator<T> iterator)
        {
            return iterator.MoveNext()
                       ? new Sequence<T>(iterator.Current, () => With(iterator))
                       : Empty<T>();
        }

        /// <summary>
        /// Creates an infinite sequence that repeatedly applies a given function to a start value.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the sequence.</typeparam>
        /// <param name="start">The first value of the sequence.</param>
        /// <param name="func">The function that's repeatedly applied to the last element to produce the next element.</param>
        /// <returns>An infinite sequence obtained by repeatedly applying <paramref name="func"/> to <paramref name="start"/>.</returns>
        [Pure]
        public static ISequence<T> Iterate<T>(T start, Func<T, T> func)
        {
            if (func == null) throw new ArgumentNullException("func");
            return new Sequence<T>(start, () => Iterate(func(start), func));
        }

        /// <summary>
        /// Creates a finite sequence of a given length that repeatedly applies a given function to a start value.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the sequence.</typeparam>
        /// <param name="start">The first value of the sequence.</param>
        /// <param name="length">The number of elements in the sequence.</param>
        /// <param name="func">The function that's repeatedly applied to the last element to produce the next element.</param>
        /// <returns>A finite sequence of length <paramref name="length"/> obtained by repeatedly applying <paramref name="func"/> to <paramref name="start"/>.</returns>
        [Pure]
        public static ISequence<T> Iterate<T>(T start, int length, Func<T, T> func)
        {
            if (func == null) throw new ArgumentNullException("func");

            return length <= 0
                       ? Empty<T>()
                       : new Sequence<T>(start, () => Iterate(func(start), length - 1, func));
        }

        /// <summary>
        /// Creates a sequence obtained by applying a given function over a range of integer values starting at 0.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the sequence.</typeparam>
        /// <param name="length">The number of elements in the collection.</param>
        /// <param name="func">The function used to produce the elements.</param>
        /// <returns>A sequence obtained by applying <paramref name="func"/> over a range of integer values from 0 to <paramref name="length"/> - 1.</returns>
        [Pure]
        public static ISequence<T> Tabulate<T>(int length, Func<int, T> func)
        {
            if (func == null) throw new ArgumentNullException("func");
            return Tabulate(length, 0, func);
        }

        private static ISequence<T> Tabulate<T>(int length, int current, Func<int, T> func)
        {
            return current >= length
                       ? Empty<T>()
                       : new Sequence<T>(func(current), () => Tabulate(length, current + 1, func));
        }

        /// <summary>
        /// Creates a finite sequence containing the given element a number of times.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <param name="elem">The delegate to be repeatedly evaluated.</param>
        /// <param name="count">The number of times to repeat <paramref name="elem"/>.</param>
        /// <returns>A sequence containg <paramref name="count"/> number of <paramref name="elem"/>.</returns>
        [Pure]
        public static ISequence<T> Fill<T>(Func<T> elem, int count)
        {
            if (elem == null) throw new ArgumentNullException("elem");
            return (count > 0)
                       ? new Sequence<T>(elem(), () => Fill(elem, count - 1))
                       : Empty<T>();
        }

        /// <summary>
        /// Creates a finite sequence containing the given element a number of times.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <param name="elem">The element to be repeated.</param>
        /// <param name="count">The number of times to repeat <paramref name="elem"/>.</param>
        /// <returns>A sequence containg <paramref name="count"/> number of <paramref name="elem"/>.</returns>
        [Pure]
        public static ISequence<T> Fill<T>(T elem, int count)
        {
            return Fill(() => elem, count);
        }

        /// <summary>
        /// Create an infinite sequence containing the given element expression (which is computed for each occurrence).
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <param name="elem">A delegate that will be continuously evaluated.</param>
        /// <returns>A sequence containing an infinite number of elements returned by the <paramref name="elem"/> delegate.</returns>
        [Pure]
        public static ISequence<T> Continually<T>(Func<T> elem)
        {
            if (elem == null) throw new ArgumentNullException("elem");
            return new Sequence<T>(elem(), () => Continually(elem));
        }

        /// <summary>
        /// Create an infinite sequence containing the given element.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <param name="elem">The element to be continuously repeated.</param>
        /// <returns>A sequence containing an infinite number of <paramref name="elem"/></returns>
        [Pure]
        public static ISequence<T> Continually<T>(T elem)
        {
            return Continually(() => elem);
        }

        /// <summary>
        /// Creates an infinite sequence starting at <paramref name="start"/> and incrementing by <paramref name="step"/> in each step.
        /// </summary>
        /// <param name="start">The start value of the sequence.</param>
        /// <param name="step">The value to increment in each step (positive or negative).</param>
        /// <returns>A sequence starting at value <paramref name="start"/>.</returns>
        [Pure]
        public static ISequence<int> From(int start, int step)
        {
            return new Sequence<int>(start, () => From(start + step, step));
        }

        /// <summary>
        /// Creates an infinite sequence starting at <paramref name="start"/> and incrementing by 1 in each step.
        /// </summary>
        /// <param name="start">The start value of the sequence.</param>
        /// <returns>A sequence starting at value <paramref name="start"/>.</returns>
        [Pure]
        public static ISequence<int> From(int start)
        {
            return From(start, 1);
        }

        /// <summary>
        /// Creates an infinite sequence starting at <paramref name="start"/> and incrementing by <paramref name="step"/> in each step.
        /// </summary>
        /// <param name="start">The start value of the sequence.</param>
        /// <param name="step">The value to increment in each step (positive or negative).</param>
        /// <returns>A sequence starting at value <paramref name="start"/>.</returns>
        [Pure]
        public static ISequence<long> From(long start, long step)
        {
            return new Sequence<long>(start, () => From(start + step, step));
        }

        /// <summary>
        /// Creates an infinite sequence starting at <paramref name="start"/> and incrementing by 1 in each step.
        /// </summary>
        /// <param name="start">The start value of the sequence.</param>
        /// <returns>A sequence starting at value <paramref name="start"/>.</returns>
        [Pure]
        public static ISequence<long> From(long start)
        {
            return From(start, 1);
        }

        /// <summary>
        /// Creates an infinite sequence starting at <paramref name="start"/> and incrementing by <paramref name="step"/> in each step.
        /// </summary>
        /// <param name="start">The start value of the sequence.</param>
        /// <param name="step">The value to increment in each step (positive or negative).</param>
        /// <returns>A sequence starting at value <paramref name="start"/>.</returns>
        [Pure]
        public static ISequence<BigInteger> From(BigInteger start, BigInteger step)
        {
            return new Sequence<BigInteger>(start, () => From(start + step, step));
        }

        /// <summary>
        /// Creates an infinite sequence starting at <paramref name="start"/> and incrementing by 1 in each step.
        /// </summary>
        /// <param name="start">The start value of the sequence.</param>
        /// <returns>A sequence starting at value <paramref name="start"/>.</returns>
        [Pure]
        public static ISequence<BigInteger> From(BigInteger start)
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
        [Pure]
        public static ISequence<int> Range(int start, int end, int step)
        {
            if ((step > 0 && start >= end) ||
                (step <= 0 && start <= end))
                return Empty<int>();

            return new Sequence<int>(start, () => Range(start + step, end, step));
        }

        /// <summary>
        /// Creates a sequence containing equally spaced values in some integer interval.
        /// </summary>
        /// <param name="start">The start value of the collection.</param>
        /// <param name="end">The exclusive upper bound.</param>
        /// <returns>A collection with values <paramref name="start"/>, <paramref name="start"/> + 1, ...
        /// up to, but excluding <paramref name="end"/>.</returns>
        [Pure]
        public static ISequence<int> Range(int start, int end)
        {
            return Range(start, end, 1);
        }

        /// <summary>
        /// Creates a sequence containing equally spaced values in some integer interval.
        /// </summary>
        /// <param name="start">The start value of the collection.</param>
        /// <param name="end">The exclusive upper bound.</param>
        /// <param name="step">The value to increment in each step (positive or negative).</param>
        /// <returns>A collection with values <paramref name="start"/>, <paramref name="start"/> + <paramref name="step"/>, ...
        /// up to, but excluding <paramref name="end"/>.</returns>
        [Pure]
        public static ISequence<long> Range(long start, long end, long step)
        {
            if ((step > 0 && start >= end) ||
                (step <= 0 && start <= end))
                return Empty<long>();

            return new Sequence<long>(start, () => Range(start + step, end, step));
        }

        /// <summary>
        /// Creates a sequence containing equally spaced values in some integer interval.
        /// </summary>
        /// <param name="start">The start value of the collection.</param>
        /// <param name="end">The exclusive upper bound.</param>
        /// <returns>A collection with values <paramref name="start"/>, <paramref name="start"/> + 1, ...
        /// up to, but excluding <paramref name="end"/>.</returns>
        [Pure]
        public static ISequence<long> Range(long start, long end)
        {
            return Range(start, end, 1);
        }

        /// <summary>
        /// Creates a sequence containing equally spaced values in some integer interval.
        /// </summary>
        /// <param name="start">The start value of the collection.</param>
        /// <param name="end">The exclusive upper bound.</param>
        /// <param name="step">The value to increment in each step (positive or negative).</param>
        /// <returns>A collection with values <paramref name="start"/>, <paramref name="start"/> + <paramref name="step"/>, ...
        /// up to, but excluding <paramref name="end"/>.</returns>
        [Pure]
        public static ISequence<BigInteger> Range(BigInteger start, BigInteger end, BigInteger step)
        {
            if ((step > 0 && start >= end) ||
                (step <= 0 && start <= end))
                return Empty<BigInteger>();

            return new Sequence<BigInteger>(start, () => Range(start + step, end, step));
        }

        /// <summary>
        /// Creates a sequence containing equally spaced values in some integer interval.
        /// </summary>
        /// <param name="start">The start value of the collection.</param>
        /// <param name="end">The exclusive upper bound.</param>
        /// <returns>A collection with values <paramref name="start"/>, <paramref name="start"/> + 1, ...
        /// up to, but excluding <paramref name="end"/>.</returns>
        [Pure]
        public static ISequence<BigInteger> Range(BigInteger start, BigInteger end)
        {
            return Range(start, end, 1);
        }
    }
}
