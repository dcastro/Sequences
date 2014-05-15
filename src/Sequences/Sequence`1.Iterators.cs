using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Sequences
{
    public partial class Sequence<T>
    {

        /**
         * An "iterator block" lets the compiler generate an <see cref="IEnumerator{T}"/> for us.
         * However, that enumerator holds onto the sequence that created it, and doesn't let the GC collect it.
         * 
         * These specialized iterators don't do that.
         * Instead, when they move to a sequence's tail, they replace the reference to the sequence with a reference to its tail,
         * letting GC collect the original sequence.
         * 
         * This lets us, for example, iterate through an infinite sequence for as long as we want, e.g., foreach(var e in Sequence.From(1L)) { }.
         * A compiler-generated iterator would keep a reference to the first sequence - Sequence(1, ?) - and, therefore, to all its tails.
         * Using these iterators, the GC will be able to collect all intermediate sequences as the loop progresses.
         */

        private class TailsIterator<TElem> : IEnumerator<TElem>
        {
            private ISequence<T> _seq;
            private readonly Func<ISequence<T>, TElem> _selector;
            private readonly bool _returnEmptyTail;
            private bool _hasMoved;
            private bool _hasFinished;

            public TailsIterator(ISequence<T> seq, Func<ISequence<T>, TElem> selector, bool returnEmptyTail)
            {
                _seq = seq;
                _selector = selector;
                _returnEmptyTail = returnEmptyTail;
            }

            public bool MoveNext()
            {
                //check if the iterator has reached the end of the sequence 
                if (_hasFinished)
                    return false;

                //move to the sequence's tail on every call but the first
                if (_hasMoved)
                    _seq = _seq.Tail;
                else
                    _hasMoved = true;

                //check if the iterator has reached the end of the sequence
                if (_seq.IsEmpty)
                    _hasFinished = true;

                if (_seq.NonEmpty || _returnEmptyTail)
                {
                    Current = _selector(_seq);
                    return true;
                }

                return false;
            }

            void IEnumerator.Reset()
            {
                throw new NotSupportedException();
            }

            public TElem Current { get; private set; }

            object IEnumerator.Current
            {
                get { return Current; }
            }

            public void Dispose()
            {
            }
        }

        private class IndexIterator : IEnumerator<int>
        {
            private readonly IEnumerator<T> _iter;
            private int _index;
            private bool _hasFinished;

            public IndexIterator(IEnumerable<T> seq)
            {
                _iter = seq.GetEnumerator();
            }

            public bool MoveNext()
            {
                if (_hasFinished || !_iter.TryMoveNext())
                {
                    _hasFinished = true;
                    return false;
                }

                Current = _index++;
                return true;
            }

            void IEnumerator.Reset()
            {
                throw new NotSupportedException();
            }

            public int Current { get; private set; }

            object IEnumerator.Current
            {
                get { return Current; }
            }

            public void Dispose()
            {
            }
        }

        private class SlidingIterator : IEnumerator<ISequence<T>>
        {
            private ISequence<T> _seq;
            private readonly int _size;
            private readonly int _step;

            private bool _hasMoved;
            private bool _hasMoreElems;

            private readonly List<T> _buffer;

            public SlidingIterator(ISequence<T> seq, int size, int step)
            {
                _seq = seq;
                _size = size;
                _step = step;

                _buffer = new List<T>(_size);
                _hasMoreElems = _seq.NonEmpty;
            }

            public bool MoveNext()
            {
                //move "_step" elements on every call but the first
                if (_hasMoved)
                    _seq = _seq.Skip(_step);
                else
                    _hasMoved = true;

                //in addition to checking if the previous iterator had more elements,
                //we also need to check if the sequence is still not empty after advancing "_step" elements
                _hasMoreElems &= _seq.NonEmpty;

                if (!_hasMoreElems)
                    return false;

                //group elements into a buffer
                var iterator = _seq.GetEnumerator();

                for (int i = 0; i < _size && iterator.TryMoveNext(); i++)
                    _buffer.Add(iterator.Current);

                //force the evaluation of the buffer's contents, before we clear the buffer.
                Current = _buffer.AsSequence().Force();
                _buffer.Clear();

                //check if there are any more elements
                _hasMoreElems = iterator.TryMoveNext();

                return true;
            }

            void IEnumerator.Reset()
            {
                throw new NotSupportedException();
            }

            public ISequence<T> Current { get; private set; }

            object IEnumerator.Current
            {
                get { return Current; }
            }

            public void Dispose()
            {
            }
        }

        private class GroupedIterator : IEnumerator<ISequence<T>>
        {
            private ISequence<T> _seq;
            private readonly int _size;

            private bool _hasMoved;

            public GroupedIterator(ISequence<T> seq, int size)
            {
                _seq = seq;
                _size = size;
            }

            public bool MoveNext()
            {
                if (_hasMoved)
                    _seq = _seq.Skip(_size);
                else
                    _hasMoved = true;

                if (_seq.IsEmpty)
                    return false;

                Current = _seq.Take(_size);
                return true;
            }

            void IEnumerator.Reset()
            {
                throw new NotSupportedException();
            }

            public ISequence<T> Current { get; private set; }

            object IEnumerator.Current
            {
                get { return Current; }
            }

            public void Dispose()
            {
            }
        }

        private class CombinationsEnumerable : IEnumerable<ISequence<T>>
        {
            private readonly ISequence<T> _sequence;
            private readonly int _size;

            public CombinationsEnumerable(ISequence<T> sequence, int size)
            {
                _sequence = sequence;
                _size = size;
            }

            public IEnumerator<ISequence<T>> GetEnumerator()
            {
                if (_size == 0)
                    yield return Sequence.Empty<T>();
                else
                    //combine each distinct element (subsequence.Head)
                    //with each possible combination of the remaining elements (tailCombination)
                    foreach (var subSequence in _sequence.NonEmptyTails().Distinct(new CompareByHead()))
                        foreach (var tailCombination in subSequence.Tail.Combinations(_size - 1))
                            yield return new Sequence<T>(subSequence.Head, () => tailCombination);
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            /// <summary>
            /// Compares two sequences by their heads. The implementation assumes input sequences are not empty.
            /// </summary>
            private class CompareByHead : IEqualityComparer<ISequence<T>>
            {
                private readonly IEqualityComparer<T> _headComparer = EqualityComparer<T>.Default;

                public bool Equals(ISequence<T> x, ISequence<T> y)
                {
                    return _headComparer.Equals(x.Head, y.Head);
                }

                public int GetHashCode(ISequence<T> seq)
                {
                    return seq.Head.GetHashCode();
                }
            }
        }

        private class PermutationsEnumerable : IEnumerable<ISequence<T>>
        {
            private readonly ISequence<T> _sequence;

            public PermutationsEnumerable(ISequence<T> sequence)
            {
                _sequence = sequence;
            }

            public IEnumerator<ISequence<T>> GetEnumerator()
            {
                if (_sequence.IsEmpty)
                    yield return Sequence.Empty<T>();
                else
                    foreach (var elem in _sequence.Distinct())
                        foreach (var restPermutation in _sequence.Remove(elem).Permutations())
                            yield return new Sequence<T>(elem, () => restPermutation);
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        private class GenericEnumerable<TElem> : IEnumerable<TElem>
        {
            private readonly Func<IEnumerator<TElem>> _iteratorFactory;

            public GenericEnumerable(Func<IEnumerator<TElem>> iteratorFactory)
            {
                _iteratorFactory = iteratorFactory;
            }

            public IEnumerator<TElem> GetEnumerator()
            {
                return _iteratorFactory();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
    }
}
