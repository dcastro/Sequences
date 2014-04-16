using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sequences
{
    /// <summary>
    /// Represents an immutable lazy sequence of elements.
    /// Elements are only evaluated when they're needed, and <see cref="Sequence{T}"/> employs memoization to store the computed values and avoid re-evaluation.
    /// </summary>
    /// <typeparam name="T">The type of elements in the sequence.</typeparam>
    public class Sequence<T> : ISequence<T>
    {
        private readonly T _head;
        private readonly Lazy<ISequence<T>> _tail;

        /// <summary>
        /// Tests whether the sequence is empty.
        /// </summary>
        public virtual bool IsEmpty
        {
            get { return false; }
        }

        /// <summary>
        /// Returns the first element of this sequence.
        /// </summary>
        public virtual T Head
        {
            get { return _head; }
        }

        /// <summary>
        /// Returns a sequence of all elements except the first.
        /// </summary>
        public virtual ISequence<T> Tail
        {
            get { return _tail.Value; }
        }

        private static readonly Lazy<ISequence<T>> _empty =
            new Lazy<ISequence<T>>(() => new EmptySequence());

        /// <summary>
        /// Returns an empty sequence.
        /// </summary>
        public static ISequence<T> Empty
        {
            get { return _empty.Value; }
        }

        /// <summary>
        /// Initializes a new instance of <see cref="Sequence{T}"/>.
        /// </summary>
        /// <param name="head">The first element of the sequence.</param>
        /// <param name="tail">A delegate that will be used to realize the sequence's tail when needed.</param>
        public Sequence(T head, Func<ISequence<T>> tail)
            : this(head, new Lazy<ISequence<T>>(tail))
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="Sequence{T}"/>.
        /// </summary>
        /// <param name="head">The first element of the sequence.</param>
        /// <param name="tail">The tail of the sequence.</param>
        protected Sequence(T head, Lazy<ISequence<T>> tail)
        {
            _head = head;
            _tail = tail;
        }

        /// <summary>
        /// Returns an enumerator that iterates through the <see cref="Sequence{T}"/>.
        /// </summary>
        /// <returns>An <see cref="IEnumerator{T}"/> for the sequence.</returns>
        public IEnumerator<T> GetEnumerator()
        {
            //we use an iterative proccess, instead of recursively calling Tail.GetEnumerator
            //to avoid a stack overflow exception
            ISequence<T> sequence = this;

            while (!sequence.IsEmpty)
            {
                yield return sequence.Head;
                sequence = sequence.Tail;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private class EmptySequence : Sequence<T>
        {
            public EmptySequence()
                : base(default(T), null as Lazy<ISequence<T>>)
            {
            }

            public override bool IsEmpty
            {
                get { return true; }
            }

            public override T Head
            {
                get { throw new InvalidOperationException("An empty sequence doesn't have a head."); }
            }

            public override ISequence<T> Tail
            {
                get { throw new InvalidOperationException("An empty sequence doesn't have a tail."); }
            }
        }
    }
}
