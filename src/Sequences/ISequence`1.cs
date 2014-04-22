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
    }
}
