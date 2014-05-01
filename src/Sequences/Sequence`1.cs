using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Sequences
{
    /// <summary>
    /// Represents an immutable lazy sequence of elements.
    /// Elements are only evaluated when they're needed, and <see cref="Sequence{T}"/> employs memoization to store the computed values and avoid re-evaluation.
    /// </summary>
    /// <typeparam name="T">The type of elements in the sequence.</typeparam>
    public partial class Sequence<T> : ISequence<T>
    {
        private readonly T _head;
        private readonly Lazy<ISequence<T>> _tail;
        private int _count = int.MinValue;
        private bool _hasDefiniteSize = false;

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

        /// <summary>
        /// Returns this sequence without its last element.
        /// </summary>
        public virtual ISequence<T> Init
        {
            get { return Tail.IsEmpty ? Tail : new Sequence<T>(Head, () => Tail.Init); }
        }

        /// <summary>
        /// Returns the length of this sequence.
        /// If this sequence represents an infinite series, this will never return!
        /// </summary>
        public int Count
        {
            get
            {
                if (_count < 0)
                    _count = this.Count();

                return _count;
            }
        }

        /// <summary>
        /// Tests whether this sequence is known to have a finite size.
        /// </summary>
        public bool HasDefiniteSize
        {
            get
            {
                if (!_hasDefiniteSize)
                {
                    ISequence<T> left = this;
                    while (!left.IsEmpty && left.IsTailDefined)
                        left = left.Tail;

                    _hasDefiniteSize = left.IsEmpty;
                }

                return _hasDefiniteSize;
            }
        }

        /// <summary>
        /// Checks whether this sequence's tail has been evaluated.
        /// </summary>
        public bool IsTailDefined
        {
            get { return _tail.IsValueCreated; }
        }

        /// <summary>
        /// Gets the element at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the element to get.</param>
        /// <returns>The element at the specified index.</returns>
        public T this[int index]
        {
            get { return this.ElementAt(index); }
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

        /// <summary>
        /// Iterates over the tails of this sequence. The first value will be this sequence, and the last value will be an empty sequence,
        /// with the intervening values the results of successive applications of <see cref="ISequence{T}.Tail"/>.
        /// </summary>
        /// <example>Sequence.Range(1,4) = (1,2,3), (2,3), (3), Empty</example>
        /// <returns>An iterator over all the tails of this sequence.</returns>
        public IEnumerable<ISequence<T>> Tails()
        {
            ISequence<T> sequence = this;

            while (!sequence.IsEmpty)
            {
                yield return sequence;
                sequence = sequence.Tail;
            }

            //return empty sequence
            yield return sequence;
        }

        /// <summary>
        /// Iterates over the tails of this sequence. The first value will be this sequence, and the last value will be a sequence with the last element of this sequence,
        /// with the intervening values the results of successive applications of <see cref="ISequence{T}.Tail"/>.
        /// </summary>
        /// <example>Sequence.Range(1,4) = (1,2,3), (2,3), (3)</example>
        /// <returns>An iterator over all the tails of this sequence.</returns>
        public IEnumerable<ISequence<T>> NonEmptyTails()
        {
            ISequence<T> sequence = this;

            while (!sequence.IsEmpty)
            {
                yield return sequence;
                sequence = sequence.Tail;
            }
        }

        /// <summary>
        /// Iterates over the inits of this sequence. The first value will be this sequence, and the last value will be an empty sequence,
        /// with the intervening values the results of successive applications of <see cref="ISequence{T}.Init"/>.
        /// </summary>
        /// <example>Sequence.Range(1,4) = (1,2,3), (1,2), (1), Empty</example>
        /// <returns>An iterator over all the inits of this sequence.</returns>
        public IEnumerable<ISequence<T>> Inits()
        {
            ISequence<T> sequence = this;

            while (! sequence.IsEmpty)
            {
                yield return sequence;
                sequence = sequence.Init;
            }

            //return empty sequence
            yield return sequence;
        }

        /// <summary>
        /// Forces evaluation of the whole sequence and returns it.
        /// If this sequence represents an infinite series, the method will never return!
        /// </summary>
        /// <returns></returns>
        public ISequence<T> Force()
        {
            foreach (var elem in this)
            {
            }
            return this;
        }

        /// <summary>
        /// Returns a copy of this sequence with the given element appended.
        /// </summary>
        /// <param name="elem">The element to append to this sequence.</param>
        /// <returns>A copy of this sequence with the given element appended.</returns>
        public ISequence<T> Append(T elem)
        {
            return Concat(() => new Sequence<T>(elem, Sequence.Empty<T>));
        }

        /// <summary>
        /// Returns a copy of this sequence with the given element prepended.
        /// </summary>
        /// <param name="elem">The element to prepend.</param>
        /// <returns>A copy of this sequence with the given element prepended.</returns>
        public ISequence<T> Prepend(T elem)
        {
            return new Sequence<T>(elem, () => this);
        }

        /// <summary>
        /// Returns a copy of this sequence concatenated with <paramref name="otherSequence"/>.
        /// </summary>
        /// <param name="otherSequence">The sequence with which to concatenate this sequence; will be lazily evaluated.</param>
        /// <returns>A copy of this sequence concatenated with <paramref name="otherSequence"/>.</returns>
        public ISequence<T> Concat(Func<IEnumerable<T>> otherSequence)
        {
            return IsEmpty
                ? otherSequence().AsSequence()
                : new Sequence<T>(Head, () => Tail.Concat(otherSequence));
        }

        /// <summary>
        /// Returns a copy of this sequence without the first occurrence of the given element, if any is found.
        /// </summary>
        /// <param name="elem">The element to remove.</param>
        /// <returns>A copy of this sequence without the first occurrence of <paramref name="elem"/>.</returns>
        public ISequence<T> Remove(T elem)
        {
            IEqualityComparer<T> comparer = EqualityComparer<T>.Default;

            if (IsEmpty)
                return this;
            if (comparer.Equals(Head, elem))
                return Tail;
            return new Sequence<T>(Head, () => Tail.Remove(elem));
        }

        /// <summary>
        /// Returns a copy of this sequence with one single replaced element.
        /// If <paramref name="index"/> is greater than the number of elements in this sequence, nothing will be replaced.
        /// </summary>
        /// <param name="index">The position of the replacement.</param>
        /// <param name="elem">The replacing element.</param>
        /// <returns>A copy of this sequence with the element at position <paramref name="index"/> replaced by <paramref name="elem"/>.</returns>
        public ISequence<T> Updated(int index, T elem)
        {
            if (index < 0)
                throw new ArgumentOutOfRangeException("index", "index must be a non-negative integer.");

            if (IsEmpty)
                return this;

            return index == 0
                ? new Sequence<T>(elem, () => Tail)
                : new Sequence<T>(Head, () => Tail.Updated(index - 1, elem));
        }

        /// <summary>
        /// Apply the given function to each element of this sequence.
        /// </summary>
        /// <param name="function">The function to apply to each element.</param>
        public void ForEach(Action<T> function)
        {
            foreach (var elem in this)
                function(elem);
        }

        /// <summary>
        /// Folds the elements of this sequence using the specified accumulator function. 
        /// </summary> 
        /// <example><code>int sum = Sequence.For(1,2,3,4).Fold(0, (a, b) => a + b);</code></example>
        /// <param name="seed">The initial accumulator value. A neutral value for the fold operation (e.g., empty list, or 0 for adding the elements of this sequence, or 1 for multiplication).</param>
        /// <param name="op">A function that takes the accumulator and an element of this sequence, and computes the new accumulator.</param>
        /// <returns>The result of applying <paramref name="op"/> between all the elements and <paramref name="seed"/>.</returns>
        public T Fold(T seed, Func<T, T, T> op)
        {
            return this.Aggregate(seed, op);
        }

        /// <summary>
        /// Folds the elements of this sequence using the specified accumulator function, going right to left. 
        /// </summary> 
        /// <example><code>int sum = Sequence.For(1,2,3,4).FoldRight(0, (a, b) => a + b);</code></example>
        /// <param name="seed">The initial accumulator value. A neutral value for the fold operation (e.g., empty list, or 0 for adding the elements of this sequence, or 1 for multiplication).</param>
        /// <param name="op">A function that takes an element of this sequence and the accumulator, and computes the new accumulator.</param>
        /// <returns>The result of applying <paramref name="op"/> between all the elements and <paramref name="seed"/>.</returns>
        public T FoldRight(T seed, Func<T, T, T> op)
        {
            if (IsEmpty)
                return seed;
            return op(Head, Tail.FoldRight(seed, op));
        }

        /// <summary>
        /// Reduces the elements of this sequence using the specified function.
        /// </summary>
        /// <param name="op">The operation to perform on successive elements of the sequence.</param>
        /// <returns>The accumulated value from successive applications of <paramref name="op"/>.</returns>
        public T Reduce(Func<T, T, T> op)
        {
            if (IsEmpty)
                throw new InvalidOperationException("Cannot reduce empty sequence.");

            return Tail.Fold(Head, op);
        }

        /// <summary>
        /// Reduces the elements of this sequence using the specified function, going right to left. 
        /// </summary>
        /// <param name="op">The operation to perform on successive elements of the sequence.</param>
        /// <returns>The accumulated value from successive applications of <paramref name="op"/>.</returns>
        public T ReduceRight(Func<T, T, T> op)
        {
            if (IsEmpty)
                throw new InvalidOperationException("Cannot reduce empty sequence.");
            if (Tail.IsEmpty)
                return Head;

            return op(Head, Tail.ReduceRight(op));
        }

        /// <summary>
        /// Crates a new sequence which contains all intermediate results of successive applications of a function <paramref name="op"/> to subsequent elements left to right.
        /// </summary>
        /// <param name="seed">The initial value for the scan.</param>
        /// <param name="op">A function that will apply operations to successive values in the sequence against previous accumulated results.</param>
        /// <returns>A new sequence which contains all intermediate results of successive applications of a function <paramref name="op"/> to subsequent elements left to right.</returns>
        public ISequence<T> Scan(T seed, Func<T, T, T> op)
        {
            if (IsEmpty)
                return Sequence.For(seed);

            return new Sequence<T>(seed, () =>
                Tail.Scan(op(seed, Head), op));
        }

        /// <summary>
        /// Crates a new sequence which contains all intermediate results of successive applications of a function <paramref name="op"/> to subsequent elements right to left.
        /// </summary>
        /// <param name="seed">The initial value for the scan.</param>
        /// <param name="op">A function that will apply operations to successive values in the sequence against previous accumulated results.</param>
        /// <returns>A new sequence which contains all intermediate results of successive applications of a function <paramref name="op"/> to subsequent elements left to right.</returns>
        public ISequence<T> ScanRight(T seed, Func<T, T, T> op)
        {
            var scanned = new Stack<T>();
            scanned.Push(seed);

            var acc = seed;

            foreach (var elem in this.Reverse())
            {
                acc = op(elem, acc);
                scanned.Push(acc);
            }

            return scanned.AsSequence();
        }

        /// <summary>
        /// Groups elements in fixed size blocks by passing a "sliding window" over them.
        /// </summary>
        /// <param name="size">The number of elements per group.</param>
        /// <returns>An iterator producing sequences of size <paramref name="size"/>. The last sequence will be truncated if there are fewer elements than <paramref name="size"/>.</returns>
        public IEnumerable<ISequence<T>> Sliding(int size)
        {
            return Sliding(size, 1);
        }

        /// <summary>
        /// Groups elements in fixed size blocks by passing a "sliding window" over them.
        /// </summary>
        /// <param name="size">The number of elements per group.</param>
        /// <param name="step">The number of elements to skip per iteration.</param>
        /// <returns>An iterator producing sequences of size <paramref name="size"/>. The last sequence will be truncated if there are fewer elements than <paramref name="size"/>.</returns>
        public IEnumerable<ISequence<T>> Sliding(int size, int step)
        {
            if (size <= 0)
                throw new ArgumentOutOfRangeException("size", "size must be a positive integer.");
            if (step <= 0)
                throw new ArgumentOutOfRangeException("step", "step must be a positive integer.");


            return new SlidingIterator(this, size, step);
        }

        /// <summary>
        /// Returns a subsequence starting at index <paramref name="from"/> and extending up to (but not including) index <paramref name="until"/>.
        /// </summary>
        /// <param name="from">The lowest index to include from this sequence.</param>
        /// <param name="until">The highest index to exclude from this sequence.</param>
        /// <returns>A subsequence starting at index <paramref name="from"/> and extending up to (but not including) index <paramref name="until"/>.</returns>
        public ISequence<T> Slice(int from, int until)
        {
            from = Math.Max(from, 0);

            if (until <= from || IsEmpty)
                return Sequence.Empty<T>();

            return this.Skip(from).Take(until - from);
        }

        /// <summary>
        /// Partitions elements in fixed size sequences.
        /// </summary>
        /// <param name="size">The number of elements per group.</param>
        /// <returns>An iterator producing sequences of size <paramref name="size"/>. The last sequence will be truncated if the elements don't divide evenly.</returns>
        public IEnumerable<ISequence<T>> Grouped(int size)
        {
            if (size <= 0)
                throw new ArgumentOutOfRangeException("size", "size must be a positive integer.");

            return new GroupedIterator(this, size);
        }

        /// <summary>
        /// Produces the range of all indices of this sequence.
        /// </summary>
        public IEnumerable<int> Indices
        {
            get { return this.Select((elem, index) => index); }
        }

        /// <summary>
        /// Finds the index of the first occurrence of some value in this sequence.
        /// </summary>
        /// <param name="elem">The value to search for.</param>
        /// <returns>The index of the first occurrence of <paramref name="elem"/> if any is found; otherwise, -1.</returns>
        public int IndexOf(T elem)
        {
            return IndexOf(elem, 0);
        }

        /// <summary>
        /// Finds the index of the first occurrence of some value in this sequence, after or at some start index.
        /// </summary>
        /// <param name="elem">The value to search for.</param>
        /// <param name="from">The start index.</param>
        /// <returns>The index of the first occurrence of <paramref name="elem"/> if any is found; otherwise, -1.</returns>
        public int IndexOf(T elem, int from)
        {
            IEqualityComparer<T> comparer = EqualityComparer<T>.Default;
            return IndexWhere(current => comparer.Equals(current, elem), from);
        }

        /// <summary>
        /// Finds the index of the first occurrence of some value in this sequence, after or at some start index and within the range specified by <paramref name="count"/>.
        /// </summary>
        /// <param name="elem">The value to search for.</param>
        /// <param name="from">The start index.</param>
        /// <param name="count">The number of elements in the section to search.</param>
        /// <returns>The index of the first occurrence of <paramref name="elem"/> if any is found; otherwise, -1.</returns>
        public int IndexOf(T elem, int from, int count)
        {
            IEqualityComparer<T> comparer = EqualityComparer<T>.Default;
            return IndexWhere(current => comparer.Equals(current, elem), from, count);
        }

        /// <summary>
        /// Finds the index of the first element satisfying some predicate.
        /// </summary>
        /// <param name="predicate">The predicate used to test elements.</param>
        /// <returns>The index of the first element that satisfies the predicate, or -1 if none exists.</returns>
        public int IndexWhere(Func<T, bool> predicate)
        {
            return IndexWhere(predicate, 0);
        }

        /// <summary>
        /// Finds the index of the first element satisfying some predicate after or at some start index.
        /// </summary>
        /// <param name="predicate">The predicate used to test elements.</param>
        /// <param name="from">The start index.</param>
        /// <returns>The index of the first element that satisfies the predicate, or -1 if none exists.</returns>
        public int IndexWhere(Func<T, bool> predicate, int from)
        {
            if (from < 0)
                from = 0;

            int index = from;
            foreach (var current in this.Skip(from))
            {
                if (predicate(current))
                    return index;
                index++;
            }
            return -1;
        }

        /// <summary>
        /// Finds the index of the first element satisfying some predicate after or at some start index and within the range specified by <paramref name="count"/>.
        /// </summary>
        /// <param name="predicate">The predicate used to test elements.</param>
        /// <param name="from">The start index.</param>
        /// <param name="count">The number of elements in the section to search.</param>
        /// <returns>The index of the first element that satisfies the predicate, or -1 if none exists.</returns>
        public int IndexWhere(Func<T, bool> predicate, int from, int count)
        {
            if (from < 0)
                from = 0;

            int index = from;
            int until = from + count;
            foreach (var current in this.Skip(from))
            {
                if (index >= until)
                    break;
                if (predicate(current))
                    return index;
                index++;
            }
            return -1;
        }

        /// <summary>
        /// Finds the index of the last occurrence of some value in this sequence.
        /// </summary>
        /// <param name="elem">The value to search for.</param>
        /// <returns>The index of the last occurrence of <paramref name="elem"/> if any is found; otherwise, -1.</returns>
        public int LastIndexOf(T elem)
        {
            return LastIndexOf(elem, Count - 1);
        }

        /// <summary>
        /// Finds the index of the last occurrence of some value in this sequence, before or at some end index.
        /// </summary>
        /// <param name="elem">The value to search for.</param>
        /// <param name="end">The end index.</param>
        /// <returns>The index of the last occurrence of <paramref name="elem"/> if any is found; otherwise, -1.</returns>
        public int LastIndexOf(T elem, int end)
        {
            return LastIndexOf(elem, end, Count);
        }

        /// <summary>
        /// Finds the index of the last occurrence of some value in this sequence, before or at some end index and within the range specified by <paramref name="count"/>.
        /// </summary>
        /// <param name="elem">The value to search for.</param>
        /// <param name="end">The end index.</param>
        /// <param name="count">The number of elements in the section to search.</param>
        /// <returns>The index of the last occurrence of <paramref name="elem"/> if any is found; otherwise, -1.</returns>
        public int LastIndexOf(T elem, int end, int count)
        {
            IEqualityComparer<T> comparer = EqualityComparer<T>.Default;
            return LastIndexWhere(current => comparer.Equals(current, elem), end, count);
        }

        /// <summary>
        /// Finds the index of the last element satisfying some predicate.
        /// </summary>
        /// <param name="predicate">The predicate used to test elements.</param>
        /// <returns>The index of the last element that satisfies the predicate, or -1 if none exists.</returns>
        public int LastIndexWhere(Func<T, bool> predicate)
        {
            return LastIndexWhere(predicate, Count - 1);
        }

        /// <summary>
        /// Finds the index of the last element satisfying some predicate before or at some end index.
        /// </summary>
        /// <param name="predicate">The predicate used to test elements.</param>
        /// <param name="end">The end index.</param>
        /// <returns>The index of the last element that satisfies the predicate, or -1 if none exists.</returns>
        public int LastIndexWhere(Func<T, bool> predicate, int end)
        {
            return LastIndexWhere(predicate, end, Count);
        }

        /// <summary>
        /// Finds the index of the last element satisfying some predicate before or at some end index and within the range specified by <paramref name="count"/>.
        /// </summary>
        /// <param name="predicate">The predicate used to test elements.</param>
        /// <param name="end">The end index.</param>
        /// <param name="count">The number of elements in the section to search.</param>
        /// <returns>The index of the last element that satisfies the predicate, or -1 if none exists.</returns>
        public int LastIndexWhere(Func<T, bool> predicate, int end, int count)
        {
            int index = end - count + 1;
            if (index < 0)
                index = 0;

            var lastIndex = -1;

            foreach (var current in this.Skip(index))
            {
                if (index > end)
                    break;
                if (predicate(current))
                    lastIndex = index;
                index++;
            }
            return lastIndex;
        }

        /// <summary>
        /// Iterate over distinct combinations of a given size of this sequence's elements.
        /// </summary>
        /// <example>"abcd".AsSequence().Combinations(2) = ab, ac, ad, bc, bd, cd</example>
        /// <param name="size">The size of each combination.</param>
        /// <returns>An iterator that traverses the possible n-element combinations of this sequence's elements.</returns>
        public IEnumerable<ISequence<T>> Combinations(int size)
        {
            if (size < 0)
                throw new ArgumentOutOfRangeException("size", "size must be a non-negative integer.");

            return new CombinationsIterator(this, size);
        }

        /// <summary>
        /// Iterates over distinct permutations of this sequence.
        /// </summary>
        /// <example>"abb".AsSequence().Permutations() = abb, bab, bba</example>
        /// <returns>An iterator which traverses the distinct permutations of this sequence.</returns>
        public IEnumerable<ISequence<T>> Permutations()
        {
            return new PermutationsIterator(this);
        }

        /// <summary>
        /// Returns a string with all the elements of this sequence.
        /// </summary>
        /// <returns>A string with all the elements of this sequence.</returns>
        public string MkString()
        {
            return MkString("");
        }

        /// <summary>
        /// Returns a string with all the elements of this sequence, using a seperator string.
        /// </summary>
        /// <param name="separator">The separator string.</param>
        /// <returns>A string with all the elements of this sequence.</returns>
        public string MkString(string separator)
        {
            return string.Join(separator,
                Enumerable.Select(this,
                    elem => elem.ToString()));
        }

        /// <summary>
        /// Returns a string that represents this sequence.
        /// </summary>
        /// <returns>A string that represents this sequence.</returns>
        public override string ToString()
        {
            return string.Format("Sequence({0}, ?)", Head);
        }
    }
}
