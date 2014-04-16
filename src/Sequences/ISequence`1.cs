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
    public interface ISequence<out T> : IEnumerable<T>
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
    }
}
