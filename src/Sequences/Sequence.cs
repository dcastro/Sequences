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
        /// Returns an empty sequence.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static ISequence<T> Empty<T>()
        {
            return EmptySequence<T>.Instance;
        }

        /// <summary>
        /// Creates a sequence from a given array.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <param name="items">The elements for which a sequence will be created.</param>
        /// <returns>A sequence created from the elements in <paramref name="items"/>.</returns>
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
        public static ISequence<T> With<T>(IEnumerable<T> items)
        {
            return items as ISequence<T> ?? With(items.GetEnumerator());
        }

        private static ISequence<T> With<T>(IEnumerator<T> iterator)
        {
            return iterator.MoveNext()
                       ? new Sequence<T>(iterator.Current, () => With(iterator))
                       : Empty<T>();
        }

        /// <summary>
        /// Creates a finite sequence containing the given element a number of times.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <param name="elem">The delegate to be repeatedly evaluated.</param>
        /// <param name="count">The number of times to repeat <paramref name="elem"/>.</param>
        /// <returns>A sequence containg <paramref name="count"/> number of <paramref name="elem"/>.</returns>
        public static ISequence<T> Fill<T>(Func<T> elem, int count)
        {
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
        public static ISequence<T> Continually<T>(Func<T> elem)
        {
            return new Sequence<T>(elem(), () => Continually(elem));
        }

        /// <summary>
        /// Create an infinite sequence containing the given element.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <param name="elem">The element to be continuously repeated.</param>
        /// <returns>A sequence containing an infinite number of <paramref name="elem"/></returns>
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
        public static ISequence<int> From(int start, int step)
        {
            return new Sequence<int>(start, () => From(start + step, step));
        }

        /// <summary>
        /// Creates an infinite sequence starting at <paramref name="start"/> and incrementing by 1 in each step.
        /// </summary>
        /// <param name="start">The start value of the sequence.</param>
        /// <returns>A sequence starting at value <paramref name="start"/>.</returns>
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
        public static ISequence<long> From(long start, long step)
        {
            return new Sequence<long>(start, () => From(start + step, step));
        }

        /// <summary>
        /// Creates an infinite sequence starting at <paramref name="start"/> and incrementing by 1 in each step.
        /// </summary>
        /// <param name="start">The start value of the sequence.</param>
        /// <returns>A sequence starting at value <paramref name="start"/>.</returns>
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
        public static ISequence<BigInteger> From(BigInteger start, BigInteger step)
        {
            return new Sequence<BigInteger>(start, () => From(start + step, step));
        }

        /// <summary>
        /// Creates an infinite sequence starting at <paramref name="start"/> and incrementing by 1 in each step.
        /// </summary>
        /// <param name="start">The start value of the sequence.</param>
        /// <returns>A sequence starting at value <paramref name="start"/>.</returns>
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
        public static ISequence<BigInteger> Range(BigInteger start, BigInteger end)
        {
            return Range(start, end, 1);
        }

        /// <summary>
        /// Creates a sequence from a given enumerable.
        /// </summary>
        /// <typeparam name="T">The type of elements in the sequence.</typeparam>
        /// <param name="enumerable">The enumerable to be evaluated.</param>
        /// <returns>A sequence created by lazily-evaluating <paramref name="enumerable"/>.</returns>
        public static ISequence<T> AsSequence<T>(this IEnumerable<T> enumerable)
        {
            if(enumerable == null)
                throw new ArgumentNullException("enumerable");

            return With(enumerable);
        }

        #region Extension Methods

        /// <summary>
        /// Bypasses a specified number of elements in a sequence and then returns the remaining elements.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">A sequence to return elements from.</param>
        /// <param name="count">The number of elements to skip before returning the remaining elements.</param>
        /// <returns>A sequence that contains the elements that occur after the specified index in the input sequence.</returns>
        public static ISequence<TSource> Skip<TSource>(this ISequence<TSource> source, int count)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            ISequence<TSource> seq = source;

            while (!seq.IsEmpty && count-- > 0)
                seq = seq.Tail;

            return seq;
        }

        /// <summary>
        /// Bypasses elements in a sequence as long as a specified condition is true and then returns the remaining elements.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">A sequence to return elements from.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns>A sequence that contains the elements from the input sequence starting at the first element in the linear series that does not pass the test specified by <paramref name="predicate"/></returns>
        public static ISequence<TSource> SkipWhile<TSource>(this ISequence<TSource> source,
                                                            Func<TSource, bool> predicate)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            if (predicate == null)
                throw new ArgumentNullException("predicate");

            ISequence<TSource> seq = source;

            while (!seq.IsEmpty && predicate(seq.Head))
                seq = seq.Tail;

            return seq;
        }

        /// <summary>
        /// Bypasses elements in a sequence as long as a specified condition is true and then returns the remaining elements.
        /// The element's index is used in the logic of the predicate function.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">The sequence to return elements from.</param>
        /// <param name="predicate">A function to test each element for a condition; the second parameter of the function represents the index of the element.</param>
        /// <returns>A sequence that contains the elements from the input sequence starting at the first element in the linear series that does not pass the test specified by <paramref name="predicate"/></returns>
        public static ISequence<TSource> SkipWhile<TSource>(this ISequence<TSource> source,
                                                            Func<TSource, int, bool> predicate)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            if (predicate == null)
                throw new ArgumentNullException("predicate");

            ISequence<TSource> seq = source;
            int index = 0;

            while (!seq.IsEmpty && predicate(seq.Head, index++))
                seq = seq.Tail;

            return seq;
        }

        /// <summary>
        /// Projects each element of a sequence into a new sequence.
        /// </summary>
        /// <typeparam name="TResult">The type of the elements returned by <paramref name="selector"/>.</typeparam>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">A sequence of values to invoke a transform function on.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <returns>A sequence whose elements are the result of invoking the transform function on each element of <paramref name="source"/>.</returns>
        public static ISequence<TResult> Select<TSource, TResult>(this ISequence<TSource> source,
                                                                  Func<TSource, TResult> selector)
        {
            return Enumerable.Select(source, selector).AsSequence();
        }

        /// <summary>
        /// Projects each element of a sequence into a new sequence.
        /// Each element's index is used in the logic of the selector function.
        /// </summary>
        /// <typeparam name="TResult">The type of the elements returned by <paramref name="selector"/>.</typeparam>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">A sequence of values to invoke a transform function on.</param>
        /// <param name="selector">A transform function to apply to each element and its index.</param>
        /// <returns>A sequence whose elements are the result of invoking the transform function on each element of <paramref name="source"/>.</returns>
        public static ISequence<TResult> Select<TSource, TResult>(this ISequence<TSource> source,
                                                                  Func<TSource, int, TResult> selector)
        {
            return Enumerable.Select(source, selector).AsSequence();
        }

        /// <summary>
        /// Projects each element of a sequence to an <see cref="IEnumerable{T}"/> and flattens the resulting enumerables into one sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TResult">The type of the elements of the sequence returned by selector.</typeparam>
        /// <param name="source">A sequence of values to project.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <returns>A sequence whose elements are the result of invoking the one-to-many transform function on each element of the input sequence.</returns>
        public static ISequence<TResult> SelectMany<TSource, TResult>(this ISequence<TSource> source,
                                                                      Func<TSource, IEnumerable<TResult>> selector)
        {
            return Enumerable.SelectMany(source, selector).AsSequence();
        }

        /// <summary>
        /// Projects each element of a sequence to an <see cref="IEnumerable{T}"/> and flattens the resulting enumerables into one sequence.
        /// The index of each source element is used in the projected form of that element.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TResult">The type of the elements of the sequence returned by selector.</typeparam>
        /// <param name="source">A sequence of values to project.</param>
        /// <param name="selector">A transform function to apply to each element; the second parameter of the function represents the index of the element.</param>
        /// <returns>A sequence whose elements are the result of invoking the one-to-many transform function on each element of the input sequence.</returns>
        public static ISequence<TResult> SelectMany<TSource, TResult>(this ISequence<TSource> source,
                                                                      Func<TSource, int, IEnumerable<TResult>> selector)
        {
            return Enumerable.SelectMany(source, selector).AsSequence();
        }

        /// <summary>
        /// Projects each element of a sequence to an <see cref="IEnumerable{T}"/>, flattens the resulting enumerables into one sequence, and invokes a result selector function on each element therein.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TCollection">The type of the intermediate elements collected by <paramref name="collectionSelector"/>.</typeparam>
        /// <typeparam name="TResult">The type of the elements of the resulting sequence.</typeparam>
        /// <param name="source">A sequence of values to project.</param>
        /// <param name="collectionSelector">A transform function to apply to each element of the input sequence.</param>
        /// <param name="resultSelector">A transform function to apply to each element of the intermediate sequence.</param>
        /// <returns>A sequence whose elements are the result of invoking the one-to-many transform function <paramref name="collectionSelector"/> on each element of <paramref name="source"/> and then mapping each of those sequence elements and their corresponding source element to a result element.</returns>
        public static ISequence<TResult> SelectMany<TSource, TCollection, TResult>(
            this ISequence<TSource> source,
            Func<TSource, IEnumerable<TCollection>> collectionSelector,
            Func<TSource, TCollection, TResult> resultSelector)
        {
            return Enumerable.SelectMany(source, collectionSelector, resultSelector).AsSequence();
        }

        /// <summary>
        /// Projects each element of a sequence to an <see cref="IEnumerable{T}"/>, flattens the resulting enumerables into one sequence, and invokes a result selector function on each element therein.
        /// The index of each source element is used in the intermediate projected form of that element.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TCollection">The type of the intermediate elements collected by <paramref name="collectionSelector"/>.</typeparam>
        /// <typeparam name="TResult">The type of the elements of the resulting sequence.</typeparam>
        /// <param name="source">A sequence of values to project.</param>
        /// <param name="collectionSelector">A transform function to apply to each <paramref name="source"/> element; the second parameter of the function represents the index of the <paramref name="source"/> element.</param>
        /// <param name="resultSelector">A transform function to apply to each element of the intermediate sequence.</param>
        /// <returns>A sequence whose elements are the result of invoking the one-to-many transform function <paramref name="collectionSelector"/> on each element of <paramref name="source"/> and then mapping each of those sequence elements and their corresponding source element to a result element.</returns>
        public static ISequence<TResult> SelectMany<TSource, TCollection, TResult>(
            this ISequence<TSource> source,
            Func<TSource, int, IEnumerable<TCollection>> collectionSelector,
            Func<TSource, TCollection, TResult> resultSelector)
        {
            return Enumerable.SelectMany(source, collectionSelector, resultSelector).AsSequence();
        }

        /// <summary>
        /// Filters a sequence of values based on a predicate.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">A sequence to filter.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns>A sequence that contains elements from <paramref name="source"/> that satisfy the condition.</returns>
        public static ISequence<TSource> Where<TSource>(this ISequence<TSource> source, Func<TSource, bool> predicate)
        {
            return Enumerable.Where(source, predicate).AsSequence();
        }

        /// <summary>
        /// Filters a sequence of values based on a predicate.
        /// Each element's index is used in the logic of the predicate function.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">A sequence to filter.</param>
        /// <param name="predicate">A function to test each element and its index for a condition.</param>
        /// <returns>A sequence that contains elements from <paramref name="source"/> that satisfy the condition.</returns>
        public static ISequence<TSource> Where<TSource>(this ISequence<TSource> source,
                                                        Func<TSource, int, bool> predicate)
        {
            return Enumerable.Where(source, predicate).AsSequence();
        }

        /// <summary>
        /// Returns a specified number of contiguous elements from the start of a sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">The sequence to return elements from.</param>
        /// <param name="count">The number of elements to return.</param>
        /// <returns>A sequence that contains the specified number of elements from the start of the input sequence.</returns>
        public static ISequence<TSource> Take<TSource>(this ISequence<TSource> source, int count)
        {
            return Enumerable.Take(source, count).AsSequence();
        }

        /// <summary>
        /// Returns elements from a sequence as long as a specified condition is true.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">The sequence to return elements from.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns>A sequence that contains the elements from the input sequence that occur before the element at which the test no longer passes.</returns>
        public static ISequence<TSource> TakeWhile<TSource>(this ISequence<TSource> source,
                                                            Func<TSource, bool> predicate)
        {
            return Enumerable.TakeWhile(source, predicate).AsSequence();
        }

        /// <summary>
        /// Returns elements from a sequence as long as a specified condition is true.
        /// The element's index is used in the logic of the predicate function.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">The sequence to return elements from.</param>
        /// <param name="predicate">A function to test each element for a condition; the second parameter of the function represents the index of the element.</param>
        /// <returns>A sequence that contains the elements from the input sequence that occur before the element at which the test no longer passes.</returns>
        public static ISequence<TSource> TakeWhile<TSource>(this ISequence<TSource> source,
                                                            Func<TSource, int, bool> predicate)
        {
            return Enumerable.TakeWhile(source, predicate).AsSequence();
        }

        /// <summary>
        /// Returns a sequence of tuples, where each tuple is formed by associating an element of the <paramref name="first"/> sequence with the element at the same position in the <paramref name="second"/> sequence.
        /// If one of the two sequences is longer than the other, its remaining elements are ignored.
        /// </summary>
        /// <typeparam name="TFirst">The type of the elements of <paramref name="first"/>.</typeparam>
        /// <typeparam name="TSecond">The type of the elements of <paramref name="second"/>.</typeparam>
        /// <param name="first">The sequence providing the first half of each result pair</param>
        /// <param name="second">The sequence providing the second half of each result pair.</param>
        /// <returns>A sequence of tuples, where each tuple is formed by associating an element of the first sequence with the element at the same position in the second sequence.</returns>
        public static ISequence<Tuple<TFirst, TSecond>> Zip<TFirst, TSecond>(this ISequence<TFirst> first,
                                                                             IEnumerable<TSecond> second)
        {
            return Zip(first, second, Tuple.Create);
        }

        /// <summary>
        /// Applies a specified function to the corresponding elements of two sequences, producing a sequence of the results.
        /// </summary>
        /// <typeparam name="TFirst">The type of the elements of <paramref name="first"/>.</typeparam>
        /// <typeparam name="TSecond">The type of the elements of <paramref name="second"/>.</typeparam>
        /// <typeparam name="TResult">The type of the elements of the result sequence.</typeparam>
        /// <param name="first">The first sequence to merge.</param>
        /// <param name="second">The second sequence to merge.</param>
        /// <param name="resultSelector">A function that specifies how to merge the elements from the two sequences.</param>
        /// <returns>A sequence that contains merged elements of two input sequences.</returns>
        public static ISequence<TResult> Zip<TFirst, TSecond, TResult>(this ISequence<TFirst> first,
                                                                       IEnumerable<TSecond> second,
                                                                       Func<TFirst, TSecond, TResult> resultSelector)
        {
            return Enumerable.Zip(first, second, resultSelector).AsSequence();
        }

        /// <summary>
        /// Returns distinct elements from a sequence by using the default equality comparer to compare values.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">The sequence to remove duplicate elements from.</param>
        /// <returns>A sequence that contains distinct elements from the input sequence.</returns>
        public static ISequence<TSource> Distinct<TSource>(this ISequence<TSource> source)
        {
            return Enumerable.Distinct(source).AsSequence();
        }

        /// <summary>
        /// Returns distinct elements from a sequence by using a specified <see cref="IEqualityComparer{T}"/> to compare values.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">The sequence to remove duplicate elements from.</param>
        /// <param name="comparer">An <see cref="IEqualityComparer{T}"/> to compare elements.</param>
        /// <returns>A sequence that contains distinct elements from the input sequence.</returns>
        public static ISequence<TSource> Distinct<TSource>(this ISequence<TSource> source,
                                                           IEqualityComparer<TSource> comparer)
        {
            return Enumerable.Distinct(source, comparer).AsSequence();
        }

        /// <summary>
        /// Produces the set difference of two sequences by using the default equality comparer to compare values.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of the input sequences.</typeparam>
        /// <param name="first">A sequence whose elements that are not also in <paramref name="second"/> will be returned.</param>
        /// <param name="second">A sequence whose elements that also occur in this sequence will cause those elements to be removed from the returned sequence.</param>
        /// <returns>A sequence that contains the set difference of the elements of two sequences.</returns>
        public static ISequence<TSource> Except<TSource>(this ISequence<TSource> first, IEnumerable<TSource> second)
        {
            return Enumerable.Except(first, second).AsSequence();
        }

        /// <summary>
        /// Produces the set difference of two sequences by using the specified <see cref="IEqualityComparer{T}"/> to compare values.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of the input sequences.</typeparam>
        /// <param name="first">A sequence whose elements that are not also in <paramref name="second"/> will be returned.</param>
        /// <param name="second">A sequence whose elements that also occur in this sequence will cause those elements to be removed from the returned sequence.</param>
        /// <param name="comparer">An <see cref="IEqualityComparer{T}"/> to compare values.</param>
        /// <returns>A sequence that contains the set difference of the elements of two sequences.</returns>
        public static ISequence<TSource> Except<TSource>(this ISequence<TSource> first, IEnumerable<TSource> second,
                                                         IEqualityComparer<TSource> comparer)
        {
            return Enumerable.Except(first, second, comparer).AsSequence();
        }

        /// <summary>
        /// Produces the set intersection of two sequences by using the default equality comparer to compare values.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of the input sequences.</typeparam>
        /// <param name="first">A sequence whose distinct elements that also appear in <paramref name="second"/> will be returned.</param>
        /// <param name="second">A sequence whose distinct elements that also appear in the first sequence will be returned.</param>
        /// <returns>A sequence that contains the elements that form the set intersection of two sequences.</returns>
        public static ISequence<TSource> Intersect<TSource>(this ISequence<TSource> first, IEnumerable<TSource> second)
        {
            return Enumerable.Intersect(first, second).AsSequence();
        }

        /// <summary>
        /// Produces the set intersection of two sequences by using the specified <see cref="IEqualityComparer{T}"/> to compare values.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of the input sequences.</typeparam>
        /// <param name="first">A sequence whose distinct elements that also appear in <paramref name="second"/> will be returned.</param>
        /// <param name="second">A sequence whose distinct elements that also appear in the first sequence will be returned.</param>
        /// <param name="comparer"></param>
        /// <returns>A sequence that contains the elements that form the set intersection of two sequences.</returns>
        public static ISequence<TSource> Intersect<TSource>(this ISequence<TSource> first, IEnumerable<TSource> second,
                                                            IEqualityComparer<TSource> comparer)
        {
            return Enumerable.Intersect(first, second, comparer).AsSequence();
        }

        /// <summary>
        /// Inverts the order of the elements in a sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">A sequence of values to reverse.</param>
        /// <returns>A sequence whose elements correspond to those of the input sequence in reverse order.</returns>
        public static ISequence<TSource> Reverse<TSource>(this ISequence<TSource> source)
        {
            return Enumerable.Reverse(source).AsSequence();
        }

        /// <summary>
        /// Computes the sum of the sequence of <see cref="BigInteger"/> values that are obtained by invoking a transform function on each element of the input sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">A sequence of values that are used to calculate a sum.</param>
        /// <param name="selector"></param>
        /// <returns>A transform function to apply to each element.</returns>
        public static BigInteger Sum<TSource>(this ISequence<TSource> source, Func<TSource, BigInteger> selector)
        {
            return source.Select(selector).Sum();
        }

        /// <summary>
        /// Computes the sum of the sequence of <see cref="Nullable{BigInteger}"/> values that are obtained by invoking a transform function on each element of the input sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">A sequence of values that are used to calculate a sum.</param>
        /// <param name="selector"></param>
        /// <returns>A transform function to apply to each element.</returns>
        public static BigInteger? Sum<TSource>(this ISequence<TSource> source, Func<TSource, BigInteger?> selector)
        {
            return source.Select(selector).Sum();
        }

        /// <summary>
        /// Computes the sum of a sequence of <see cref="BigInteger"/> values.
        /// </summary>
        /// <param name="source">A sequence of <see cref="BigInteger"/> values to calculate the sum of.</param>
        /// <returns>The sum of the values in the sequence.</returns>
        public static BigInteger Sum(this ISequence<BigInteger> source)
        {
            return source.Aggregate<BigInteger, BigInteger>(
                0,
                (acc, i) => acc + i);
        }

        /// <summary>
        /// Computes the sum of a sequence of <see cref="Nullable{BigInteger}"/> values.
        /// </summary>
        /// <param name="source">A sequence of <see cref="Nullable{BigInteger}"/> values to calculate the sum of.</param>
        /// <returns>The sum of the values in the sequence.</returns>
        public static BigInteger? Sum(this ISequence<BigInteger?> source)
        {
            return source.Aggregate<BigInteger?, BigInteger?>(
                0,
                (acc, i) => i == null ? acc : acc + i);
        }

        /// <summary>
        /// Produces the set union of two sequences by using the default equality comparer.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of the input sequences.</typeparam>
        /// <param name="first">A sequence whose distinct elements form the first set for the union.</param>
        /// <param name="second">A sequence whose distinct elements form the second set for the union.</param>
        /// <returns></returns>
        public static ISequence<TSource> Union<TSource>(this ISequence<TSource> first, IEnumerable<TSource> second)
        {
            return Enumerable.Union(first, second).AsSequence();
        }

        /// <summary>
        /// Produces the set union of two sequences by using a specified <see cref="IEqualityComparer{T}"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of the input sequences.</typeparam>
        /// <param name="first">A sequence whose distinct elements form the first set for the union.</param>
        /// <param name="second">A sequence whose distinct elements form the second set for the union.</param>
        /// <param name="comparer">The <see cref="IEqualityComparer{T}"/> to compare values.</param>
        /// <returns></returns>
        public static ISequence<TSource> Union<TSource>(this ISequence<TSource> first, IEnumerable<TSource> second,
                                                        IEqualityComparer<TSource> comparer)
        {
            return Enumerable.Union(first, second, comparer).AsSequence();
        }

        #endregion
    }
}
