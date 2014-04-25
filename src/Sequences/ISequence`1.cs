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
    }
}
