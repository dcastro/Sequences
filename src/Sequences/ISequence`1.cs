using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sequences
{
    /// <summary>
    /// Represents an immutable lazy sequence of elements.
    /// Elements are only evaluated when they're needed, and <see cref="ISequence{T}"/> employs memoization to store the computed values and avoid re-evaluation.
    /// </summary>
    /// <typeparam name="T">The type of elements in the sequence.</typeparam>
    public interface ISequence<T> : IEnumerable<T>
    {
        /// <summary>
        /// Tests whether the sequence is empty.
        /// </summary>
        bool IsEmpty { get; }

        /// <summary>
        /// Returns the first element of this sequence.
        /// </summary>
        T Head { get; }

        /// <summary>
        /// Returns a sequence of all elements except the first.
        /// </summary>
        ISequence<T> Tail { get; }

        /// <summary>
        /// Returns this sequence without its last element.
        /// </summary>
        ISequence<T> Init { get; }

        /// <summary>
        /// Returns the length of this sequence.
        /// If this sequence represents an infinite series, this will never return!
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Tests whether this sequence is known to have a finite size.
        /// </summary>
        bool HasDefiniteSize { get; }

        /// <summary>
        /// Checks whether this sequence's tail has been evaluated.
        /// </summary>
        bool IsTailDefined { get; }

        /// <summary>
        /// Gets the element at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the element to get.</param>
        /// <returns>The element at the specified index.</returns>
        T this[int index] { get; }

        /// <summary>
        /// Iterates over the tails of this sequence. The first value will be this sequence, and the last value will be an empty sequence,
        /// with the intervening values the results of successive applications of <see cref="Tail"/>.
        /// </summary>
        /// <example>Sequence.Range(1,4) = (1,2,3), (2,3), (3), Empty</example>
        /// <returns>An iterator over all the tails of this sequence.</returns>
        IEnumerable<ISequence<T>> Tails();

        /// <summary>
        /// Iterates over the tails of this sequence. The first value will be this sequence, and the last value will be a sequence with the last element of this sequence,
        /// with the intervening values the results of successive applications of <see cref="Tail"/>.
        /// </summary>
        /// <example>Sequence.Range(1,4) = (1,2,3), (2,3), (3)</example>
        /// <returns>An iterator over all the tails of this sequence.</returns>
        IEnumerable<ISequence<T>> NonEmptyTails();

        /// <summary>
        /// Iterates over the inits of this sequence. The first value will be this sequence, and the last value will be an empty sequence,
        /// with the intervening values the results of successive applications of <see cref="Init"/>.
        /// </summary>
        /// <example>Sequence.Range(1,4) = (1,2,3), (1,2), (1), Empty</example>
        /// <returns>An iterator over all the inits of this sequence.</returns>
        IEnumerable<ISequence<T>> Inits();

        /// <summary>
        /// Forces evaluation of the whole sequence and returns it.
        /// If this sequence represents an infinite series, the method will never return!
        /// </summary>
        /// <returns></returns>
        ISequence<T> Force();

        /// <summary>
        /// Returns a copy of this sequence with the given element appended.
        /// </summary>
        /// <param name="elem">The element to append to this sequence.</param>
        /// <returns>A copy of this sequence with the given element appended.</returns>
        ISequence<T> Append(T elem);

        /// <summary>
        /// Returns a copy of this sequence with the given element prepended.
        /// </summary>
        /// <param name="elem">The element to prepend.</param>
        /// <returns>A copy of this sequence with the given element prepended.</returns>
        ISequence<T> Prepend(T elem);

        /// <summary>
        /// Returns a copy of this sequence concatenated with <paramref name="otherSequence"/>.
        /// </summary>
        /// <param name="otherSequence">The sequence with which to concatenate this sequence; will be lazily evaluated.</param>
        /// <returns>A copy of this sequence concatenated with <paramref name="otherSequence"/>.</returns>
        ISequence<T> Concat(Func<IEnumerable<T>> otherSequence);

        /// <summary>
        /// Returns a copy of this sequence without the first occurrence of the given element, if any is found.
        /// </summary>
        /// <param name="elem">The element to remove.</param>
        /// <returns>A copy of this sequence without the first occurrence of <paramref name="elem"/>.</returns>
        ISequence<T> Remove(T elem);

        /// <summary>
        /// Returns a copy of this sequence with one single replaced element.
        /// If <paramref name="index"/> is greater than the number of elements in this sequence, nothing will be replaced.
        /// </summary>
        /// <param name="index">The position of the replacement.</param>
        /// <param name="elem">The replacing element.</param>
        /// <returns>A copy of this sequence with the element at position <paramref name="index"/> replaced by <paramref name="elem"/>.</returns>
        ISequence<T> Updated(int index, T elem);

        /// <summary>
        /// Returns a new sequence in which the end is padded with <paramref name="elem"/>, if this sequence has less elements than <paramref name="length"/>.
        /// </summary>
        /// <param name="length">The number of elements to pad into the sequence.</param>
        /// <param name="elem">The element to use for padding.</param>
        /// <returns>A new sequence in which the end is padded with <paramref name="elem"/>, if this sequence has less elements than <paramref name="length"/>.</returns>
        ISequence<T> PadTo(int length, T elem);

        /// <summary>
        /// Splits this sequence into two at a given position.
        /// </summary>
        /// <param name="n">The position at which to split.</param>
        /// <returns>A pair of sequences consisting of the first <paramref name="n"/> elements of this sequence, and the other elements.</returns>
        Tuple<ISequence<T>, ISequence<T>> SplitAt(int n);

        /// <summary>
        /// Splits this sequence into a prefix/suffix pair according to a predicate.
        /// The first sequence will contain the longest prefix of this sequence whose elements all satisfy <paramref name="predicate"/>.
        /// </summary>
        /// <param name="predicate">The predicate used to split this sequence.</param>
        /// <returns>A pair consisting of the longest prefix of this sequence whose elements all satisfy <paramref name="predicate"/>, and the rest of this sequence.</returns>
        Tuple<ISequence<T>, ISequence<T>> Span(Func<T, bool> predicate);

        /// <summary>
        /// Returns a pair of sequences, where the first contains all the elements of this sequence that satisfy the <paramref name="predicate"/> function,
        /// and the second contains the elements that don't.
        /// </summary>
        /// <param name="predicate">The preidcate used to partition the elements.</param>
        /// <returns>A pair of sequences with the elements that satisfy <paramref name="predicate"/> and the elements that don't.</returns>
        Tuple<ISequence<T>, ISequence<T>> Partition(Func<T, bool> predicate);

        /// <summary>
        /// Returns a copy of this sequence where a slice of its elements is replaced by a <paramref name="patch"/> sequence.
        /// </summary>
        /// <param name="from">The index of the first replaced element.</param>
        /// <param name="patch">The elements with which to replace this sequence's slice.</param>
        /// <param name="replaced">A new sequence consisting of all elements of this sequence except that <paramref name="replaced"/> elements starting from <paramref name="from"/> are replaced by <paramref name="patch"/>.</param>
        /// <returns></returns>
        ISequence<T> Patch(int from, IEnumerable<T> patch, int replaced);

        /// <summary>
        /// Apply the given function to each element of this sequence.
        /// </summary>
        /// <param name="function">The function to apply to each element.</param>
        void ForEach(Action<T> function);

        /// <summary>
        /// Folds the elements of this sequence using the specified accumulator function. 
        /// </summary> 
        /// <example><code>int sum = Sequence.For(1,2,3,4).Fold(0, (a, b) => a + b);</code></example>
        /// <param name="seed">The initial accumulator value. A neutral value for the fold operation (e.g., empty list, or 0 for adding the elements of this sequence, or 1 for multiplication).</param>
        /// <param name="op">A function that takes the accumulator and an element of this sequence, and computes the new accumulator.</param>
        /// <returns>The result of applying <paramref name="op"/> between all the elements and <paramref name="seed"/>.</returns>
        T Fold(T seed, Func<T, T, T> op);

        /// <summary>
        /// Folds the elements of this sequence using the specified accumulator function, going right to left. 
        /// </summary> 
        /// <example><code>int sum = Sequence.For(1,2,3,4).FoldRight(0, (a, b) => a + b);</code></example>
        /// <param name="seed">The initial accumulator value. A neutral value for the fold operation (e.g., empty list, or 0 for adding the elements of this sequence, or 1 for multiplication).</param>
        /// <param name="op">A function that takes an element of this sequence and the accumulator, and computes the new accumulator.</param>
        /// <returns>The result of applying <paramref name="op"/> between all the elements and <paramref name="seed"/>.</returns>
        T FoldRight(T seed, Func<T, T, T> op);

        /// <summary>
        /// Reduces the elements of this sequence using the specified function.
        /// </summary>
        /// <param name="op">The operation to perform on successive elements of the sequence.</param>
        /// <returns>The accumulated value from successive applications of <paramref name="op"/>.</returns>
        T Reduce(Func<T, T, T> op);

        /// <summary>
        /// Reduces the elements of this sequence using the specified function, going right to left. 
        /// </summary>
        /// <param name="op">The operation to perform on successive elements of the sequence.</param>
        /// <returns>The accumulated value from successive applications of <paramref name="op"/>.</returns>
        T ReduceRight(Func<T, T, T> op);

        /// <summary>
        /// Crates a new sequence which contains all intermediate results of successive applications of a function <paramref name="op"/> to subsequent elements left to right.
        /// </summary>
        /// <param name="seed">The initial value for the scan.</param>
        /// <param name="op">A function that will apply operations to successive values in the sequence against previous accumulated results.</param>
        /// <returns>A new sequence which contains all intermediate results of successive applications of a function <paramref name="op"/> to subsequent elements left to right.</returns>
        ISequence<T> Scan(T seed, Func<T, T, T> op);

        /// <summary>
        /// Crates a new sequence which contains all intermediate results of successive applications of a function <paramref name="op"/> to subsequent elements right to left.
        /// </summary>
        /// <param name="seed">The initial value for the scan.</param>
        /// <param name="op">A function that will apply operations to successive values in the sequence against previous accumulated results.</param>
        /// <returns>A new sequence which contains all intermediate results of successive applications of a function <paramref name="op"/> to subsequent elements left to right.</returns>
        ISequence<T> ScanRight(T seed, Func<T, T, T> op);

        /// <summary>
        /// Groups elements in fixed size blocks by passing a "sliding window" over them.
        /// </summary>
        /// <param name="size">The number of elements per group.</param>
        /// <returns>An iterator producing sequences of size <paramref name="size"/>. The last sequence will be truncated if there are fewer elements than <paramref name="size"/>.</returns>
        IEnumerable<ISequence<T>> Sliding(int size);

        /// <summary>
        /// Groups elements in fixed size blocks by passing a "sliding window" over them.
        /// </summary>
        /// <param name="size">The number of elements per group.</param>
        /// <param name="step">The number of elements to skip per iteration.</param>
        /// <returns>An iterator producing sequences of size <paramref name="size"/>. The last sequence will be truncated if there are fewer elements than <paramref name="size"/>.</returns>
        IEnumerable<ISequence<T>> Sliding(int size, int step);

        /// <summary>
        /// Returns a subsequence starting at index <paramref name="from"/> and extending up to (but not including) index <paramref name="until"/>.
        /// </summary>
        /// <param name="from">The lowest index to include from this sequence.</param>
        /// <param name="until">The highest index to exclude from this sequence.</param>
        /// <returns>A subsequence starting at index <paramref name="from"/> and extending up to (but not including) index <paramref name="until"/>.</returns>
        ISequence<T> Slice(int from, int until);

        /// <summary>
        /// Partitions elements in fixed size sequences.
        /// </summary>
        /// <param name="size">The number of elements per group.</param>
        /// <returns>An iterator producing sequences of size <paramref name="size"/>. The last sequence will be truncated if the elements don't divide evenly.</returns>
        IEnumerable<ISequence<T>> Grouped(int size);

        /// <summary>
        /// Produces the range of all indices of this sequence.
        /// </summary>
        IEnumerable<int> Indices { get; }

        /// <summary>
        /// Finds the index of the first occurrence of some value in this sequence.
        /// </summary>
        /// <param name="elem">The value to search for.</param>
        /// <returns>The index of the first occurrence of <paramref name="elem"/> if any is found; otherwise, -1.</returns>
        int IndexOf(T elem);

        /// <summary>
        /// Finds the index of the first occurrence of some value in this sequence, after or at some start index.
        /// </summary>
        /// <param name="elem">The value to search for.</param>
        /// <param name="from">The start index.</param>
        /// <returns>The index of the first occurrence of <paramref name="elem"/> if any is found; otherwise, -1.</returns>
        int IndexOf(T elem, int from);

        /// <summary>
        /// Finds the index of the first occurrence of some value in this sequence, after or at some start index and within the range specified by <paramref name="count"/>.
        /// </summary>
        /// <param name="elem">The value to search for.</param>
        /// <param name="from">The start index.</param>
        /// <param name="count">The number of elements in the section to search.</param>
        /// <returns>The index of the first occurrence of <paramref name="elem"/> if any is found; otherwise, -1.</returns>
        int IndexOf(T elem, int from, int count);

        /// <summary>
        /// Finds the index of the first element satisfying some predicate.
        /// </summary>
        /// <param name="predicate">The predicate used to test elements.</param>
        /// <returns>The index of the first element that satisfies the predicate, or -1 if none exists.</returns>
        int IndexWhere(Func<T, bool> predicate);

        /// <summary>
        /// Finds the index of the first element satisfying some predicate after or at some start index.
        /// </summary>
        /// <param name="predicate">The predicate used to test elements.</param>
        /// <param name="from">The start index.</param>
        /// <returns>The index of the first element that satisfies the predicate, or -1 if none exists.</returns>
        int IndexWhere(Func<T, bool> predicate, int from);

        /// <summary>
        /// Finds the index of the first element satisfying some predicate after or at some start index and within the range specified by <paramref name="count"/>.
        /// </summary>
        /// <param name="predicate">The predicate used to test elements.</param>
        /// <param name="from">The start index.</param>
        /// <param name="count">The number of elements in the section to search.</param>
        /// <returns>The index of the first element that satisfies the predicate, or -1 if none exists.</returns>
        int IndexWhere(Func<T, bool> predicate, int from, int count);

        /// <summary>
        /// Finds the index of the last occurrence of some value in this sequence.
        /// </summary>
        /// <param name="elem">The value to search for.</param>
        /// <returns>The index of the last occurrence of <paramref name="elem"/> if any is found; otherwise, -1.</returns>
        int LastIndexOf(T elem);

        /// <summary>
        /// Finds the index of the last occurrence of some value in this sequence, before or at some end index.
        /// </summary>
        /// <param name="elem">The value to search for.</param>
        /// <param name="end">The end index.</param>
        /// <returns>The index of the last occurrence of <paramref name="elem"/> if any is found; otherwise, -1.</returns>
        int LastIndexOf(T elem, int end);

        /// <summary>
        /// Finds the index of the last occurrence of some value in this sequence, before or at some end index and within the range specified by <paramref name="count"/>.
        /// </summary>
        /// <param name="elem">The value to search for.</param>
        /// <param name="end">The end index.</param>
        /// <param name="count">The number of elements in the section to search.</param>
        /// <returns>The index of the last occurrence of <paramref name="elem"/> if any is found; otherwise, -1.</returns>
        int LastIndexOf(T elem, int end, int count);

        /// <summary>
        /// Finds the index of the last element satisfying some predicate.
        /// </summary>
        /// <param name="predicate">The predicate used to test elements.</param>
        /// <returns>The index of the last element that satisfies the predicate, or -1 if none exists.</returns>
        int LastIndexWhere(Func<T, bool> predicate);

        /// <summary>
        /// Finds the index of the last element satisfying some predicate before or at some end index.
        /// </summary>
        /// <param name="predicate">The predicate used to test elements.</param>
        /// <param name="end">The end index.</param>
        /// <returns>The index of the last element that satisfies the predicate, or -1 if none exists.</returns>
        int LastIndexWhere(Func<T, bool> predicate, int end);

        /// <summary>
        /// Finds the index of the last element satisfying some predicate before or at some end index and within the range specified by <paramref name="count"/>.
        /// </summary>
        /// <param name="predicate">The predicate used to test elements.</param>
        /// <param name="end">The end index.</param>
        /// <param name="count">The number of elements in the section to search.</param>
        /// <returns>The index of the last element that satisfies the predicate, or -1 if none exists.</returns>
        int LastIndexWhere(Func<T, bool> predicate, int end, int count);

        /// <summary>
        /// Copies the entire sequence to an array, starting at the beginning of the target array.
        /// Copying will stop once either the end of this sequence is reached, or the end of the array is reached.
        /// </summary>
        /// <param name="destination">The one-dimensional array that is the destination of the elements copied from this sequence.</param>
        /// <returns>The number of elements copied.</returns>
        int CopyTo(T[] destination);

        /// <summary>
        /// Copies the entire sequence to an array, starting at the position <paramref name="destinationIndex"/> of the target array.
        /// Copying will stop once either the end of this sequence is reached, or the end of the array is reached.
        /// </summary>
        /// <param name="destination">The one-dimensional array that is the destination of the elements copied from this sequence.</param>
        /// <param name="destinationIndex">The position of the target array at which copying begins.</param>
        /// <returns>The number of elements copied.</returns>
        int CopyTo(T[] destination, int destinationIndex);

        /// <summary>
        /// Copies a given number of elements from this sequence to an array, starting at the position <paramref name="index"/> of this sequence 
        /// and at the position <paramref name="destinationIndex"/> of the target array.
        /// Copying will stop once either the end of this sequence is reached, or the end of the array is reached, or <paramref name="count"/> elements have been copied.
        /// </summary>
        /// <param name="index">The position of this sequence at which copying begins.</param>
        /// <param name="destination">The one-dimensional array that is the destination of the elements copied from this sequence.</param>
        /// <param name="destinationIndex">The position of the target array at which copying begins.</param>
        /// <param name="count">The maximum number of elements to copy.</param>
        /// <returns>The number of elements copied.</returns>
        int CopyTo(int index, T[] destination, int destinationIndex, int count);

        /// <summary>
        /// Iterate over distinct combinations of a given size of this sequence's elements.
        /// </summary>
        /// <example>"abcd".AsSequence().Combinations(2) = ab, ac, ad, bc, bd, cd</example>
        /// <param name="size">The size of each combination.</param>
        /// <returns>An iterator that traverses the possible n-element combinations of this sequence's elements.</returns>
        IEnumerable<ISequence<T>> Combinations(int size);

        /// <summary>
        /// Iterates over distinct permutations of this sequence.
        /// </summary>
        /// <example>"abb".AsSequence().Permutations() = abb, bab, bba</example>
        /// <returns>An iterator which traverses the distinct permutations of this sequence.</returns>
        IEnumerable<ISequence<T>> Permutations();

        /// <summary>
        /// Returns a string with all the elements of this sequence.
        /// </summary>
        /// <returns>A string with all the elements of this sequence.</returns>
        string MkString();

        /// <summary>
        /// Returns a string with all the elements of this sequence, using a seperator string.
        /// </summary>
        /// <param name="separator">The separator string.</param>
        /// <returns>A string with all the elements of this sequence.</returns>
        string MkString(string separator);
    }
}
